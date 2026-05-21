using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualCPU
{
    /// <summary>
    /// This acts as the interface between the virtual CPU and the memory, it provides methods to read and write to memory,
    /// through the stack and heap.
    /// </summary>
    public class Memory
    {
        #region Variables
        /// <summary>
        /// The memory of the virtual CPU, this is where the program and data are stored. 
        /// The size of the memory is determined by the length of the byte array passed to the constructor.
        /// </summary>
        private byte[] _heapMemory;
        public int HeapMemorySize => _heapMemory.Length;
        /// <summary>
        /// The virtual CPU that this Memory is associated with, this is used to access the program and registers of the virtual CPU,
        /// and to handle crashes when invalid memory access occurs.
        /// </summary>
        private VCPU _vCPU;

        /// <summary>
        /// The crash handle is a delegate that is called when an invalid memory access occurs,
        /// it takes a string message as a parameter, which describes the reason for the crash.
        /// </summary>
        private Action<string> _crashHandle;

        /// <summary>
        /// The stack of the virtual CPU, this is used for function calls and local variable storage, it is implemented as a Stack of bytes.
        /// </summary>
        private Stack<byte> _stack = new Stack<byte>();
        private uint _stackSize;

        #endregion

        #region Methods

        #region Logging
        /// <summary>
        /// Generates a segmentation fault message for an invalid memory access.
        /// </summary>
        /// <param name="adress">The address that was attempted to be accessed.</param>
        /// <returns>A string message describing the reason for the crash.</returns>
        private string SegmentationFaultMessage(uint adress) => $"Segmentation fault: Attempted to access address {adress}, which is outside the bounds of the memory.";

        /// <summary>
        /// Generates a stack overflow or underflow message for an invalid stack operation.
        /// </summary>
        /// <param name="isOverFlow">Indicates whether the stack operation caused an overflow (true) or underflow (false).</param>
        /// <returns>A string message describing the reason for the stack crash.</returns>
        private string StackflowMessage(bool isOverFlow) => $"Stack {(isOverFlow ? "overflow" : "underflow")}: Attempted to {(isOverFlow ? "push onto" : "pop from")} a {(isOverFlow ? "full" : "empty")} stack.";

        #endregion

        public Memory(byte[] heapMem, uint stackSize, VCPU vCpu, Action<string> crashHandle)
        {
            _heapMemory = heapMem;
            _vCPU = vCpu;
            _crashHandle = crashHandle;
            _stack = new Stack<byte>((int)stackSize);
            _stackSize = stackSize;
        }

        #region Stack

        /// <summary>
        /// Pushes memory onto the stack, if the stack is full;
        /// </summary>
        /// <param name="value"></param>
        /// <remarks> ! Can StackOverflow ! </remarks>
        public void PushToStack(byte value)
        {
            if(_stack.Count >= _stackSize)
            {
                _crashHandle(StackflowMessage(true));
                return;
            }
            _stack.Push(value);
        }

        /// <summary>
        /// Peeks at the top value of the stack without removing it.
        /// </summary>
        /// <returns>The byte value at the top of the stack, or 0 if the stack is empty.</returns>
        public byte PeekStack()
        {
            if (_stack.Count == 0)
            {
                _crashHandle(StackflowMessage(false));
                return 0;
            }
            return _stack.Peek();
        }

        /// <summary>
        /// Pops a value from the stack
        /// </summary>
        /// <returns>The byte value popped from the stack.</returns>
        /// <remarks> ! Can StackUnderflow ! </remarks>
        public byte PopFromStack()
        {
            if (_stack.Count == 0)
            {
                _crashHandle(StackflowMessage(false));
                return 0;
            }
            return _stack.Pop();
        }
        #endregion

        #region Heap
        /// <summary>
        /// Writes a byte value to a specified address in memory.
        /// </summary>
        /// <param name="adress">The address in memory where the byte value will be written.</param>
        /// <param name="value">The byte value to write to the specified address.</param>
        /// <remarks> ! Can SegFault !</remarks>
        public void WriteToMemory(uint adress, byte value)
        {
            if (adress >= _heapMemory.Length)
            {
                _crashHandle(SegmentationFaultMessage(adress));
                return;
            }
            _heapMemory[adress] = value;
        }

        /// <summary>
        /// Reads a byte value from a specified address in memory.
        /// </summary>
        /// <param name="adress">The address in memory from which the byte value will be read.</param>
        /// <returns>The byte value read from the specified address.</returns>
        /// <remarks> ! Can SegFault !</remarks>
        public byte GetFromMemory(uint adress)
        {
            if (adress > _heapMemory.Length)
            {
                _crashHandle(SegmentationFaultMessage(adress));
                return 0;
            }
            return _heapMemory[adress];
        }

        /// <summary>
        /// Retrieves the size of the memory in bytes.
        /// </summary>
        /// <returns>The size of the memory in bytes.</returns>
        public uint GetMemorySize() => (uint)_heapMemory.Length;


        #endregion

        #endregion
    }
}
