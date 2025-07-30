using WindowsInput;
using WindowsInput.Native;

namespace SymbolPad.Services
{
    public class InputService
    {
        private readonly IInputSimulator _inputSimulator;

        public InputService()
        {
            _inputSimulator = new InputSimulator();
        }

        /// <summary>
        /// Sends a symbol string to the active window.
        /// </summary>
        /// <param name="symbolText">The text of the symbol to send.</param>
        /// <param name="isPaired">Whether the symbol is a paired symbol (e.g., brackets).</param>
        public void SendSymbol(string symbolText, bool isPaired)
        {
            if (string.IsNullOrEmpty(symbolText))
            {
                return;
            }

            // Send the characters of the symbol
            _inputSimulator.Keyboard.TextEntry(symbolText);

            // If it's a paired symbol, move the cursor back to the middle
            if (isPaired && symbolText.Length >= 2)
            {
                // Calculate the midpoint. For "《》" (length 2), move left 1. For "$" (length 2), move left 1.
                int moveLeftCount = symbolText.Length / 2;
                for (int i = 0; i < moveLeftCount; i++)
                {
                    _inputSimulator.Keyboard.KeyPress(VirtualKeyCode.LEFT);
                }
            }
        }

        public void ToggleFullWidthMode()
        {
            _inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.SHIFT, VirtualKeyCode.SPACE);
        }
    }
}
