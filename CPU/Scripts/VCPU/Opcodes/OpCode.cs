

namespace VirtualCPU
{
    /// <summary>
    /// This enum provides a non-magic way to refer to the opcodes.
    /// Each opcode is represented by a unique byte value, which the VirtualCPU will interpret during execution.
    /// </summary>
    public enum OpCode : byte
    {
        //<-----------Program opcodes------------>
        /// <summary>
        /// Signifies the end of the program. When the VirtualCPU encounters this opcode, it will stop executing further instructions.
        /// </summary>
        END = 0x00,


        //<-----------Locational opcodes------------>

        /// <summary>
        /// Loads a value into a specified register.
        /// </summary>
        LOAD = 0x05,

        /// <summary>
        /// Jumps to a specified address in the program.
        /// </summary>
        JMP = 0x06,

        /// <summary>
        /// Moves a value from Memory to Register or Register to Memory
        /// </summary>
        MOV = 0x07,


        //<-----------Arithmetic opcodes------------>

        /// <summary>
        /// Adds the values of two registers and stores the result in the first register.
        /// </summary>
        ADD = 0x20,

        /// <summary>
        /// Compares the values of two registers and sets the appropriate flags based on the result 
        /// (e.g., zero flag, signed flag, overflow flag).
        /// </summary>
        CMP = 0x21,

        /// <summary>
        /// Subtracts the value of the second register from the first register and stores the result in the first register.
        /// </summary>
        SUB = 0x22,

        /// <summary>
        /// Increments the value of a specified register by 1.
        /// </summary>
        INC = 0x23,

        /// <summary>
        /// Decrements the value of a specified register by 1.
        /// </summary>
        DEC = 0x24
    }
}