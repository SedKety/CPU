using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualCPU.Opcodes
{
    /// <summary>
    /// Instruction to end the program, this will stop the execution of the program and return control to the caller,
    /// </summary>
    public class EndInstruction : OpcodeInstruction
    {
        public string Name => "END";
        public bool Accept(byte opcode) => opcode == (byte)OpCodes.END;

        public void Act(VCPU vCpu, byte opcode, Action<string> crashHandle)
        {
            vCpu.EndProgram();
        }
    }
}
