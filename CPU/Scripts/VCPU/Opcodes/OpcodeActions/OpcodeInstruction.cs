using System;

namespace VirtualCPU
{
    /// <summary>
    /// Represents an instruction that can be executed by the virtual CPU.
    /// </summary>
    public interface OpcodeInstruction
    {
        /// <summary>
        /// Gets the name of the instruction.
        /// </summary>
        public string Name { get; } 
        /// <summary>
        /// Determines whether the instruction accepts the specified opcode.
        /// </summary>
        /// <param name="opcode">The opcode to check.</param>
        /// <returns>True if the instruction accepts the opcode; otherwise, false.</returns>
        public bool Accept(byte opcode);

        /// <summary>
        /// The logic that is executed within the virtual CPU when the instruction is executed.
        /// </summary>
        /// <param name="vCpu">The virtual CPU on which to execute the instruction.</param>
        /// <param name="opcode">The opcode of the instruction.</param>
        /// <param name="crashHandle">The action to handle crashes.</param>
        public void Act(VCPU vCpu, byte opcode, Action<string> crashHandle);
    }
}