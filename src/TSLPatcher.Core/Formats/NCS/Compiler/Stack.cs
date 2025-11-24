using System;
using System.Collections.Generic;
using System.Linq;
using TSLPatcher.Core.Common;
using TSLPatcher.Core.Common.Script;

namespace TSLPatcher.Core.Formats.NCS.Compiler;

/// <summary>
/// Stack implementation for NCS bytecode execution.
/// 1:1 port from Python Stack class in pykotor/resource/formats/ncs/compiler/interpreter.py
/// </summary>
public class Stack
{
    private readonly List<StackObject> _stack = new();
#pragma warning disable IDE0044 // Add readonly modifier - these fields are intentionally mutable
    private int _bp; // Mutable - modified by save_bp/restore_bp
    private int _globalBp; // Mutable - set in save_bp when entering main
#pragma warning restore IDE0044
    private readonly List<int> _bpBuffer = new();

    /// <summary>
    /// Get a copy of the current stack state.
    /// </summary>
    public List<StackObject> State()
    {
        return _stack.ToList();
    }

    /// <summary>
    /// Add a value to the stack.
    /// </summary>
    public void Add(DataType dataType, object? value)
    {
        _stack.Add(new StackObject(dataType, value));
    }

    /// <summary>
    /// Get the stack pointer (size in bytes, assuming 4 bytes per element).
    /// </summary>
    public int StackPointer()
    {
        return _stack.Count * 4;
    }

    /// <summary>
    /// Get the base pointer.
    /// </summary>
    public int BasePointer()
    {
        return _bp;
    }

    /// <summary>
    /// Peek at a value at the specified offset from the top of the stack.
    /// Returns the StackObject at that position.
    /// </summary>
    public StackObject Peek(int offset)
    {
        int realIndex = StackIndex(offset);
        return _stack[realIndex];
    }

    /// <summary>
    /// Move the stack pointer by offset (shrink or grow stack).
    /// </summary>
    public void Move(int offset)
    {
        if (offset == 0)
        {
            return;
        }

        if (offset > 0)
        {
            if (offset % 4 != 0)
            {
                throw new ArgumentException($"Stack growth offset must be multiple of 4, got {offset}");
            }
            int words = offset / 4;
            for (int i = 0; i < words; i++)
            {
                _stack.Add(new StackObject(DataType.Int, 0));
            }
            return;
        }

        int removeTo = StackIndex(offset);
        _stack.RemoveRange(removeTo, _stack.Count - removeTo);
    }

    /// <summary>
    /// Copy values from the top of the stack down to the specified offset.
    /// </summary>
    public void CopyDown(int offset, int size)
    {
        if (size % 4 != 0)
        {
            throw new ArgumentException("Size must be divisible by 4");
        }

        int numElements = size / 4;

        if (numElements > _stack.Count)
        {
            throw new IndexOutOfRangeException("Size exceeds the current stack size");
        }

        // Find the target indices first
        var targetIndices = new List<int>();
        int tempOffset = offset;

        for (int i = 0; i < numElements; i++)
        {
            int targetIndex = StackIndex(tempOffset);
            targetIndices.Add(targetIndex);
            tempOffset += 4; // Move to the next position
        }

        // Now copy the elements down the stack
        for (int i = 0; i < numElements; i++)
        {
            int sourceIndex = _stack.Count - 1 - i; // Counting from the end of the list
            int targetIndex = targetIndices[numElements - 1 - i]; // The last target index corresponds to the first source index
            _stack[targetIndex] = new StackObject(_stack[sourceIndex].DataType, _stack[sourceIndex].Value);
        }
    }

    /// <summary>
    /// Convert a stack offset to an actual list index.
    /// </summary>
    private int StackIndex(int offset)
    {
        if (offset == 0)
        {
            throw new ArgumentException("Stack offset of zero is not valid");
        }
        if (offset % 4 != 0)
        {
            throw new ArgumentException($"Stack offset must be a multiple of 4 bytes, got {offset}");
        }
        if (_stack.Count == 0)
        {
            throw new ArgumentException("Cannot resolve stack offset on an empty stack");
        }

        int remaining = Math.Abs(offset);
        int listIndex = _stack.Count - 1; // Start from the top of the stack (last element)

        while (true)
        {
            if (listIndex < 0)
            {
                throw new ArgumentException($"Stack offset {offset} is out of range");
            }

            StackObject element = _stack[listIndex];
            int elementSize = element.DataType.Size();
            if (elementSize <= 0)
            {
                throw new ArgumentException($"Unsupported element size {elementSize} for {element.DataType}");
            }

            if (remaining <= elementSize)
            {
                return listIndex;
            }

            remaining -= elementSize;
            listIndex -= 1;
        }
    }

}

