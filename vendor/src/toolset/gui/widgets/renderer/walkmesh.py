from __future__ import annotations

import math

from copy import copy
from typing import TYPE_CHECKING, Generic, NamedTuple, TypeVar

import qtpy

from qtpy.QtCore import (
    QPoint,
    QPointF,
    QRect,
    QRectF,
    QTimer,
    Qt,
    Signal,  # pyright: ignore[reportPrivateImportUsage]
)
from qtpy.QtGui import QColor, QCursor, QImage, QPainter, QPainterPath, QPalette, QPen, QPixmap, QTransform
from qtpy.QtWidgets import QApplication, QWidget

from pykotor.resource.formats.bwm import BWM
from pykotor.resource.formats.lyt import LYT
from pykotor.resource.formats.tpc import TPCTextureFormat
from pykotor.resource.generics.are import ARENorthAxis
from pykotor.resource.generics.git import (
    GITCamera,
    GITCreature,
    GITDoor,
    GITEncounter,
    GITEncounterSpawnPoint,
    GITPlaceable,
    GITSound,
    GITStore,
    GITTrigger,
    GITWaypoint,
)
from toolset.utils.misc import clamp
from utility.common.geometry import Vector2, Vector3
from utility.error_handling import assert_with_variable_trace

if TYPE_CHECKING:
    from qtpy.QtGui import QFocusEvent, QKeyEvent, QMouseEvent, QPaintEvent, QWheelEvent

    from pykotor.resource.formats.bwm.bwm_data import BWMFace
    from pykotor.resource.formats.lyt.lyt_data import LYTRoom
    from pykotor.resource.formats.tpc.tpc_data import TPC, TPCMipmap
    from pykotor.resource.generics.are import ARE
    from pykotor.resource.generics.git import GIT, GITInstance
    from pykotor.resource.generics.pth import PTH
    from utility.common.geometry import SurfaceMaterial

T = TypeVar("T")


class GeomPoint(NamedTuple):
    instance: GITInstance
    point: Vector3


class EncounterSpawnPoint(NamedTuple):
    encounter: GITEncounter
    spawn: GITEncounterSpawnPoint


class WalkmeshCamera:
    def __init__(self):
        self._position: Vector2 = Vector2.from_null()
        self._rotation: float = 0.0
        self._zoom: float = 1.0

    def position(self) -> Vector2:
        return copy(self._position)

    def rotation(self) -> float:
        return self._rotation

    def zoom(self) -> float:
        return self._zoom

    def set_position(self, x: float, y: float):
        self._position.x = x
        self._position.y = y

    def nudge_position(self, x: float, y: float):
        self._position.x += x
        self._position.y += y

    def set_rotation(self, rotation: float):
        self._rotation = rotation

    def nudge_rotation(self, rotation: float):
        self._rotation += rotation

    def set_zoom(self, zoom: float):
        self._zoom = clamp(zoom, 0.1, 100)

    def nudge_zoom(self, zoomFactor: float):
        new_zoom = self._zoom * zoomFactor
        self._zoom = clamp(new_zoom, 0.1, 100)


class WalkmeshSelection(Generic[T]):
    def __init__(self):
        self._selection: list[T] = []

    def remove(self, element: T):
        self._selection.remove(element)

    def last(self) -> T | None:
        return self._selection[-1] if self._selection else None

    def count(self) -> int:
        return len(self._selection)

    def isEmpty(self) -> bool:
        return len(self._selection) == 0

    def all(self) -> list[T]:
        return copy(self._selection)

    def get(self, index: int) -> T:
        return self._selection[index]

    def clear(self):
        self._selection.clear()

    def select(self, elements: list[T], *, clear_existing: bool = True):
        if clear_existing:
            self._selection.clear()
        self._selection.extend(elements)


