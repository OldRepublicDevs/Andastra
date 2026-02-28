using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using OdyTools.Data;
using KitComponent = OdyTools.Data.KitComponent;
using BWM = BioWare.Resource.Formats.BWM.BWM;
using BWMFace = BioWare.Resource.Formats.BWM.BWMFace;
using JetBrains.Annotations;

namespace OdyTools.Windows
{
    public class IndoorMapRenderer
    {
        private readonly List<IndoorMapRoom> _selectedRooms = new List<IndoorMapRoom>();
        private IndoorMap _map;
        private bool _dirty = false;
        private UndoStack _undoStack;
        private KitComponent _cursorComponent;

        // NOTE: We no longer cache *transformed* room walkmeshes (they require deepcopy + transforms).
        // Instead we cache BWM face paths/indices in local space and apply transforms cheaply.
        private readonly Dictionary<int, BWMSurfaceCache> _bwmSurfaceCache = new Dictionary<int, BWMSurfaceCache>();

        /// <summary>
        /// Precomputed geometry for a BWM in *local* space.
        /// This cache exists to avoid rebuilding transformed BWMs (deepcopy + rotate/flip/translate)
        /// on every mouse move / repaint. Transforming is handled by the painter + cheap math.
        /// </summary>
        private class BWMSurfaceCache
        {
            public int BwmObjId { get; set; }
            public List<FaceData> FaceDataList { get; set; }
            public Dictionary<int, int> FaceIdToIndex { get; set; }
            // Unique vertex list for operations like marquee selection (local space).
            public List<Vector3> Vertices { get; set; }
            // Local-space AABB for cheap early-out in picking.
            public Vector3 BbMin { get; set; }
            public Vector3 BbMax { get; set; }
        }

        // Face data structure for rendering (replaces QPainterPath in Qt version)
        private class FaceData
        {
            public Vector3 V1 { get; set; }
            public Vector3 V2 { get; set; }
            public Vector3 V3 { get; set; }
        }

        public bool SnapToGrid { get; set; } = false;
        public bool SnapToHooks { get; set; } = true;
        public float GridSize { get; set; } = 1.0f; // DEFAULT_GRID_SIZE = 1.0
        public float RotationSnap { get; set; } = 15.0f; // DEFAULT_ROTATION_SNAP = 15

        private Vector2 _camPosition = new Vector2(0.0f, 0.0f); // DEFAULT_CAMERA_POSITION_X/Y = 0.0
        private float _camScale = 1.0f; // DEFAULT_CAMERA_ZOOM = 1.0
        private float _camRotation = 0.0f; // DEFAULT_CAMERA_ROTATION = 0.0

        public void MarkDirty()
        {
            _dirty = true;
        }

        public void SelectRoom(IndoorMapRoom room, bool clearExisting = true)
        {
            if (room == null)
            {
                return;
            }

            // Matching Python line 2617: if clear_existing:
            if (clearExisting)
            {
                _selectedRooms.Clear();
            }

            // Matching Python lines 2619-2621:
            // if room in self._selected_rooms:
            //     self._selected_rooms.remove(room)
            // self._selected_rooms.append(room)
            // This moves the room to the end if it's already selected
            if (_selectedRooms.Contains(room))
            {
                _selectedRooms.Remove(room);
            }
            _selectedRooms.Add(room);

            // Matching Python line 2622: self.mark_dirty()
            MarkDirty();
        }

        public void SelectRooms(List<IndoorMapRoom> rooms, bool clearExisting = true)
        {
            if (rooms == null || rooms.Count == 0)
            {
                return;
            }

            // Matching Python line 2626: if clear_existing:
            if (clearExisting)
            {
                _selectedRooms.Clear();
            }

            // Matching Python lines 2628-2631:
            // for room in rooms:
            //     if room in self._selected_rooms:
            //         self._selected_rooms.remove(room)
            //     self._selected_rooms.append(room)
            foreach (var room in rooms)
            {
                if (room != null)
                {
                    if (_selectedRooms.Contains(room))
                    {
                        _selectedRooms.Remove(room);
                    }
                    _selectedRooms.Add(room);
                }
            }

            // Matching Python line 2632: self.mark_dirty()
            MarkDirty();
        }

        public List<IndoorMapRoom> SelectedRooms()
        {
            // Matching Python line 2658: return self._selected_rooms
            // Note: Python returns the list directly, but we return a copy to prevent external modification
            // This matches the intent while maintaining encapsulation
            return new List<IndoorMapRoom>(_selectedRooms);
        }

        public void ClearSelectedRooms()
        {
            // Matching Python line 2661: self._selected_rooms.clear()
            _selectedRooms.Clear();
            // Matching Python line 2662: self.mark_dirty()
            MarkDirty();
        }

        public void SetMap(IndoorMap indoorMap)
        {
            // Matching Python line 283: self._map = indoor_map
            _map = indoorMap;
            // Matching Python line 284: self._bwm_surface_cache.clear()
            _bwmSurfaceCache.Clear();
            // Matching Python line 285: self.mark_dirty()
            MarkDirty();
        }

