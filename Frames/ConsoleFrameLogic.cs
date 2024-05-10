using Terminal.Gui;

namespace fot
{
    internal class ConsoleFrameLogic
    {
        private List<string> consoleOutput; // this will hold all the lines of text for the words chosen and the similarity

        public ConsoleFrameLogic()
        {
            consoleOutput ??= new();
        }
        public void UpdateConsoleFrame(string chosenWord)
        {
            AddConsoleOutput(chosenWord);
            ConsoleOutputToFrame();
        }
        private void AddConsoleOutput(string chosenWord)
        {
            consoleOutput.Add($"Wrong word: {chosenWord}");
            consoleOutput.Add($"{getSimilarLetters(chosenWord, GameStatistics.CorrectWord)} / {GameStatistics.CorrectWord.Length} letters similar");
        }

        private int getSimilarLetters(string word1, string word2)
        {
            int similarLetters = 0;
            int n = word1.Length >= word2.Length ? word2.Length : word1.Length; // words should be same length but use smaller word to avoid errors
            
            for (int i = 0; i < n; i++)
            {
                if (word1[i] == word2[i]) similarLetters++;
            }

            return similarLetters;
        }
        
        private void ConsoleOutputToFrame()
        {
            if (FalloutTerminal.ConsoleFrame.Subviews.Count > 1) // Content view is default subview == 1
                FalloutTerminal.ConsoleFrame.Subviews.Clear();

            for (int i = 0; i < consoleOutput.Count; i++)
            {
                var label = new Label()
                {
                    Text = consoleOutput[i],
                    X = 0,
                    Y = i + 1
                };
                FalloutTerminal.ConsoleFrame.Add(label);
            }
        }
    }
}
