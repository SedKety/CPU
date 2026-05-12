using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualCPU.Opcodes
{
    /// <summary>
    /// Instruction to add the values of two registers and store the result in a destination register.
    /// </summary>
    public class AddInstruction : OpcodeInstruction
    {
        public string Name => "ADD";

        public bool Accept(byte opcode)
        {
            return opcode == (byte)OpCode.ADD;
        }

        public void Act(VCPU vCpu, byte opcode, Action<string> crashHandle)
        {
            
        }
    }
}
