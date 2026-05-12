using System.Reflection;

namespace VirtualCPU
{
    public class Program
    {
        private static byte[] s_program = new byte[0];

        //Entry point
        static void Main(string[] args)
        {
            s_program = new byte[32];

            s_program[0] = (byte)OpCode.INC;
            s_program[1] = (byte)Register.R0;

            s_program[2] = (byte)OpCode.INC;
            s_program[3] = (byte)Register.R0;

            s_program[4] = (byte)OpCode.INC;
            s_program[5] = (byte)Register.R0;

            s_program[6] = (byte)OpCode.JMP;
            s_program[7] = 0x09;              //Location to jump to

            s_program[8] = (byte)OpCode.END;  //Gets skipped due to the jump

            s_program[9] = (byte)OpCode.INC;
            s_program[10] = (byte)Register.R0;

            //Register to register MOV
            s_program[11] = (byte)OpCode.MOV;
            s_program[12] = (byte)Register.R0;
            s_program[13] = 1;                // Source is register (1)
            s_program[14] = (byte)Register.R1;
            s_program[15] = 1;                // Destination is register (1)


            //Register to memory MOV
            s_program[16] = (byte)OpCode.MOV;
            s_program[17] = (byte)Register.R0; // The register to move from
            s_program[18] = 1;                // Source is register (1)
            s_program[19] = 10;                // The adress to move to in memory
            s_program[20] = 0;                // Destination is in memory (0)


            //Memory to register MOV
            s_program[21] = (byte)OpCode.MOV;
            s_program[22] = 10;               // The location to move in memory
            s_program[23] = 0;                // Source is in memory (0)
            s_program[24] = (byte)Register.R2;
            s_program[25] = 1;                // Destination is register (1)

            //Memory to memory MOV (Crash)
            s_program[26] = (byte)OpCode.MOV;
            s_program[27] = 10;               // The location to move in memory 
            s_program[28] = 0;                // Source is in memory (0)
            s_program[29] = 2;                // The adress to move to in memory
            s_program[30] = 0;                // Destination is in memory (0)

            s_program[31] = (byte)OpCode.END;


            OpcodeInstruction[] instructions = Assembly.GetAssembly(typeof(Program)).GetTypes()
                .Where(t => typeof(OpcodeInstruction).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .Select(t => (OpcodeInstruction)Activator.CreateInstance(t))
                .ToArray();

            VCPU cpu = new VCPU(s_program, instructions, true);
        }
    }
}
