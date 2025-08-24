namespace CPU
{
    public enum OpCodes : byte
    {
        //Program-deciding opcodes
        END = 0x00, // End of program

        //Memory opcodes
        LOAD = 0x01, // Load value into a register

        //Jump opcodes
        JMP = 0x02,

        //Arithmetic opcodes
        ADD = 0x03,
        CMP = 0x04,
        SUB = 0x05,
        INC = 0x06,
        DEC = 0x07
    }

    public class Program
    {
        static byte[] program = new byte[256]; // For now just 256 considering the program size might differ later on

        //Entry point, no shit
        static void Main(string[] args)
        {
            program[0] = (byte)OpCodes.LOAD; //Load a value
            program[1] = 0x00; //Register it loads in is R0
            program[2] = 0x0A; //Value that is tored in R0

            program[3] = (byte)OpCodes.LOAD; //Load another value
            program[4] = 0x01; //In register R1
            program[5] = 0x01; //Value that is stored in R1

            program[6] = (byte)OpCodes.SUB; // We subtract the values of the two registers 
            program[7] = 0x00; //R0, We also place the result in this
            program[8] = 0x01; //R1

            program[9] = (byte)OpCodes.INC;
            program[10] = 0x01; //Incrementing R1

            program[11] = (byte)OpCodes.DEC;
            program[12] = 0x01; //Decrementing R1

            program[13] = (byte)OpCodes.JMP;
            program[14] = 0x09;

            Cpu cpu = new Cpu();
            cpu.Run(program);
        }
    }
}
