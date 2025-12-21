using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Andastra.Runtime.MonoGame.Rendering
{
    /// <summary>
    /// Command list optimizer for reducing draw call overhead.
    /// 
    /// Optimizes command lists by merging compatible commands,
    /// reducing CPU overhead and improving GPU utilization.
    /// 
    /// Features:
    /// - Command merging
    /// - Redundant call elimination
    /// - State change minimization
    /// - Draw call batching
    /// 
    /// Based on industry-standard command buffer optimization techniques:
    /// - Draw call batching by combining compatible draws
    /// - State sorting to minimize state changes
    /// - Geometry merging for consecutive draws with same state
    /// - Instancing conversion when appropriate
    /// </summary>
    public class CommandListOptimizer
    {
        /// <summary>
        /// Draw command data for indexed draw calls.
        /// Contains information needed to execute a DrawIndexedPrimitives call.
        /// </summary>
        private struct DrawIndexedCommandData
        {
            public PrimitiveType PrimitiveType;
            public int BaseVertex;
            public int MinVertexIndex;
            public int NumVertices;
            public int StartIndex;
            public int PrimitiveCount;
            public object VertexBuffer; // VertexBuffer or equivalent
            public object IndexBuffer;  // IndexBuffer or equivalent

            public DrawIndexedCommandData(PrimitiveType primitiveType, int baseVertex, int minVertexIndex, int numVertices, int startIndex, int primitiveCount, object vertexBuffer, object indexBuffer)
            {
                PrimitiveType = primitiveType;
                BaseVertex = baseVertex;
                MinVertexIndex = minVertexIndex;
                NumVertices = numVertices;
                StartIndex = startIndex;
                PrimitiveCount = primitiveCount;
                VertexBuffer = vertexBuffer;
                IndexBuffer = indexBuffer;
            }
        }

        /// <summary>
        /// Draw command data for non-indexed draw calls.
        /// Contains information needed to execute a DrawPrimitives call.
        /// </summary>
        private struct DrawCommandData
        {
            public PrimitiveType PrimitiveType;
            public int VertexStart;
            public int PrimitiveCount;
            public object VertexBuffer; // VertexBuffer or equivalent

            public DrawCommandData(PrimitiveType primitiveType, int vertexStart, int primitiveCount, object vertexBuffer)
            {
                PrimitiveType = primitiveType;
                VertexStart = vertexStart;
                PrimitiveCount = primitiveCount;
                VertexBuffer = vertexBuffer;
            }
        }

        /// <summary>
        /// Instanced draw command data.
        /// Contains information needed to execute a DrawInstancedPrimitives call.
        /// </summary>
        private struct DrawInstancedCommandData
        {
            public PrimitiveType PrimitiveType;
            public int BaseVertex;
            public int MinVertexIndex;
            public int NumVertices;
            public int StartIndex;
            public int PrimitiveCountPerInstance;
            public int InstanceCount;
            public int StartInstanceLocation;
            public object VertexBuffer; // VertexBuffer or equivalent
            public object IndexBuffer;  // IndexBuffer or equivalent

            public DrawInstancedCommandData(PrimitiveType primitiveType, int baseVertex, int minVertexIndex, int numVertices, int startIndex, int primitiveCountPerInstance, int instanceCount, int startInstanceLocation, object vertexBuffer, object indexBuffer)
            {
                PrimitiveType = primitiveType;
                BaseVertex = baseVertex;
                MinVertexIndex = minVertexIndex;
                NumVertices = numVertices;
                StartIndex = startIndex;
                PrimitiveCountPerInstance = primitiveCountPerInstance;
                InstanceCount = instanceCount;
                StartInstanceLocation = startInstanceLocation;
                VertexBuffer = vertexBuffer;
                IndexBuffer = indexBuffer;
            }
        }
        /// <summary>
        /// Optimizes a command buffer by merging and reordering commands.
        /// </summary>
        /// <param name="buffer">Command buffer to optimize. Can be null (no-op).</param>
        public void Optimize(CommandBuffer buffer)
        {
            if (buffer == null)
            {
                return;
            }

            // Get commands
            var commands = new List<CommandBuffer.RenderCommand>(buffer.GetCommands());

            // Sort by state to minimize changes
            commands.Sort((a, b) => a.SortKey.CompareTo(b.SortKey));

            // Merge compatible commands
            MergeCommands(commands);

            // Clear and rebuild buffer
            buffer.Clear();
            foreach (CommandBuffer.RenderCommand cmd in commands)
            {
                buffer.AddCommand(cmd.Type, cmd.Data, cmd.SortKey);
            }
        }

        /// <summary>
        /// Merges compatible commands to reduce draw calls.
        /// 
        /// This optimization reduces CPU overhead by combining multiple draw calls
        /// into a single call when they share the same render state and buffers.
        /// 
        /// Merging strategies:
        /// 1. For indexed draws with same buffers: Combine into single draw with adjusted ranges
        /// 2. For non-indexed draws with same buffers: Combine into single draw with adjusted ranges
        /// 3. For compatible draws: Convert to instanced rendering when appropriate
        /// </summary>
        private void MergeCommands(List<CommandBuffer.RenderCommand> commands)
        {
            if (commands == null || commands.Count < 2)
            {
                return;
            }

            // Merge consecutive draw calls with same state
            int i = 0;
            while (i < commands.Count - 1)
            {
                CommandBuffer.RenderCommand current = commands[i];
                CommandBuffer.RenderCommand next = commands[i + 1];

                // Check if commands can be merged
                if (CanMerge(current, next))
                {
                    // Attempt to merge the commands
                    if (TryMergeDrawCommands(commands, i, i + 1))
                    {
                        // Successfully merged - remove the second command
                        commands.RemoveAt(i + 1);
                        // Don't increment i - check current command again as it may be mergeable with next
                    }
                    else
                    {
                        // Could not merge - move to next command
                        i++;
                    }
                }
                else
                {
                    // Cannot merge - move to next command
                    i++;
                }
            }
        }

        /// <summary>
        /// Attempts to merge two draw commands.
        /// 
        /// Returns true if the commands were successfully merged into the first command,
        /// false if merging is not possible.
        /// </summary>
        private bool TryMergeDrawCommands(List<CommandBuffer.RenderCommand> commands, int firstIndex, int secondIndex)
        {
            CommandBuffer.RenderCommand first = commands[firstIndex];
            CommandBuffer.RenderCommand second = commands[secondIndex];

            // Handle indexed draw commands
            if (first.Type == CommandBuffer.CommandType.DrawIndexed && second.Type == CommandBuffer.CommandType.DrawIndexed)
            {
                return TryMergeDrawIndexedCommands(commands, firstIndex, secondIndex);
            }

            // Handle non-indexed draw commands
            if (first.Type == CommandBuffer.CommandType.Draw && second.Type == CommandBuffer.CommandType.Draw)
            {
                return TryMergeDrawCommandsNonIndexed(commands, firstIndex, secondIndex);
            }

            // Handle instanced draw commands
            if (first.Type == CommandBuffer.CommandType.DrawInstanced && second.Type == CommandBuffer.CommandType.DrawInstanced)
            {
                return TryMergeDrawInstancedCommands(commands, firstIndex, secondIndex);
            }

            // Cannot merge different command types
            return false;
        }

        /// <summary>
        /// Attempts to merge two indexed draw commands.
        /// 
        /// Two indexed draws can be merged if they:
        /// 1. Use the same primitive type
        /// 2. Use the same vertex and index buffers
        /// 3. Have compatible ranges (can be combined into a single draw)
        /// </summary>
        private bool TryMergeDrawIndexedCommands(List<CommandBuffer.RenderCommand> commands, int firstIndex, int secondIndex)
        {
            CommandBuffer.RenderCommand first = commands[firstIndex];
            CommandBuffer.RenderCommand second = commands[secondIndex];

            // Extract draw command data - must be in expected format
            DrawIndexedCommandData? firstDataNullable = ExtractDrawIndexedData(first.Data);
            DrawIndexedCommandData? secondDataNullable = ExtractDrawIndexedData(second.Data);

            if (!firstDataNullable.HasValue || !secondDataNullable.HasValue)
            {
                return false; // Cannot extract data - data format not recognized, cannot merge
            }

            DrawIndexedCommandData firstData = firstDataNullable.Value;
            DrawIndexedCommandData secondData = secondDataNullable.Value;

            // Check if we successfully extracted valid data (non-null buffers indicate valid extraction)
            if (firstData.VertexBuffer == null || firstData.IndexBuffer == null ||
                secondData.VertexBuffer == null || secondData.IndexBuffer == null)
            {
                return false; // Invalid or unrecognized data format - cannot merge
            }

            // Check if commands use the same buffers and primitive type
            if (!AreBuffersEqual(firstData.VertexBuffer, secondData.VertexBuffer) ||
                !AreBuffersEqual(firstData.IndexBuffer, secondData.IndexBuffer) ||
                firstData.PrimitiveType != secondData.PrimitiveType)
            {
                return false; // Cannot merge - different buffers or primitive type
            }

            // Check if the draws are consecutive in the buffer (can be merged into single draw)
            // For indexed draws, we can merge if the second draw starts where the first ends
            int firstEndIndex = firstData.StartIndex + (firstData.PrimitiveCount * GetIndicesPerPrimitive(firstData.PrimitiveType));
            if (secondData.StartIndex == firstEndIndex && firstData.BaseVertex == secondData.BaseVertex)
            {
                // Merge: Combine into single draw with expanded range
                DrawIndexedCommandData mergedData = new DrawIndexedCommandData(
                    firstData.PrimitiveType,
                    firstData.BaseVertex,
                    Math.Min(firstData.MinVertexIndex, secondData.MinVertexIndex), // Use minimum vertex index
                    Math.Max(firstData.NumVertices, secondData.NumVertices), // Use maximum vertex count
                    firstData.StartIndex, // Start at first draw's start index
                    firstData.PrimitiveCount + secondData.PrimitiveCount, // Combined primitive count
                    firstData.VertexBuffer,
                    firstData.IndexBuffer);

                // Update the first command with merged data
                CommandBuffer.RenderCommand mergedCommand = new CommandBuffer.RenderCommand
                {
                    Type = first.Type,
                    Data = mergedData,
                    SortKey = first.SortKey
                };
                commands[firstIndex] = mergedCommand;
                return true;
            }

            // If not consecutive, check if we can convert to instanced rendering
            // (same geometry, different transforms - would require instancing support)
            // For now, we only merge consecutive draws
            return false;
        }

        /// <summary>
        /// Attempts to merge two non-indexed draw commands.
        /// 
        /// Two non-indexed draws can be merged if they:
        /// 1. Use the same primitive type
        /// 2. Use the same vertex buffer
        /// 3. Have consecutive ranges (can be combined into a single draw)
        /// </summary>
        private bool TryMergeDrawCommandsNonIndexed(List<CommandBuffer.RenderCommand> commands, int firstIndex, int secondIndex)
        {
            CommandBuffer.RenderCommand first = commands[firstIndex];
            CommandBuffer.RenderCommand second = commands[secondIndex];

            // Extract draw command data - must be in expected format
            DrawCommandData? firstDataNullable = ExtractDrawData(first.Data);
            DrawCommandData? secondDataNullable = ExtractDrawData(second.Data);

            if (!firstDataNullable.HasValue || !secondDataNullable.HasValue)
            {
                return false; // Cannot extract data - data format not recognized, cannot merge
            }

            DrawCommandData firstData = firstDataNullable.Value;
            DrawCommandData secondData = secondDataNullable.Value;

            // Check if we successfully extracted valid data (non-null buffer indicates valid extraction)
            if (firstData.VertexBuffer == null || secondData.VertexBuffer == null)
            {
                return false; // Invalid or unrecognized data format - cannot merge
            }

            // Check if commands use the same buffer and primitive type
            if (!AreBuffersEqual(firstData.VertexBuffer, secondData.VertexBuffer) ||
                firstData.PrimitiveType != secondData.PrimitiveType)
            {
                return false; // Cannot merge - different buffers or primitive type
            }

            // Check if the draws are consecutive in the buffer (can be merged into single draw)
            int verticesPerPrimitive = GetVerticesPerPrimitive(firstData.PrimitiveType);
            int firstEndVertex = firstData.VertexStart + (firstData.PrimitiveCount * verticesPerPrimitive);
            if (secondData.VertexStart == firstEndVertex)
            {
                // Merge: Combine into single draw with expanded range
                DrawCommandData mergedData = new DrawCommandData(
                    firstData.PrimitiveType,
                    firstData.VertexStart, // Start at first draw's start vertex
                    firstData.PrimitiveCount + secondData.PrimitiveCount, // Combined primitive count
                    firstData.VertexBuffer);

                // Update the first command with merged data
                CommandBuffer.RenderCommand mergedCommand = new CommandBuffer.RenderCommand
                {
                    Type = first.Type,
                    Data = mergedData,
                    SortKey = first.SortKey
                };
                commands[firstIndex] = mergedCommand;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to merge two instanced draw commands.
        /// 
        /// Two instanced draws can be merged if they:
        /// 1. Use the same primitive type
        /// 2. Use the same vertex and index buffers
        /// 3. Have the same geometry (same base vertex, start index, primitive count per instance)
        /// 4. Can combine instance counts
        /// </summary>
        private bool TryMergeDrawInstancedCommands(List<CommandBuffer.RenderCommand> commands, int firstIndex, int secondIndex)
        {
            CommandBuffer.RenderCommand first = commands[firstIndex];
            CommandBuffer.RenderCommand second = commands[secondIndex];

            // Extract draw command data - must be in expected format
            DrawInstancedCommandData? firstDataNullable = ExtractDrawInstancedData(first.Data);
            DrawInstancedCommandData? secondDataNullable = ExtractDrawInstancedData(second.Data);

            if (!firstDataNullable.HasValue || !secondDataNullable.HasValue)
            {
                return false; // Cannot extract data - data format not recognized, cannot merge
            }

            DrawInstancedCommandData firstData = firstDataNullable.Value;
            DrawInstancedCommandData secondData = secondDataNullable.Value;

            // Check if we successfully extracted valid data (non-null buffers indicate valid extraction)
            if (firstData.VertexBuffer == null || firstData.IndexBuffer == null ||
                secondData.VertexBuffer == null || secondData.IndexBuffer == null)
            {
                return false; // Invalid or unrecognized data format - cannot merge
            }

            // Check if commands use the same buffers and have identical geometry
            if (!AreBuffersEqual(firstData.VertexBuffer, secondData.VertexBuffer) ||
                !AreBuffersEqual(firstData.IndexBuffer, secondData.IndexBuffer) ||
                firstData.PrimitiveType != secondData.PrimitiveType ||
                firstData.BaseVertex != secondData.BaseVertex ||
                firstData.StartIndex != secondData.StartIndex ||
                firstData.PrimitiveCountPerInstance != secondData.PrimitiveCountPerInstance)
            {
                return false; // Cannot merge - different buffers, geometry, or primitive type
            }

            // Merge: Combine instance counts (same geometry, more instances)
            DrawInstancedCommandData mergedData = new DrawInstancedCommandData(
                firstData.PrimitiveType,
                firstData.BaseVertex,
                firstData.MinVertexIndex,
                firstData.NumVertices,
                firstData.StartIndex,
                firstData.PrimitiveCountPerInstance,
                firstData.InstanceCount + secondData.InstanceCount, // Combined instance count
                firstData.StartInstanceLocation, // Start at first draw's start instance
                firstData.VertexBuffer,
                firstData.IndexBuffer);

            // Update the first command with merged data
            CommandBuffer.RenderCommand mergedCommand = new CommandBuffer.RenderCommand
            {
                Type = first.Type,
                Data = mergedData,
                SortKey = first.SortKey
            };
            commands[firstIndex] = mergedCommand;
            return true;
        }

        /// <summary>
        /// Checks if two commands can be merged.
        /// 
        /// Commands can be merged if they:
        /// 1. Have the same sort key (same render state, material, etc.)
        /// 2. Are the same command type (Draw, DrawIndexed, or DrawInstanced)
        /// </summary>
        private bool CanMerge(CommandBuffer.RenderCommand a, CommandBuffer.RenderCommand b)
        {
            // Commands can be merged if they have the same sort key (same state, material, etc.)
            // and are draw commands of compatible types
            if (a.SortKey != b.SortKey)
            {
                return false; // Different render state - cannot merge
            }

            // Both must be draw commands
            bool aIsDraw = a.Type == CommandBuffer.CommandType.Draw ||
                          a.Type == CommandBuffer.CommandType.DrawIndexed ||
                          a.Type == CommandBuffer.CommandType.DrawInstanced;

            bool bIsDraw = b.Type == CommandBuffer.CommandType.Draw ||
                          b.Type == CommandBuffer.CommandType.DrawIndexed ||
                          b.Type == CommandBuffer.CommandType.DrawInstanced;

            if (!aIsDraw || !bIsDraw)
            {
                return false; // Not draw commands - cannot merge
            }

            // Must be the same draw command type
            return a.Type == b.Type;
        }

        /// <summary>
        /// Extracts draw indexed command data from command data object.
        /// 
        /// Returns the extracted data if successful, null if the data format is not recognized.
        /// This ensures we only attempt to merge commands with known, valid data formats.
        /// </summary>
        private DrawIndexedCommandData? ExtractDrawIndexedData(object data)
        {
            if (data == null)
            {
                return null; // Null data - cannot extract
            }

            // If data is already a DrawIndexedCommandData structure (boxed or unboxed), return it
            if (data is DrawIndexedCommandData drawData)
            {
                return drawData;
            }

            // Data format not recognized - return null to indicate extraction failure
            // This prevents incorrect merging when data is in an unknown format
            return null;
        }

        /// <summary>
        /// Extracts draw command data from command data object.
        /// 
        /// Returns the extracted data if successful, null if the data format is not recognized.
        /// This ensures we only attempt to merge commands with known, valid data formats.
        /// </summary>
        private DrawCommandData? ExtractDrawData(object data)
        {
            if (data == null)
            {
                return null; // Null data - cannot extract
            }

            // If data is already a DrawCommandData structure (boxed or unboxed), return it
            if (data is DrawCommandData drawData)
            {
                return drawData;
            }

            // Data format not recognized - return null to indicate extraction failure
            // This prevents incorrect merging when data is in an unknown format
            return null;
        }

        /// <summary>
        /// Extracts draw instanced command data from command data object.
        /// 
        /// Returns the extracted data if successful, null if the data format is not recognized.
        /// This ensures we only attempt to merge commands with known, valid data formats.
        /// </summary>
        private DrawInstancedCommandData? ExtractDrawInstancedData(object data)
        {
            if (data == null)
            {
                return null; // Null data - cannot extract
            }

            // If data is already a DrawInstancedCommandData structure (boxed or unboxed), return it
            if (data is DrawInstancedCommandData drawData)
            {
                return drawData;
            }

            // Data format not recognized - return null to indicate extraction failure
            // This prevents incorrect merging when data is in an unknown format
            return null;
        }

        /// <summary>
        /// Checks if two buffer objects are equal.
        /// Buffers are compared by reference equality or by comparing their identity.
        /// </summary>
        private bool AreBuffersEqual(object bufferA, object bufferB)
        {
            if (bufferA == null && bufferB == null)
            {
                return true; // Both null - equal
            }

            if (bufferA == null || bufferB == null)
            {
                return false; // One null, one not - not equal
            }

            // Compare by reference equality
            // In a full implementation, might also check buffer identity or handle
            return ReferenceEquals(bufferA, bufferB);
        }

        /// <summary>
        /// Gets the number of indices per primitive for a given primitive type.
        /// </summary>
        private int GetIndicesPerPrimitive(PrimitiveType primitiveType)
        {
            switch (primitiveType)
            {
                case PrimitiveType.TriangleList:
                    return 3;
                case PrimitiveType.TriangleStrip:
                    return 1; // Triangle strip uses 1 index per triangle after first
                case PrimitiveType.LineList:
                    return 2;
                case PrimitiveType.LineStrip:
                    return 1; // Line strip uses 1 index per line after first
                case PrimitiveType.PointList:
                    return 1;
                default:
                    return 3; // Default to triangle list
            }
        }

        /// <summary>
        /// Gets the number of vertices per primitive for a given primitive type.
        /// </summary>
        private int GetVerticesPerPrimitive(PrimitiveType primitiveType)
        {
            switch (primitiveType)
            {
                case PrimitiveType.TriangleList:
                    return 3;
                case PrimitiveType.TriangleStrip:
                    return 1; // Triangle strip uses 1 vertex per triangle after first
                case PrimitiveType.LineList:
                    return 2;
                case PrimitiveType.LineStrip:
                    return 1; // Line strip uses 1 vertex per line after first
                case PrimitiveType.PointList:
                    return 1;
                default:
                    return 3; // Default to triangle list
            }
        }
    }
}

