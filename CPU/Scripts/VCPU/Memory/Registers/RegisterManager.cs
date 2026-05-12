using System;

namespace VirtualCPU
{
    public enum Register : byte
    {
        R0 = 0,
        R1 = 1,
        R2 = 2,
        R3 = 3,
        R4 = 4,
        R5 = 5,
        R6 = 6,
        R7 = 7,
        R8 = 8,
        R9 = 9,
        R10 = 10,
        R11 = 11,
        R12 = 12,
    }

    /// <summary>
    /// Class that represents the registers of the virtual CPU, it provides methods to get and set the values of the registers,
    /// </summary>
    /// <remarks>There currently are 12 general-purpose registers.</remarks>
    public class RegisterManager
    {
        private Dictionary<Register, byte> _registerValues;
        private Flags _flagsRegister; 
        private Action<string> _crashHandle;

        public Flags FlagsRegister { get => _flagsRegister; }

        #region Methods

        #region Public Methods  
        public RegisterManager(Action<string> crashHandle = null)
        {
            _crashHandle = crashHandle;
            InitializeRegisters();
        }

        /// <summary>
        /// Gets the value of a register, and outputs it in the value parameter, 
        /// if the register does not exist it will crash the program
        /// </summary>
        /// <param name="register">The register number to get the value from</param>
        public byte GetRegisterValue(byte register)
        {
            byte value = 0;

            if (register >= Enum.GetValues(typeof(Register)).Length)
            {
                _crashHandle?.Invoke("Tried to access a non-existent register");
                return value;
            }

            Register register1 = (Register)register;
            value = GetRegisterValue(register1);

            return value;
        }

        /// <summary>
        /// Sets the specified register to the given value. 
        /// </summary>
        /// <param name="register">The index of the register to set.</param>
        /// <param name="value">The value to assign to the register.</param>
        public void SetRegisterValue(byte register, byte value)
        {
            if (register >= Enum.GetValues(typeof(Register)).Length)
            {
                _crashHandle?.Invoke("Tried to access a non-existent register");
                return;
            }

            Register register1 = (Register)register;
            SetRegisterValue(register1, value);
        }
        #endregion

        #region Private Methods
        private void InitializeRegisters()
        {
            _registerValues = new Dictionary<Register, byte>();
            foreach (Register reg in Enum.GetValues(typeof(Register)))
            {
                _registerValues[reg] = 0; // Initialize all registers to 0
            }
        }


        /// <summary>
        /// Sets the value of the specified register.
        /// </summary>
        /// <param name="register">The register to set the value of.</param>
        /// <param name="value">The value to set the register to.</param>
        private void SetRegisterValue(Register register, byte value)
        {
            _registerValues[register] = value;
        }

        /// <summary>
        /// Gets the value of the specified register.
        /// </summary>
        /// <param name="register">The register to get the value of.</param>
        /// <returns>The value of the specified register.</returns>
        private byte GetRegisterValue(Register register)
        {
            return _registerValues[register];
        }
        #endregion

        #endregion
    }
}
