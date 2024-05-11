using System.Text;
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
            "base", "bath", "bear", "beat",  "beer",
            "bell", "belt", "best", "bill", "bird", "blue",
            "boat", "body", "bond", "bone", "book", "boom",
            "born", "boss", "both", "bowl", "bulk", "burn",
            "bush", "busy", "button", "cafe", "cake", "call",
            "calm", "came", "camp", "card", "care", "case"
        };

        private int nextHexNumber;

        public HexFrameLogic()
        {
            Random random = new Random();
            nextHexNumber = random.Next(0x1000, 0x10000);
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
            string hexNumber = GetHexNumber();
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
        
        private string GetHexNumber()
        {
                int nextNumber = (nextHexNumber + 1) & 0xFFFF; // Increment and wrap around to 0x0000 if exceeds 0xFFFF
                nextHexNumber++;
                return "0x" + nextNumber.ToString("X4"); // Convert to 4-digit hex string
        }
    }
}