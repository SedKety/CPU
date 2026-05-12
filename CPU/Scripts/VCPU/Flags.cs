namespace VirtualCPU
{
    /// <summary>
    /// Provides a non-magic way to refer to the flags from the flag register.
    /// </summary>
    [System.Flags]
    public enum Flags : byte
    {
        //<------------------------(Status Flags)------------------------>
        /// <summary>
        /// The zero flag is set if the result of an operation is zero. 
        /// For example, if you compare two registers and they are equal,
        /// the zero flag would be set to indicate that the result of the comparison is zero (i.e., they are equal).
        /// </summary>
        Zero = 0x01,

        /// <summary>
        /// The signed flag is set if the result of an operation is negative. (AKA: signed)
        /// </summary>
        Signed = 0x02,

        /// <summary>
        /// The overflow flag is set if an arithmetic operation results in a value that exceeds
        /// the maximum (or minimum) value that can be represented in the destination operand.
        /// </summary>
        Overflow = 0x04,

        /// <summary>
        /// The carry flag is set if an arithmetic operation generates a carry out of the most significant bit (for addition)
        /// or a borrow into the most significant bit (for subtraction).
        /// </summary>
        Carry = 0x08,

        /// <summary>
        /// The parity flag is set if the number of set bits (1s) in the result of an operation is even.
        /// </summary>
        /// <remarks>
        /// I have NO clue what this is used for, but I thought it would be fun to include it as a flag, and maybe I can find a use for it in the future.
        /// </remarks>
        Parity = 0x10,

    }
}