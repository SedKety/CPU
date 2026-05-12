
namespace VirtualCPU.ExtensionMethods
{
    public static class BitExtensions
    {
        #region Bit operations
        /// <summary>
        /// Retrieves the value of the bit at the specified index in the byte.
        /// </summary>
        /// <param name="b">The byte from which to extract the bit.</param>
        /// <param name="index">The zero-based position of the bit to retrieve.</param>
        /// <returns>The value of the bit at the specified index, either 0 or 1.</returns>
        public static byte GetBit(this byte b, int index)
        {
            return (byte)((b >> index) & 0x01);
        }

        /// <summary>
        /// Sets the bit at the specified index in the byte to 1.
        /// </summary>
        /// <param name="b">The byte in which to set the bit.</param>
        /// <param name="index">The zero-based position of the bit to set.</param>
        /// <returns>The byte with the bit at the specified index set to 1.</returns>
        public static byte SetBit(this byte b, int index)
        {
            return (byte)(b | (0x01 << index));
        }

        /// <summary>
        /// Clears the bit at the specified index in the byte, setting it to 0.
        /// </summary>
        /// <param name="b">The byte in which to clear the bit.</param>
        /// <param name="index">The zero-based position of the bit to clear.</param>
        /// <returns>The byte with the bit at the specified index cleared to 0.</returns>
        public static byte ClearBit(this byte b, int index)
        {
            return (byte)(b & ~(0x01 << index));
        }
        #endregion

        #region Nibble (4 bits) operations

        /// <summary>
        /// Gets the value of the nibble (4 bits) at the specified index in the byte.
        /// </summary>
        /// <param name="b">The byte from which to extract the nibble.</param>
        /// <param name="index">The zero-based position of the nibble to retrieve. False for lower nibble, True for upper nibble.</param>
        /// <returns>The value of the nibble at the specified index.</returns>
        public static byte GetNibble(this byte b, bool index)
        {
            return (byte)((b >> (index ? 4 : 0)) & 0x0F);
        }

        /// <summary>
        /// Sets the value of the nibble (4 bits) at the specified index in the byte to the provided value.
        /// </summary>
        /// <param name="b">The byte in which to set the nibble.</param>
        /// <param name="index">The zero-based position of the nibble to set. False for lower nibble, True for upper nibble.</param>
        /// <param name="value">The value to set the nibble to.</param>
        /// <returns>The byte with the nibble at the specified index set to the provided value.</returns>
        public static byte SetNibble(this byte b, int index, byte value)
        {
            return (byte)((b & ~(0x0F << (index * 4))) | ((value & 0x0F) << (index * 4)));
        }

        /// <summary>
        /// Clears the nibble (4 bits) at the specified index in the byte, setting it to 0.
        /// </summary>
        /// <param name="b">The byte in which to clear the nibble.</param>
        /// <param name="index">The zero-based position of the nibble to clear. False for lower nibble, True for upper nibble.</param>
        /// <returns>The byte with the nibble at the specified index cleared to 0.</returns>
        public static byte ClearNibble(this byte b, int index)
        {
            return (byte)(b & ~(0x0F << (index * 4)));
        }
        #endregion
    }
}