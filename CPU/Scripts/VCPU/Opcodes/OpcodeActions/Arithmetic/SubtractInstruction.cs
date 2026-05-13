using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualCPU.Opcodes
{
    /// <summary>
    /// Instruction to subtract the values of two registers and store the result in the destination register.
    /// The instruction format is as follows:
    /// SUB DestinationRegister SourceRegister1
    /// </summary>
    public class SubtractInstruction : OpcodeInstruction
    {
        public string Name => "SUB";

        public bool Accept(byte opcode)
        {
            return opcode == (byte)OpCodes.SUB;
        }

        public void Act(VCPU vCpu, byte opcode, Action<string> crashHandle)
        {
            var lhs = vCpu.Registers.GetRegisterValue(vCpu.Program[vCpu.ProgramCounter + 1]);
            var rhs = vCpu.Registers.GetRegisterValue(vCpu.Program[vCpu.ProgramCounter + 2]);
            vCpu.Log($"Subtracting {rhs} from {lhs} from registers {vCpu.Program[vCpu.ProgramCounter + 1]} and {vCpu.Program[vCpu.ProgramCounter + 2]}");
            vCpu.Registers.UpdateFlags(lhs, rhs, isSubtraction: true);

            var result = (byte)(lhs - rhs);
            vCpu.Registers.SetRegisterValue(vCpu.Program[vCpu.ProgramCounter + 1], result);

            vCpu.Log($"Result of subtraction: {result} stored in register {vCpu.Program[vCpu.ProgramCounter + 1]}");

            vCpu.SetProgramCounter((byte)(vCpu.ProgramCounter + 3));
        }
    }
}
