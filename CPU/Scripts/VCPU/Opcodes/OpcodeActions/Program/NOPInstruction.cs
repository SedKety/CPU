using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualCPU.Opcodes
{
    /// <summary>
    /// Instruction to perform no operation, this will do nothing and move to the next instruction.
    /// </summary>
    public class NOPInstruction : OpcodeInstruction
    {
        public string Name => "NOP";
        public bool Accept(byte opcode) => opcode == (byte)OpCodes.NOP;

        // Move to the next instruction
        public void Act(VCPU vCpu, byte opcode, Action<string> crashHandle) => vCpu.SetProgramCounter((byte)(vCpu.ProgramCounter + 1)); 
    }
}
