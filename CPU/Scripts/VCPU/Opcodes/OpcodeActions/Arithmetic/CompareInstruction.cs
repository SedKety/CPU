using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualCPU.Opcodes
{
    /// <summary>
    /// Instruction to compare the values of two registers and update the flags register accordingly.
    /// The instruction format is as follows:
    /// CMP Register1 Register2
    /// </summary>
    public class CompareInstruction : OpcodeInstruction
    {
        public string Name => "CMP";

        public bool Accept(byte opcode)
        {
            return opcode == (byte)OpCodes.CMP;
        }

        public void Act(VCPU vCpu, byte opcode, Action<string> crashHandle)
        {
            var lhs = vCpu.Registers.GetRegisterValue(vCpu.Program[vCpu.ProgramCounter + 1]);
            var rhs = vCpu.Registers.GetRegisterValue(vCpu.Program[vCpu.ProgramCounter + 2]);

            vCpu.Log($"Comparing {lhs} and {rhs} from registers {vCpu.Program[vCpu.ProgramCounter + 1]} and {vCpu.Program[vCpu.ProgramCounter + 2]}");

            vCpu.Registers.UpdateFlags(lhs, rhs, isSubtraction: true);

            for (int i = 0; i < Enum.GetValues(typeof(Flags)).Length; i++)
            {
                var flag = (Flags)(1 << i);
                var hasFlag = vCpu.Registers.FlagsRegister.HasFlag(flag);
                vCpu.Log($"Flag {flag} is {(hasFlag ? "set" : "not set")}", hasFlag ? ConsoleColor.Yellow : ConsoleColor.DarkYellow);
            }

            vCpu.SetProgramCounter((byte)(vCpu.ProgramCounter + 3));
        }
    }
}
