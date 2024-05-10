﻿using System.Text;
using Terminal.Gui;

namespace fot
{
    public class HexFrameLogic
    { 
        public event EventHandler<string> ButtonClicked;
        
        private List<string> fourLetterWords = new List<string>()
        {
            "able", "acid", "aged", "also", "area", "army",
            "away", "baby", "back", "ball", "band", "bank",
            "base", "bath", "bear", "beat", "bed",  "beer",
            "bell", "belt", "best", "bill", "bird", "blue",
            "boat", "body", "bond", "bone", "book", "boom",
            "born", "boss", "both", "bowl", "bulk", "burn",
            "bush", "busy", "button", "cafe", "cake", "call",
            "calm", "came", "camp", "card", "care", "case"
        };

        private string hexNumberStart;

        public HexFrameLogic()
        {
            hexNumberStart = null;

            Random random = new Random();
            int randomIndex = random.Next(0, fourLetterWords.Count);
            GameStatistics.CorrectWord = fourLetterWords[randomIndex];
    }
        public void CreateHexFrame(FrameView frame)
        {
            for (int i = 0; i < 20; i++)
            {
                CreateLine(i);
            }
        }

        private void CreateLine(int Y) 
        { 
            StringBuilder sb = new StringBuilder();
            string hexNumber = GetNextHexNumber(hexNumberStart);
            var hexNumLabel = new Label(0, Y, hexNumber); // 0 because its always first
            FalloutTerminal.HexFrame.Add(hexNumLabel);

            var button = new CustomButton(hexNumber.Length + 2, Y, fourLetterWords.ElementAt(0));
            fourLetterWords.RemoveAt(0);
            button.Clicked += OnTextButtonClicked;
            FalloutTerminal.HexFrame.Add(button);
        }

        public void OnTextButtonClicked(object sender) // this will be added to each button in our HexFrame
        {
            CustomButton clickedButton = (CustomButton)sender;
            string chosenWord = clickedButton.CodeWord;
            
            ButtonClicked?.Invoke(this, chosenWord);
        }
        
        private string GetNextHexNumber(string existingHex = null)
        {
            // If no existing hex number, generate a new random 4-digit hex
            if (string.IsNullOrEmpty(existingHex))
            {
                hexNumberStart = existingHex;
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
    }
}