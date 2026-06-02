using System;

namespace VirtualCPU.Opcodes
{
    /// <summary>
    /// Represents an instruction that generates a random value using the system time
    /// and stores it in a specified register in the virtual CPU.
    /// The instruction format is as follows:
    /// RND Register Min Max
    /// </summary>
    public class RandomInstruction : OpcodeInstruction
    {
        public string Name => "RND";
        public bool Accept(byte opcode) => opcode == (byte)OpCodes.RND;

        private Random random;
        public void Act(VCPU vCpu, byte opcode, Action<string> crashHandle)
        {
            random = random == null ? new Random() : random;

            var pc = vCpu.ProgramCounter;
            var registers = vCpu.Registers;
            var destinationRegister = vCpu.Program[pc + 1];
            var min = vCpu.Program[pc + 2];
            var max = vCpu.Program[pc + 3];

            // Uses the system time to emulate how you'd do it on the original masm64 setup.
            byte randomValue = (byte)random.Next(min, max + 1);

            vCpu.Log($"Generating random value between {min} and {max} for R{destinationRegister} Value = {randomValue}");

            registers.SetRegisterValue(destinationRegister, randomValue); 

            vCpu.SetProgramCounter((byte)(pc + 4));
        }
    }
}