class WalkmeshRenderer(QWidget):
    sig_mouse_moved = Signal(object, object, object, object)  # screen coords, screen delta, mouse, keys  # pyright: ignore[reportPrivateImportUsage]
    """Signal emitted when mouse is moved over the widget."""

    sig_mouse_scrolled = Signal(object, object, object)  # screen delta, mouse, keys  # pyright: ignore[reportPrivateImportUsage]
    """Signal emitted when mouse is scrolled over the widget."""

    sig_mouse_released = Signal(object, object, object)  # screen coords, mouse, keys  # pyright: ignore[reportPrivateImportUsage]
    """Signal emitted when a mouse button is released after being pressed on the widget."""

    sig_mouse_pressed = Signal(object, object, object)  # screen coords, mouse, keys  # pyright: ignore[reportPrivateImportUsage]
    """Signal emitted when a mouse button is pressed on the widget."""

    sig_key_pressed = Signal(object, object)  # mouse keys  # pyright: ignore[reportPrivateImportUsage]
    sig_key_released = Signal(object, object)  # mouse keys  # pyright: ignore[reportPrivateImportUsage]
    sig_instance_hovered = Signal(object)  # instance  # pyright: ignore[reportPrivateImportUsage]
    sig_instance_pressed = Signal(object)  # instance  # pyright: ignore[reportPrivateImportUsage]

    def __init__(self, parent: QWidget):
        """Initializes the WalkmeshViewer widget.

        Args:
        ----
            parent (QWidget): The parent widget
        """
        super().__init__(parent)

        self._walkmeshes: list[BWM] = []
        self._layout: LYT | None = None  # Store LYT layout for room boundary rendering
        self._git: GIT | None = None
        self._pth: PTH | None = None
        self._are: ARE | None = None
        self._minimap_image: QImage | None = None

        # Min/Max points and lengths for each axis
        self._bbmin: Vector3 = Vector3.from_null()
        self._bbmax: Vector3 = Vector3.from_null()
        self._world_size: Vector3 = Vector3.from_null()

        self.camera: WalkmeshCamera = WalkmeshCamera()
        self.instance_selection: WalkmeshSelection[GITInstance] = WalkmeshSelection()
        self.geometry_selection: WalkmeshSelection[GeomPoint] = WalkmeshSelection()
        self.spawn_selection: WalkmeshSelection[EncounterSpawnPoint] = WalkmeshSelection()

        self._mouse_prev: Vector2 = Vector2(self.cursor().pos().x(), self.cursor().pos().y())
        self._walkmesh_face_cache: dict[BWMFace, QPainterPath] | None = None

        self.highlight_on_hover: bool = False
        self.highlight_boundaries: bool = True
        self.hide_walkmesh_edges: bool = False
        self.hide_geom_points: bool = True
        self.instance_filter: str = ""
        self.hide_creatures: bool = True
        self.hide_placeables: bool = True
        self.hide_doors: bool = True
        self.hide_stores: bool = True
        self.hide_sounds: bool = True
        self.hide_triggers: bool = True
        self.hide_encounters: bool = True
        self.hide_waypoints: bool = True
        self.hide_cameras: bool = True
        self.hide_spawn_points: bool = True

        self.material_colors: dict[SurfaceMaterial, QColor] = {}
        self.default_material_color: QColor = QColor(255, 0, 255)
        self._highlighted_face: BWMFace | None = None
        self._highlighted_edge: int | None = None

        self._keys_down: set[int | Qt.Key] = set()
        self._mouse_down: set[int | Qt.MouseButton] = set()

        self._pixmap_creature: QPixmap = QPixmap(":/images/icons/k1/creature.png")
        self._pixmap_door: QPixmap = QPixmap(":/images/icons/k1/door.png")
        self._pixmap_placeable: QPixmap = QPixmap(":/images/icons/k1/placeable.png")
        self._pixmap_merchant: QPixmap = QPixmap(":/images/icons/k1/merchant.png")
        self._pixmap_waypoint: QPixmap = QPixmap(":/images/icons/k1/waypoint.png")
        self._pixmap_sound: QPixmap = QPixmap(":/images/icons/k1/sound.png")
        self._pixmap_trigger: QPixmap = QPixmap(":/images/icons/k1/trigger.png")
        self._pixmap_encounter: QPixmap = QPixmap(":/images/icons/k1/encounter.png")
        self._pixmap_camera: QPixmap = QPixmap(":/images/icons/k1/camera.png")

        self._instances_under_mouse: list[GITInstance] = []
        self._geom_points_under_mouse: list[GeomPoint] = []
        self._spawn_points_under_mouse: list[EncounterSpawnPoint] = []

        self._path_nodes_under_mouse: list[Vector2] = []
        self.path_selection: WalkmeshSelection[Vector2] = WalkmeshSelection()
        self._path_node_size: float = 0.3
        self._path_edge_width: float = 0.2

        self._loop()

    def keys_down(self) -> set[int | Qt.Key]:
        return self._keys_down

    def mouse_down(self) -> set[int | Qt.MouseButton]:
        return self._mouse_down

    def _loop(self):
        """The render loop."""
        self.repaint()
        QTimer.singleShot(33, self._loop)

    def reset_buttons_down(self):
        self._mouse_down.clear()

    def reset_keys_down(self):
        self._keys_down.clear()

    def reset_all_down(self):
        self._mouse_down.clear()
        self._keys_down.clear()

    def set_walkmeshes(self, walkmeshes: list[BWM]):
        """Sets the list of walkmeshes to be rendered.

        Args:
        ----
            walkmeshes: The list of walkmeshes.
        """
        self._walkmeshes = walkmeshes
        self._highlighted_face = None
        self._highlighted_edge = None
        # Keep bounds in sync so callers can immediately `center_camera()` reliably.
        # This mirrors engine-side behavior where the active area map is initialized
        # with the current area's map parameters before any drawing occurs.
        self.update_walkmesh_display()

    def setHighlightedTrans(self, face: BWMFace | None, edge: int | None):
        """Highlight a transition edge for the given face.

        This is used by the BWM editor when selecting a transition entry.
        If both face and edge are None, highlighting is cleared.
        """
        self._highlighted_face = face
        self._highlighted_edge = edge
        self.update()

    def generate_walkmeshes(self, layout: LYT):
        """Generate walkmesh based on the current room layout."""
        # Logic to generate walkmesh from layout
        self._walkmeshes = []  # Clear existing walkmeshes
        for room in layout.rooms:
            # Generate walkmesh for each room
            walkmesh = self.create_walkmesh_for_room(room)
            self._walkmeshes.append(walkmesh)
        self.update_walkmesh_display()

    def create_walkmesh_for_room(self, room: LYTRoom) -> BWM:
        """Create a walkmesh for a given room."""
        # Placeholder logic for creating a walkmesh
        # Replace with actual walkmesh generation logic
        walkmesh = BWM()
        # Add faces to walkmesh based on room dimensions
        return walkmesh

    def update_walkmesh_display(self):
        self.repaint()
        self._bbmin = Vector3(1000000, 1000000, 1000000)
        self._bbmax = Vector3(-1000000, -1000000, -1000000)
        for walkmesh in self._walkmeshes:
            bbmin, bbmax = walkmesh.box()
            self._bbmin.x = min(bbmin.x, self._bbmin.x)
            self._bbmin.y = min(bbmin.y, self._bbmin.y)
            self._bbmin.z = min(bbmin.z, self._bbmin.z)
            self._bbmax.x = max(bbmax.x, self._bbmax.x)
            self._bbmax.y = max(bbmax.y, self._bbmax.y)
            self._bbmax.z = max(bbmax.z, self._bbmax.z)

        self._world_size.x = math.fabs(self._bbmax.x - self._bbmin.x)
        self._world_size.y = math.fabs(self._bbmax.y - self._bbmin.y)
        self._world_size.z = math.fabs(self._bbmax.z - self._bbmin.z)

        # Reset camera
        self.camera.set_zoom(1.0)

        # Erase the cache so it will be rebuilt
        self._walkmesh_face_cache = None

    def set_git(self, git: GIT):
        self._git = git

    def set_layout(self, layout: LYT):
        """Set the LYT layout for room boundary rendering.

        Args:
        ----
            layout: The LYT layout containing room definitions
        """
        self._layout = layout

    def set_pth(self, pth: PTH):
        self._pth = pth

    def set_minimap(self, are: ARE, tpc: TPC):
        self._are = are

        # Convert the TPC to RGB format and get the first mipmap
        tpc.convert(TPCTextureFormat.RGB)  # Modifies TPC in place, returns None
        get_result: TPCMipmap = tpc.get(0, 0)
        tpc_rgb_data: bytearray = get_result.data
        image = QImage(bytes(tpc_rgb_data), get_result.width, get_result.height, QImage.Format.Format_RGB888)
        crop: QRect = QRect(0, 0, 435, 256)
        self._minimap_image = image.copy(crop)

    def snap_camera_to_point(
        self,
        point: Vector2 | Vector3,
        zoom: int = 8,
    ):
        self.camera.set_position(point.x, point.y)
        self.camera.set_zoom(zoom)

    def do_cursor_lock(
        self,
        mutable_screen: Vector2,
    ):
        """Reset the cursor to the center of the screen to prevent it from going off screen.

        Used with the FreeCam and drag camera movements and drag rotations.
        """
        global_old_pos: QPoint = self.mapToGlobal(QPoint(int(self._mouse_prev.x), int(self._mouse_prev.y)))
        QCursor.setPos(global_old_pos)
        local_old_pos: QPoint = self.mapFromGlobal(QPoint(global_old_pos.x(), global_old_pos.y()))
        mutable_screen.x = local_old_pos.x()
        mutable_screen.y = local_old_pos.y()

    def to_render_coords(
        self,
        x: float,
        y: float,
    ) -> Vector2:
        """Returns a screen-space coordinates coverted from the specified world-space coordinates.

        The origin of the screen-space coordinates is the top-left of the WalkmeshRenderer widget.
        """
        cos: float = math.cos(self.camera.rotation())
        sin: float = math.sin(self.camera.rotation())
        x -= self.camera.position().x
        y -= self.camera.position().y
        x2: float = (x * cos - y * sin) * self.camera.zoom() + self.width() / 2
        y2: float = (x * sin + y * cos) * self.camera.zoom() + self.height() / 2
        return Vector2(x2, y2)

    def to_world_coords(
        self,
        x: float,
        y: float,
    ) -> Vector3:
        """Returns the world-space coordinates converted from the specified screen-space coordinates.

        The Z component is calculated using the X/Y components and the walkmesh
        face the mouse is over. If there is no face underneath the mouse, the Z component is set to zero.
        """
        y = self.height() - y
        cos: float = math.cos(self.camera.rotation())
        sin: float = math.sin(self.camera.rotation())
        x = (x - self.width() / 2) / self.camera.zoom()
        y = (y - self.height() / 2) / self.camera.zoom()
        x2: float = x * cos - y * sin + self.camera.position().x
        y2: float = x * sin + y * cos + self.camera.position().y

        z: float = self.get_z_coord(x2, y2)

        return Vector3(x2, y2, z)

    def to_world_delta(
        self,
        x: float,
        y: float,
    ) -> Vector2:
        """Returns the coordinates representing a change in world-space.

        This is convereted from coordinates representing a
        change in screen-space, such as the delta paramater given in a mouseMove event.
        """
        cos: float = math.cos(-self.camera.rotation())
        sin: float = math.sin(-self.camera.rotation())
        x /= self.camera.zoom()
        y /= self.camera.zoom()
        x2: float = x * cos - y * sin
        y2: float = x * sin + y * cos
        return Vector2(x2, -y2)

    def get_z_coord(
        self,
        x: float,
        y: float,
    ) -> float:
        """Returns the Z coordinate based of walkmesh data for the specified point.

        If there are overlapping faces, the walkable face will take priority.
        """
        # We need to find a face in the walkmesh that is underneath the mouse to find the Z
        # We also want to prioritize walkable faces
        # And if we cant find a face, then set the Z to 0.0
        face: BWMFace | None = None
        for walkmesh in self._walkmeshes:
            over: BWMFace | None = walkmesh.faceAt(x, y)
            if over and (face is None or (not face.material.walkable() and over.material.walkable())):
                face = over
        return 0.0 if face is None else face.determine_z(x, y)

    def material_color(self, material: SurfaceMaterial) -> QColor:
        return self.material_colors.get(material, self.default_material_color)

    def instances_under_mouse(self) -> list[GITInstance]:
        return self._instances_under_mouse

    def path_nodes_under_mouse(self) -> list[Vector2]:
        return self._path_nodes_under_mouse

    def geom_points_under_mouse(self) -> list[GeomPoint]:
        return self._geom_points_under_mouse

    def spawn_points_under_mouse(self) -> list[EncounterSpawnPoint]:
        return self._spawn_points_under_mouse

    def is_instance_visible(self, instance: GITInstance) -> bool | None:
        retBool: bool | None = None
        if isinstance(instance, GITCamera):
            retBool = not self.hide_cameras
        elif isinstance(instance, GITCreature):
            retBool = not self.hide_creatures
        elif isinstance(instance, GITDoor):
            retBool = not self.hide_doors
        elif isinstance(instance, GITEncounter):
            retBool = not self.hide_encounters
        elif isinstance(instance, GITPlaceable):
            retBool = not self.hide_placeables
        elif isinstance(instance, GITSound):
            retBool = not self.hide_sounds
        elif isinstance(instance, GITStore):
            retBool = not self.hide_stores
        elif isinstance(instance, GITTrigger):
            retBool = not self.hide_triggers
        elif isinstance(instance, GITWaypoint):
            retBool = not self.hide_waypoints
        return retBool

    def instance_pixmap(self, instance: GITInstance) -> QPixmap:
        retPixmap: QPixmap = QPixmap()
        if isinstance(instance, GITCamera):
            retPixmap = self._pixmap_camera
        if isinstance(instance, GITCreature):
            retPixmap = self._pixmap_creature
        if isinstance(instance, GITDoor):
            retPixmap = self._pixmap_door
        if isinstance(instance, GITEncounter):
            retPixmap = self._pixmap_encounter
        if isinstance(instance, GITPlaceable):
            retPixmap = self._pixmap_placeable
        if isinstance(instance, GITTrigger):
            retPixmap = self._pixmap_trigger
        if isinstance(instance, GITSound):
            retPixmap = self._pixmap_sound
        if isinstance(instance, GITStore):
            retPixmap = self._pixmap_merchant
        if isinstance(instance, GITWaypoint):
            retPixmap = self._pixmap_waypoint
        return retPixmap

    def center_camera(self, *, fill: float = 1.0):
        self.camera.set_position((self._bbmin.x + self._bbmax.x) / 2, (self._bbmin.y + self._bbmax.y) / 2)
        world_w: float = self._world_size.x
        world_h: float = self._world_size.y

        # Guard against empty/degenerate bounds.
        if world_w <= 0 or world_h <= 0:
            self.camera.set_zoom(1.0)
            self.camera.set_rotation(0)
            return

        # If the GIT is being loaded directly after the window opens the widget won't have appropriately resized itself,
        # so we check for this and set the sizes to what it should be by default.
        if self.width() == 100:  # noqa: PLR2004
            screen_w = 520
            screen_h = 507
        else:
            screen_w: int = self.width()
            screen_h: int = self.height()

        scale_w: float = screen_w / world_w if world_w != 0 else 0.0
        scale_h: float = screen_h / world_h if world_h != 0 else 0.0
        camScale: float = min(scale_w, scale_h) * clamp(fill, 0.1, 10.0)

        self.camera.set_zoom(camScale)
        self.camera.set_rotation(0)

    def _build_face(
        self,
        face: BWMFace,
    ) -> QPainterPath:
        v1: Vector2 = Vector2(face.v1.x, face.v1.y)
        v2: Vector2 = Vector2(face.v2.x, face.v2.y)
        v3: Vector2 = Vector2(face.v3.x, face.v3.y)

        path = QPainterPath()
        path.moveTo(v1.x, v1.y)
        path.lineTo(v2.x, v2.y)
        path.lineTo(v3.x, v3.y)
        path.lineTo(v1.x, v1.y)
        path.closeSubpath()

        return path

    def _build_instance_bounds(
        self,
        instance: GITInstance,
    ) -> QPainterPath:
        path = QPainterPath()
        if (isinstance(instance, (GITEncounter, GITTrigger))) and len(instance.geometry) > 0:
            path.moveTo(instance.position.x + instance.geometry[0].x, instance.position.y + instance.geometry[0].y)  # type: ignore[]
            for point in instance.geometry[1:]:
                path.lineTo(instance.position.x + point.x, instance.position.y + point.y)  # type: ignore[]
            path.lineTo(instance.position.x + instance.geometry[0].x, instance.position.y + instance.geometry[0].y)  # type: ignore[]
        return path

    def _build_instance_bounds_points(
        self,
        instance: GITInstance,
    ) -> QPainterPath:
        path = QPainterPath()
        if isinstance(instance, (GITTrigger, GITEncounter)):
            for point in instance.geometry:
                size: float = 4 / self.camera.zoom()
                path.addEllipse(QPointF(instance.position.x + point.x, instance.position.y + point.y), size, size)
        return path

    def _draw_image(  # noqa: PLR0913
        self,
        painter: QPainter,
        pixmap: QPixmap,
        x: float,
        y: float,
        rotation: float,
        scale: float,
    ):
        painter.save()
        painter.translate(x, y)
        painter.rotate(math.degrees(rotation))
        painter.scale(-1, 1)

        source = QRectF(0, 0, pixmap.width(), pixmap.height())
        true_width, true_height = pixmap.width() * scale, pixmap.height() * scale
        painter.drawPixmap(QRectF(-true_width / 2, -true_height / 2, true_width, true_height), pixmap, source)
        painter.restore()

    # region Events
    def paintEvent(
        self,
        e: QPaintEvent,  # pyright: ignore[reportIncompatibleMethodOverride]
    ):
        # Build walkmesh faces cache
        if self._walkmesh_face_cache is None:
            self._walkmesh_face_cache = {}
            for walkmesh in self._walkmeshes:
                # We want to draw walkable faces over the unwalkable ones
                for face in walkmesh.walkable_faces():
                    self._walkmesh_face_cache[face] = self._build_face(face)
                for face in walkmesh.unwalkable_faces():
                    self._walkmesh_face_cache[face] = self._build_face(face)

        # Create the transform object using the camera values
        transform = QTransform()
        transform.translate(self.width() / 2, self.height() / 2)
        transform.rotate(math.degrees(self.camera.rotation()))
        transform.scale(self.camera.zoom(), -self.camera.zoom())
        transform.translate(-self.camera.position().x, -self.camera.position().y)

        # Fill the screen with black
        painter = QPainter(self)
        painter.setBrush(QColor(0))
        painter.drawRect(0, 0, self.width(), self.height())

        # Draw the faces of the walkmesh (cached).
        painter.setTransform(transform)

        # Draw the minimap
        # References (engine behavior mirrored):
        # - `vendor/swkotor.c:L194248-L194331` initializes the area map from ARE "Map" struct fields.
        # - `vendor/xoreos/src/engines/kotor/gui/ingame/minimap.cpp:L51-L77` maps world -> minimap coords
        #   based on NorthAxis and the ARE map/world points.
        #
        # MATHEMATICAL DERIVATION:
        # xoreos' Minimap::setPosition converts world -> normalized minimap coordinates:
        #   mapPos.x = (world.x - worldPoint1.x) * scaleX + mapPoint1.x
        #   mapPos.y = (world.y - worldPoint1.y) * scaleY + mapPoint1.y
        # Where:
        #   scaleX = (mapPoint1.x - mapPoint2.x) / (worldPoint1.x - worldPoint2.x)
        #   scaleY = (mapPoint1.y - mapPoint2.y) / (worldPoint1.y - worldPoint2.y)
        #
        # For rendering, we need the INVERSE: map texture coordinates -> world coordinates
        # Solving for world.x:
        #   mapPos.x - mapPoint1.x = (world.x - worldPoint1.x) * scaleX
        #   (mapPos.x - mapPoint1.x) / scaleX = world.x - worldPoint1.x
        #   world.x = worldPoint1.x + (mapPos.x - mapPoint1.x) / scaleX
        #
        # Substituting scaleX:
        #   world.x = worldPoint1.x + (mapPos.x - mapPoint1.x) * (worldPoint1.x - worldPoint2.x) / (mapPoint1.x - mapPoint2.x)
        #
        # For texture origin (mapPos = 0,0):
        #   world.x = worldPoint1.x + (0 - mapPoint1.x) * (worldPoint1.x - worldPoint2.x) / (mapPoint1.x - mapPoint2.x)
        #   world.x = worldPoint1.x - mapPoint1.x * (worldPoint1.x - worldPoint2.x) / (mapPoint1.x - mapPoint2.x)
        #
        # For texture end (mapPos = 1,1):
        #   world.x = worldPoint1.x + (1 - mapPoint1.x) * (worldPoint1.x - worldPoint2.x) / (mapPoint1.x - mapPoint2.x)
        #
        # However, NorthAxis changes which world axis maps to which minimap axis:
        #   Case 0,1: X/Y map directly to X/Y
        #   Case 2,3: X/Y are swapped (world.x maps to map.y, world.y maps to map.x)
        #
        # For our rendering, we need to handle NorthAxis to determine coordinate mapping.
        if self._are and self._minimap_image:
            map_pt1_x: float = self._are.map_point_1.x
            map_pt1_y: float = self._are.map_point_1.y
            map_pt2_x: float = self._are.map_point_2.x
            map_pt2_y: float = self._are.map_point_2.y
            world_pt1_x: float = self._are.world_point_1.x
            world_pt1_y: float = self._are.world_point_1.y
            world_pt2_x: float = self._are.world_point_2.x
            world_pt2_y: float = self._are.world_point_2.y

            # Calculate differences
            map_dx: float = map_pt1_x - map_pt2_x
            map_dy: float = map_pt1_y - map_pt2_y
            world_dx: float = world_pt1_x - world_pt2_x
            world_dy: float = world_pt1_y - world_pt2_y

            # Calculate scale factors (from reone's getMapPosition formula)
            # scaleX = (mapPoint1.x - mapPoint2.x) / (worldPoint1.x - worldPoint2.x)
            # But we need the inverse, so: worldScaleX = (worldPoint1.x - worldPoint2.x) / (mapPoint1.x - mapPoint2.x)
            world_scale_x: float = world_dx / map_dx if abs(map_dx) > 0.0001 else 0.0
            world_scale_y: float = world_dy / map_dy if abs(map_dy) > 0.0001 else 0.0

            # Calculate where texture origin (0,0) maps to in world space
            # world.x = worldPoint1.x + (0 - mapPoint1.x) * worldScaleX
            origin_x: float = world_pt1_x - map_pt1_x * world_scale_x
            origin_y: float = world_pt1_y - map_pt1_y * world_scale_y

            # Calculate where texture end (1,1) maps to in world space
            # world.x = worldPoint1.x + (1 - mapPoint1.x) * worldScaleX
            end_x: float = world_pt1_x + (1.0 - map_pt1_x) * world_scale_x
            end_y: float = world_pt1_y + (1.0 - map_pt1_y) * world_scale_y

            # Handle NorthAxis swapping (cases 2,3 swap X/Y).
            # Reference: `vendor/xoreos/src/engines/kotor/gui/ingame/minimap.cpp:L55-L71`
            north_axis = self._are.north_axis
            if north_axis in (ARENorthAxis.PositiveX, ARENorthAxis.NegativeX):
                # For swapped case: world.x maps to map.y, world.y maps to map.x
                # So we need to swap our calculated coordinates
                origin_x, origin_y = origin_y, origin_x
                end_x, end_y = end_y, end_x

            # Create rectangle (handle negative scales - inverted mappings)
            min_x: float = min(origin_x, end_x)
            max_x: float = max(origin_x, end_x)
            min_y: float = min(origin_y, end_y)
            max_y: float = max(origin_y, end_y)

            # Flip image if scales are negative (inverted mapping)
            image_to_draw: QImage = self._minimap_image
            if world_scale_x < 0:
                image_to_draw = image_to_draw.mirrored(True, False)
            if world_scale_y < 0:
                image_to_draw = image_to_draw.mirrored(False, True)

            targetRect = QRectF(QPointF(min_x, min_y), QPointF(max_x, max_y))
            painter.drawImage(targetRect, image_to_draw)

        # Get palette colors for edges
        app = QApplication.instance()
        if app is not None and isinstance(app, QApplication):
            palette = app.palette()
            edge_color = palette.color(QPalette.ColorRole.Mid)
            if not edge_color.isValid():
                edge_color = palette.color(QPalette.ColorRole.Shadow)
            edge_color.setAlpha(120)
        else:
            edge_color = QColor(10, 10, 10, 120)

        pen: QPen = (
            QPen(Qt.PenStyle.NoPen)
            if self.hide_walkmesh_edges
            else QPen(
                edge_color,
                1 / self.camera.zoom(),
                Qt.PenStyle.SolidLine,
            )
        )
        painter.setPen(pen)
        for face, path in self._walkmesh_face_cache.items():
            painter.setBrush(self.material_color(face.material))
            painter.drawPath(path)

        # Get palette colors for highlights
        app = QApplication.instance()
        if app is not None and isinstance(app, QApplication):
            palette = app.palette()
            from toolset.gui.common.palette_helpers import get_semantic_colors

            colors = get_semantic_colors()
            highlight_color = QColor(colors.get("warning", "#ffc800"))
            edge_highlight_color = QColor(colors.get("warning", "#ffff00"))
            boundary_color = QColor(colors.get("error", "#ff0000"))
        else:
            highlight_color = QColor(255, 200, 0)
            edge_highlight_color = QColor(255, 255, 0)
            boundary_color = QColor(255, 0, 0)

        # Highlight a specific face/edge if requested
        if self._highlighted_face is not None:
            painter.setBrush(Qt.BrushStyle.NoBrush)
            painter.setPen(QPen(highlight_color, 2 / self.camera.zoom()))
            path = self._walkmesh_face_cache.get(self._highlighted_face)
            if path is not None:
                painter.drawPath(path)

            # Edge-specific highlight
            if self._highlighted_edge is not None and self._highlighted_edge in (1, 2, 3):
                v1, v2, v3 = self._highlighted_face.v1, self._highlighted_face.v2, self._highlighted_face.v3
                edge_points = {
                    1: (v1, v2),
                    2: (v2, v3),
                    3: (v1, v3),
                }
                p1, p2 = edge_points[self._highlighted_edge]
                painter.setPen(QPen(edge_highlight_color, 3 / self.camera.zoom()))
                painter.drawLine(QPointF(p1.x, p1.y), QPointF(p2.x, p2.y))

        if self.highlight_boundaries:
            for walkmesh in self._walkmeshes:
                for face in walkmesh.walkable_faces():
                    painter.setPen(QPen(boundary_color, 3 / self.camera.zoom()))
                    path = QPainterPath()
                    if face.trans1 is not None:
                        path.moveTo(face.v1.x, face.v1.y)
                        path.lineTo(face.v2.x, face.v2.y)
                    if face.trans2 is not None:
                        path.moveTo(face.v2.x, face.v2.y)
                        path.lineTo(face.v3.x, face.v3.y)
                    if face.trans3 is not None:
                        path.moveTo(face.v1.x, face.v1.y)
                        path.lineTo(face.v3.x, face.v3.y)
                    painter.drawPath(path)

        # Draw room boundaries and names from LYT layout
        if self._layout is not None:
            # Create a mapping from room model to walkmesh
            # Walkmeshes are loaded in the same order as rooms in load_layout
            room_to_walkmesh: dict[str, BWM] = {}
            for i, room in enumerate(self._layout.rooms):
                if i < len(self._walkmeshes):
                    room_to_walkmesh[room.model.lower()] = self._walkmeshes[i]

            # Draw boundaries and names for each room
            for room in self._layout.rooms:
                walkmesh = room_to_walkmesh.get(room.model.lower())
                if walkmesh is None:
                    continue

                # Get bounding box of walkmesh (already in world space)
                bbmin, bbmax = walkmesh.box()

                # Draw room boundary - just draw the bounding rectangle for the room dimensions
                # Walkmeshes are already in world space, no need to transform by room.position
                app = QApplication.instance()
                if app is not None and isinstance(app, QApplication):
                    palette = app.palette()
                    room_boundary_color = palette.color(QPalette.ColorRole.Link)
                    room_boundary_color.setAlpha(200)
                else:
                    room_boundary_color = QColor(100, 150, 255, 200)
                painter.setPen(QPen(room_boundary_color, 2 / self.camera.zoom(), Qt.PenStyle.SolidLine))
                painter.setBrush(Qt.BrushStyle.NoBrush)

                # Draw rectangle representing the room's dimensions
                room_rect = QRectF(bbmin.x, bbmin.y, bbmax.x - bbmin.x, bbmax.y - bbmin.y)
                painter.drawRect(room_rect)

                # Draw room name in bottom right of room
                room_name = room.model
                # Calculate bottom right position in world coordinates
                # Walkmeshes are already in world space, so bounding box is also in world space
                # Note: Y increases upward in world space, so bottom is min Y
                # For bottom-right corner: max X, min Y
                text_x_world = bbmax.x
                text_y_world = bbmin.y

                # Use painter's transform to convert world coordinates to screen coordinates
                # The painter transform already accounts for camera position, rotation, zoom, and Y flip
                text_world_point = QPointF(text_x_world, text_y_world)
                text_screen_point = painter.transform().map(text_world_point)

                # Reset transform temporarily for text rendering (text should be screen-space)
                painter.save()
                painter.resetTransform()

                # Set up text rendering with palette colors
                app = QApplication.instance()
                if app is not None and isinstance(app, QApplication):
                    palette = app.palette()
                    text_color = palette.color(QPalette.ColorRole.WindowText)
                    bg_color = palette.color(QPalette.ColorRole.Window)
                    bg_color.setAlpha(180)
                else:
                    text_color = QColor(255, 255, 255, 255)
                    bg_color = QColor(0, 0, 0, 180)
                painter.setPen(QPen(text_color, 1))
                painter.setBrush(bg_color)

                # Get text metrics to position it properly (bottom right)
                font = painter.font()
                font.setPointSize(10)
                painter.setFont(font)
                text_rect = painter.fontMetrics().boundingRect(room_name)

                # Position text at bottom right (adjust for text size)
                # text_screen_point.y is screen Y (increases downward after Y flip in transform)
                # For bottom right, we want: x = right edge - text width, y = bottom edge - text height
                text_x_screen_pos = text_screen_point.x() - text_rect.width() - 5
                text_y_screen_pos = text_screen_point.y() - text_rect.height() - 5

                # Draw background rectangle
                bg_rect = QRectF(text_x_screen_pos - 2, text_y_screen_pos - text_rect.height() - 2, text_rect.width() + 4, text_rect.height() + 4)
                painter.drawRect(bg_rect)

                # Draw text
                painter.drawText(int(text_x_screen_pos), int(text_y_screen_pos), room_name)
                painter.restore()

        # Draw the pathfinding nodes and edges
        painter.setOpacity(1.0)
        if self._pth is not None:
            # Get palette colors for path rendering
            app = QApplication.instance()
            if app is not None and isinstance(app, QApplication):
                palette = app.palette()
                from toolset.gui.common.palette_helpers import get_semantic_colors

                colors = get_semantic_colors()
                path_edge_color = palette.color(QPalette.ColorRole.Mid)
                path_node_color = palette.color(QPalette.ColorRole.Mid)
                path_hover_color = palette.color(QPalette.ColorRole.Base)
                path_selected_color = QColor(colors.get("success", "#00ff00"))
            else:
                path_edge_color = QColor(200, 200, 200, 255)
                path_node_color = QColor(200, 200, 200, 255)
                path_hover_color = QColor(255, 255, 255, 255)
                path_selected_color = QColor(0, 255, 0, 255)

            for i, source in enumerate(self._pth):
                painter.setPen(QPen(path_edge_color, self._path_edge_width, Qt.PenStyle.SolidLine))
                for j in self._pth.outgoing(i):
                    target: Vector2 | None = self._pth.get(j.target)
                    assert target is not None, assert_with_variable_trace(target is not None)
                    painter.drawLine(QPointF(source.x, source.y), QPointF(target.x, target.y))

            for point_2d in self._pth:
                painter.setPen(QColor(0, 0, 0, 0))
                painter.setBrush(path_node_color)
                painter.drawEllipse(QPointF(point_2d.x, point_2d.y), self._path_node_size, self._path_node_size)

            for point_2d in self._path_nodes_under_mouse:
                painter.setPen(QColor(0, 0, 0, 0))
                painter.setBrush(path_hover_color)
                painter.drawEllipse(QPointF(point_2d.x, point_2d.y), self._path_node_size, self._path_node_size)

            for point_2d in self.path_selection.all():
                painter.setPen(QColor(0, 0, 0, 0))
                painter.setBrush(path_selected_color)
                painter.drawEllipse(QPointF(point_2d.x, point_2d.y), self._path_node_size, self._path_node_size)

        # Draw the git instances (represented as icons)
        painter.setOpacity(0.6)
        painter.setRenderHint(QPainter.RenderHint.SmoothPixmapTransform, True)
        lower_filter: str = self.instance_filter.lower()
        if self._git is not None:
            for creature in [] if self.hide_creatures else self._git.creatures:
                if creature.resref and lower_filter not in str(creature.resref).lower():
                    continue
                self._draw_image(painter, self._pixmap_creature, creature.position.x, creature.position.y, math.pi + self.camera.rotation(), 1 / 16)

            for door in [] if self.hide_doors else self._git.doors:
                if door.resref and lower_filter not in str(door.resref).lower():
                    continue
                self._draw_image(painter, self._pixmap_door, door.position.x, door.position.y, math.pi + self.camera.rotation(), 1 / 16)

            for placeable in [] if self.hide_placeables else self._git.placeables:
                if placeable.resref and lower_filter not in str(placeable.resref).lower():
                    continue
                self._draw_image(painter, self._pixmap_placeable, placeable.position.x, placeable.position.y, math.pi + self.camera.rotation(), 1 / 16)

            for merchant in [] if self.hide_stores else self._git.stores:
                if merchant.resref and lower_filter not in str(merchant.resref).lower():
                    continue
                self._draw_image(painter, self._pixmap_merchant, merchant.position.x, merchant.position.y, math.pi + self.camera.rotation(), 1 / 16)

            for waypoint in [] if self.hide_waypoints else self._git.waypoints:
                if waypoint.resref and lower_filter not in str(waypoint.resref).lower():
                    continue
                self._draw_image(painter, self._pixmap_waypoint, waypoint.position.x, waypoint.position.y, math.pi + self.camera.rotation(), 1 / 16)

            for sound in [] if self.hide_sounds else self._git.sounds:
                if sound.resref and lower_filter not in str(sound.resref).lower():
                    continue
                self._draw_image(painter, self._pixmap_sound, sound.position.x, sound.position.y, math.pi + self.camera.rotation(), 1 / 16)

            for encounter in [] if self.hide_encounters else self._git.encounters:
                if encounter.resref and lower_filter not in str(encounter.resref).lower():
                    continue
                self._draw_image(painter, self._pixmap_encounter, encounter.position.x, encounter.position.y, math.pi + self.camera.rotation(), 1 / 16)

            for trigger in [] if self.hide_triggers else self._git.triggers:
                if trigger.resref and lower_filter not in str(trigger.resref).lower():
                    continue
                self._draw_image(painter, self._pixmap_trigger, trigger.position.x, trigger.position.y, math.pi + self.camera.rotation(), 1 / 16)

            for camera in [] if self.hide_cameras else self._git.cameras:
                self._draw_image(painter, self._pixmap_camera, camera.position.x, camera.position.y, math.pi + self.camera.rotation(), 1 / 16)

            # Draw encounter spawn points (if enabled)
            if not self.hide_spawn_points:
                app = QApplication.instance()
                if app is not None and isinstance(app, QApplication):
                    palette = app.palette()
                    spawn_color = palette.color(QPalette.ColorRole.Link)
                    spawn_color.setAlpha(180)
                    spawn_arrow_color = QColor(spawn_color)
                    spawn_arrow_color.setAlpha(200)
                else:
                    spawn_color = QColor(180, 200, 255, 180)
                    spawn_arrow_color = QColor(180, 200, 255, 200)

                painter.setOpacity(0.8)
                painter.setPen(Qt.PenStyle.NoPen)
                for encounter in self._git.encounters:
                    for spawn in encounter.spawn_points:
                        painter.setBrush(spawn_color)
                        painter.drawEllipse(QPointF(spawn.x, spawn.y), 3 / self.camera.zoom(), 3 / self.camera.zoom())
                        # Orientation arrow
                        length = 1.0
                        dx = math.sin(spawn.orientation) * length
                        dy = math.cos(spawn.orientation) * length
                        painter.setBrush(Qt.BrushStyle.NoBrush)
                        painter.setPen(QPen(spawn_arrow_color, 0.12))
                        painter.drawLine(QPointF(spawn.x, spawn.y), QPointF(spawn.x + dx, spawn.y + dy))
                        painter.setPen(Qt.PenStyle.NoPen)

        # Get palette colors for highlights
        app = QApplication.instance()
        if app is not None and isinstance(app, QApplication):
            palette = app.palette()
            from toolset.gui.common.palette_helpers import get_semantic_colors

            colors = get_semantic_colors()
            hover_bg = palette.color(QPalette.ColorRole.Base)
            hover_bg.setAlpha(35)
            hover_highlight = QColor(colors.get("success", "#00dc00"))
            hover_highlight.setAlpha(50)
            hover_highlight_pen = QColor(colors.get("success", "#00ff00"))
            hover_highlight_pen.setAlpha(75)
            selected_bg = palette.color(QPalette.ColorRole.Base)
            selected_bg.setAlpha(70)
            selected_pen = palette.color(QPalette.ColorRole.WindowText)
            selected_highlight = QColor(colors.get("success", "#00dc00"))
            selected_highlight.setAlpha(100)
            selected_highlight_pen = QColor(colors.get("success", "#00ff00"))
            selected_highlight_pen.setAlpha(150)
            point_color = palette.color(QPalette.ColorRole.Base)
            point_color.setAlpha(200)
            point_selected = palette.color(QPalette.ColorRole.WindowText)
            success_color = QColor(colors.get("success", "#00ff00"))
        else:
            hover_bg = QColor(255, 255, 255, 35)
            hover_highlight = QColor(0, 220, 0, 50)
            hover_highlight_pen = QColor(0, 255, 0, 75)
            selected_bg = QColor(255, 255, 255, 70)
            selected_pen = QColor(255, 255, 255, 255)
            selected_highlight = QColor(0, 220, 0, 100)
            selected_highlight_pen = QColor(0, 255, 0, 150)
            point_color = QColor(255, 255, 255, 200)
            point_selected = QColor(255, 255, 255, 255)
            success_color = QColor(0, 255, 0, 255)

        # Highlight the first instance that is underneath the mouse
        if self._instances_under_mouse:
            instance: GITInstance = self._instances_under_mouse[0]

            painter.setBrush(hover_bg)
            painter.setPen(Qt.PenStyle.NoPen)
            painter.drawEllipse(QPointF(instance.position.x, instance.position.y), 1, 1)

            # If its a trigger or an encounter, this will draw the geometry stored inside it
            painter.setBrush(hover_highlight)
            painter.setPen(QPen(hover_highlight_pen, 2 / self.camera.zoom()))
            painter.drawPath(self._build_instance_bounds(instance))

        # Highlight first geom point that is underneath the mouse
        if self._geom_points_under_mouse:
            gpoint: GeomPoint = self._geom_points_under_mouse[0]
            point = gpoint.instance.position + gpoint.point

            if not self.hide_geom_points:
                painter.setBrush(point_color)
                painter.setPen(Qt.PenStyle.NoPen)
                painter.drawEllipse(QPointF(point.x, point.y), 4 / self.camera.zoom(), 4 / self.camera.zoom())

        # Highlight first spawn point that is underneath the mouse
        if self._spawn_points_under_mouse and not self.hide_spawn_points:
            sp_ref: EncounterSpawnPoint = self._spawn_points_under_mouse[0]
            sp = sp_ref.spawn
            painter.setBrush(point_color)
            painter.setPen(Qt.PenStyle.NoPen)
            painter.drawEllipse(QPointF(sp.x, sp.y), 4 / self.camera.zoom(), 4 / self.camera.zoom())

        # Highlight selected instances
        for instance in self.instance_selection.all():
            painter.setBrush(selected_bg)
            painter.setPen(QPen(selected_pen, 1 / self.camera.zoom()))
            painter.drawEllipse(QPointF(instance.position.x, instance.position.y), 1, 1)

            # If its a trigger or an encounter, this will draw the geometry stored inside it
            painter.setBrush(selected_highlight)
            painter.setPen(QPen(selected_highlight_pen, 2 / self.camera.zoom()))
            painter.drawPath(self._build_instance_bounds(instance))

            # If its a trigger or an encounter, this will draw the circles at the points making up the geometry
            if not self.hide_geom_points:
                painter.setBrush(success_color)
                painter.setPen(Qt.PenStyle.NoPen)
                painter.drawPath(self._build_instance_bounds_points(instance))

            # Draw an arrow representing the instance rotation (where applicable)
            instance_yaw_value: float | None = instance.yaw()
            if instance_yaw_value is not None:
                l1px: float = instance.position.x + math.cos(instance_yaw_value + math.pi / 2) * 1.1
                l1py: float = instance.position.y + math.sin(instance_yaw_value + math.pi / 2) * 1.1
                l2px: float = instance.position.x + math.cos(instance_yaw_value + math.pi / 2) * 1.3
                l2py: float = instance.position.y + math.sin(instance_yaw_value + math.pi / 2) * 1.3
                painter.setBrush(Qt.BrushStyle.NoBrush)
                painter.setPen(QPen(selected_pen, 0.15))
                painter.drawLine(QPointF(l1px, l1py), QPointF(l2px, l2py))

        # Highlight selected geometry points
        for geom_point in self.geometry_selection.all():
            point: Vector3 = geom_point.point + geom_point.instance.position
            painter.setBrush(point_selected)
            painter.setPen(Qt.PenStyle.NoPen)
            painter.drawEllipse(QPointF(point.x, point.y), 4 / self.camera.zoom(), 4 / self.camera.zoom())

        # Draw selected spawn points
        if not self.hide_spawn_points:
            for sp_ref in self.spawn_selection.all():
                if sp_ref.spawn not in sp_ref.encounter.spawn_points:
                    continue
                sp = sp_ref.spawn
                painter.setBrush(point_selected)
                painter.setPen(Qt.PenStyle.NoPen)
                painter.drawEllipse(QPointF(sp.x, sp.y), 4 / self.camera.zoom(), 4 / self.camera.zoom())
                # Orientation arrow
                length = 1.2
                dx = math.sin(sp.orientation) * length
                dy = math.cos(sp.orientation) * length
                painter.setBrush(Qt.BrushStyle.NoBrush)
                painter.setPen(QPen(QColor(255, 255, 255, 255), 0.15))
                painter.drawLine(QPointF(sp.x, sp.y), QPointF(sp.x + dx, sp.y + dy))

    def wheelEvent(self, e: QWheelEvent):  # pyright: ignore[reportIncompatibleMethodOverride]
        self.sig_mouse_scrolled.emit(Vector2(e.angleDelta().x(), e.angleDelta().y()), self._mouse_down, self._keys_down)

    def mouseMoveEvent(self, e: QMouseEvent):  # pyright: ignore[reportIncompatibleMethodOverride]
        pos = e.pos() if qtpy.QT5 else e.position().toPoint()
        super().mouseMoveEvent(e)
        coords: Vector2 = Vector2(pos.x(), pos.y())
        coords_delta: Vector2 = Vector2(coords.x - self._mouse_prev.x, coords.y - self._mouse_prev.y)
        self.sig_mouse_moved.emit(coords, coords_delta, self._mouse_down, self._keys_down)
        self._mouse_prev = coords  # Always assign mouse_prev after emitting: allows signal handlers (e.g. ModuleDesigner, GITEditor) to handle cursor lock.

        self._instances_under_mouse = []
        self._geom_points_under_mouse = []
        self._spawn_points_under_mouse = []
        self._path_nodes_under_mouse = []

        world: Vector2 = Vector2.from_vector3(self.to_world_coords(coords.x, coords.y))  # Mouse pos in world

        if self._git is not None:
            instances: list[GITInstance] = self._git.instances()
            for instance in instances:
                position = Vector2(instance.position.x, instance.position.y)
                if position.distance(world) <= 1 and self.is_instance_visible(instance):
                    self.sig_instance_hovered.emit(instance)
                    self._instances_under_mouse.append(instance)

                if isinstance(instance, GITEncounter) or (isinstance(instance, GITTrigger) and instance in self.instance_selection.all()):
                    for point in instance.geometry:
                        pworld: Vector2 = Vector2.from_vector3(instance.position + point)
                        if pworld.distance(world) <= 0.5:  # noqa: PLR2004
                            self._geom_points_under_mouse.append(GeomPoint(instance, point))

                if isinstance(instance, GITEncounter) and not self.hide_spawn_points:
                    for spawn in instance.spawn_points:
                        pworld = Vector2(spawn.x, spawn.y)
                        if pworld.distance(world) <= 0.75:  # noqa: PLR2004
                            self._spawn_points_under_mouse.append(EncounterSpawnPoint(instance, spawn))

        if self._pth is not None:
            for point in self._pth:
                if point.distance(world) <= self._path_node_size:
                    self._path_nodes_under_mouse.append(point)

    def focusOutEvent(self, e: QFocusEvent):  # pyright: ignore[reportIncompatibleMethodOverride]
        self._mouse_down.clear()
        self._keys_down.clear()
        super().focusOutEvent(e)

    def mousePressEvent(self, e: QMouseEvent):  # pyright: ignore[reportIncompatibleMethodOverride]
        super().mousePressEvent(e)
        if e is None:
            return
        button: int | Qt.MouseButton = e.button()
        self._mouse_down.add(button)
        coords = (
            Vector2(e.x(), e.y())  # pyright: ignore[reportAttributeAccessIssue]
            if qtpy.QT5
            else Vector2(e.position().toPoint().x(), e.position().toPoint().y())
        )
        self.sig_mouse_pressed.emit(coords, self._mouse_down, self._keys_down)

    def mouseReleaseEvent(self, e: QMouseEvent):  # pyright: ignore[reportIncompatibleMethodOverride]
        super().mouseReleaseEvent(e)
        if e is None:
            return
        button: int | Qt.MouseButton = e.button()
        self._mouse_down.discard(button)
        coords = (
            Vector2(e.x(), e.y())  # pyright: ignore[reportAttributeAccessIssue]
            if qtpy.QT5
            else Vector2(e.position().toPoint().x(), e.position().toPoint().y())
        )
        self.sig_mouse_released.emit(coords, self._mouse_down, self._keys_down)

    def keyPressEvent(self, e: QKeyEvent):  # pyright: ignore[reportIncompatibleMethodOverride]
        super().keyPressEvent(e)
        if e is None:
            return
        key = e.key()
        if e is None:
            return
        self._keys_down.add(key)
        if self.underMouse():
            self.sig_key_pressed.emit(self._mouse_down, self._keys_down)

    def keyReleaseEvent(self, e: QKeyEvent):  # pyright: ignore[reportIncompatibleMethodOverride]
        super().keyReleaseEvent(e)
        if e is None:
            return
        key = e.key()
        self._keys_down.discard(key)
        if self.underMouse():
            self.sig_key_released.emit(self._mouse_down, self._keys_down)

    # endregion
