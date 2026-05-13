

namespace VirtualCPU
{
    /// <summary>
    /// This enum provides a non-magic way to refer to the opcodes.
    /// Each opcode is represented by a unique byte value, which the VirtualCPU will interpret during execution.
    /// </summary>
    public enum OpCodes : byte
    {
        //<-----------Program opcodes------------>
        /// <summary>
        /// Signifies the end of the program. When the VirtualCPU encounters this opcode, it will stop executing further instructions.
        /// </summary>
        END = 0x00,

        /// <summary>
        /// No operation, does nothing and moves to the next instruction. This can be used for padding or to create intentional delays in the execution flow.
        /// </summary>
        NOP = 0x01, // No operation, does nothing and moves to the next instruction

        /// <summary>
        /// Prints the value of a register or a block of memory to the console.
        /// The instruction format is as follows:
        /// PRT SourceType(Register = 0, Memory = 1, Immediate value = 2) Source
        /// </summary>
        PRT = 0x02, // Print the value of a register / a block of memory to the console

        //<-----------Locational opcodes------------>

        /// <summary>
        /// Loads a value into a specified register.
        /// The instruction format is as follows:
        /// LOAD Value Register
        /// </summary>
        LOAD = 0x05,

        /// <summary>
        /// Jumps to a specified address in the program.
        /// The instruction format is as follows:
        /// JMP Address
        /// </summary>
        JMP = 0x06,

        /// <summary>
        /// Moves a value from Memory to Register or Register to Memory
        /// The instruction format is as follows:
        /// MOV Source Destination
        /// </summary>
        MOV = 0x07,

        JNE = 0x08, // Jump if not equal
        JE = 0x09,  // Jump if equal
        JL = 0x0A,  // Jump if less
        JG = 0x0B,  // Jump if greater

        //<-----------Arithmetic opcodes------------>

        /// <summary>
        /// Adds the values of two registers and stores the result in the first register.
        /// The instruction format is as follows:
        /// ADD Register1 Register2
        /// </summary>
        ADD = 0x14,

        /// <summary>
        /// Compares the values of two registers and sets the appropriate flags based on the result 
        /// (e.g., zero flag, signed flag, overflow flag).
        /// The instruction format is as follows:
        /// CMP Register1 Register2
        /// </summary>
        CMP = 0x15,

        /// <summary>
        /// Subtracts the value of the second register from the first register and stores the result in the first register.
        /// The instruction format is as follows:
        /// SUB Register1 Register2         
        /// </summary>
        SUB = 0x16,

        /// <summary>
        /// Increments the value of a specified register by 1.
        /// The instruction format is as follows:
        /// INC Register
        /// </summary>
        INC = 0x17,

        /// <summary>
        /// Decrements the value of a specified register by 1.
        /// The instruction format is as follows:
        /// DEC Register
        /// </summary>
        DEC = 0x18



    }
}