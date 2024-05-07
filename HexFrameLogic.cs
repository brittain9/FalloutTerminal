using Terminal.Gui;

namespace fot
{
    public class HexFrameLogic
    {
        private static string hexNumberStart = null;
        public static void CreateHexFrame()
        {
            for (int i = 0; i < FalloutTerminal.HexFrame.Bounds.Bottom; i++)
            {
                // we want to fill up the frame with words. This is problematic as we are also going to want the app to scale.
            }
        }

        public static string GetNextHexNumber(string existingHex = null)
        {
            // If no existing hex number, generate a new random 4-digit hex
            if (string.IsNullOrEmpty(existingHex))
            {
                Random random = new Random();
                int randomNumber = random.Next(0x1000, 0x10000); // Random number between 0x1000 and 0xFFFF
                return randomNumber.ToString("X4"); // Convert to 4-digit hex string
            }
            else
            {
                // Parse the existing hex number and increment it
                int existingNumber = int.Parse(existingHex, System.Globalization.NumberStyles.HexNumber);
                int nextNumber = (existingNumber + 1) & 0xFFFF; // Increment and wrap around to 0x0000 if exceeds 0xFFFF
                return nextNumber.ToString("X4"); // Convert to 4-digit hex string
            }
        }

        public static void OnTextButtonClicked()
        {
            FalloutTerminal.RemainingAttempts--;
            FalloutTerminal.UpdateAttemptsLabel();
            ConsoleFrameLogic.isCorrectWord();
        }
    }
}