        /// <summary>
        /// Get (or build) cached local-space geometry for a BWM.
        /// </summary>
        [CanBeNull]
        private BWMSurfaceCache GetBwmSurfaceCache([CanBeNull] BWM bwm)
        {
            if (bwm == null)
            {
                return null;
            }

            // Use RuntimeHelpers.GetHashCode to get object identity (similar to Python's id())
            int key = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(bwm);
            if (_bwmSurfaceCache.TryGetValue(key, out BWMSurfaceCache cached))
            {
                return cached;
            }

            // Build face data (local space) and identity->index map
            var faceDataList = new List<FaceData>();
            var faceIdToIndex = new Dictionary<int, int>();

            for (int idx = 0; idx < bwm.Faces.Count; idx++)
            {
                BWMFace face = bwm.Faces[idx];
                faceDataList.Add(new FaceData
                {
                    V1 = face.V1,
                    V2 = face.V2,
                    V3 = face.V3
                });
                // Use RuntimeHelpers.GetHashCode for face identity (similar to Python's id(face))
                int faceId = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(face);
                faceIdToIndex[faceId] = idx;
            }

            // Vertex list + AABB for early rejection
            List<Vector3> verts = bwm.Vertices();
            Vector3 bbmin;
            Vector3 bbmax;

            if (verts != null && verts.Count > 0)
            {
                bbmin = new Vector3(
                    verts.Min(v => v.X),
                    verts.Min(v => v.Y),
                    verts.Min(v => v.Z)
                );
                bbmax = new Vector3(
                    verts.Max(v => v.X),
                    verts.Max(v => v.Y),
                    verts.Max(v => v.Z)
                );
            }
            else
            {
                bbmin = Vector3.Zero;
                bbmax = Vector3.Zero;
            }

            cached = new BWMSurfaceCache
            {
                BwmObjId = key,
                FaceDataList = faceDataList,
                FaceIdToIndex = faceIdToIndex,
                Vertices = verts ?? new List<Vector3>(),
                BbMin = bbmin,
                BbMax = bbmax
            };

            _bwmSurfaceCache[key] = cached;
            return cached;
        }

        public void SetUndoStack(UndoStack undoStack)
        {
            // Matching Python line 2607: self._undo_stack = undo_stack
            _undoStack = undoStack;
        }

        public UndoStack GetUndoStack()
        {
            return _undoStack;
        }

        public void SetCursorComponent(KitComponent component)
        {
            // Matching Python line 2610: self.cursor_component = component
            _cursorComponent = component;
            // Matching Python line 2611: self.mark_dirty()
            MarkDirty();
        }

        public KitComponent GetCursorComponent()
        {
            return _cursorComponent;
        }

        public delegate void StatusCallback(Vector2? position, HashSet<int> mouseButtons, HashSet<int> keys);

        private StatusCallback _statusCallback;

        public void SetStatusCallback(StatusCallback callback)
        {
            // Matching Python line 2614: self._status_callback = callback
            _statusCallback = callback;
        }

        public void SetSnapToGrid(bool enabled)
        {
            SnapToGrid = enabled;
        }

        public void SetSnapToHooks(bool enabled)
        {
            SnapToHooks = enabled;
        }

        public void SetShowGrid(bool enabled)
        {
            // ShowGrid: applied when grid rendering is implemented
            MarkDirty();
        }

        public void SetHideMagnets(bool enabled)
        {
            // HideMagnets: applied when magnet rendering is implemented
            MarkDirty();
        }

        public void SetMaterialColors(Dictionary<BioWare.Common.SurfaceMaterial, object> materialColors)
        {
            // Material colors: stored when material rendering is fully implemented
            MarkDirty();
        }

        public void SetColorizeMaterials(bool enabled)
        {
            // ColorizeMaterials: applied when material rendering is implemented
            MarkDirty();
        }

        public IndoorMap GetMap()
        {
            return _map;
        }

        public void SetGridSize(float size)
        {
            GridSize = size;
        }

        public void SetRotationSnap(float snap)
        {
            RotationSnap = snap;
        }

        public float CameraZoom()
        {
            return _camScale;
        }

        public void SetCameraZoom(float zoom)
        {
            // Matching Python: self._cam_scale = max(MIN_CAMERA_ZOOM, min(zoom, MAX_CAMERA_ZOOM))
            _camScale = Math.Max(0.1f, Math.Min(zoom, 10.0f)); // MIN_CAMERA_ZOOM = 0.1, MAX_CAMERA_ZOOM = 10.0
            MarkDirty();
        }

        public Vector2 CameraPosition()
        {
            return _camPosition;
        }

        public void SetCameraPosition(float x, float y)
        {
            _camPosition = new Vector2(x, y);
            MarkDirty();
        }

        public float CameraRotation()
        {
            return _camRotation;
        }

        public void SetCameraRotation(float radians)
        {
            _camRotation = radians;
            MarkDirty();
        }

        public void ZoomInCamera(float zoom)
        {
            // Matching Python: self.set_camera_zoom(self._cam_scale + zoom)
            SetCameraZoom(_camScale + zoom);
        }
    }
}

