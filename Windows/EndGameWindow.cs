using Terminal.Gui;

namespace fot
{
    public class EndGameWindow : Window
    {
        public static bool IsGameOver { get; set; } = false;
        private GameStatistics gameStats;
        public EndGameWindow(GameStatistics stats)
        {
            gameStats = stats;
            
            ColorScheme = new ColorScheme
            {
                Normal = Terminal.Gui.Attribute.Make(Color.Green, Color.Black),
            };
            
            Title = "Ending Window";

            var endingLabel = new Label("Game Over")
            {
                X = Pos.Center(),
                Y = Pos.Center()
            };
            var statsLabel = new Label($"Games played: {gameStats.GamesPlayed}")
            {
                X = Pos.Center(),
                Y = Pos.Top(endingLabel) + 2
            };

            Add(endingLabel, statsLabel);
            
            KeyPress += EndGameWindow_KeyPress;
        }
        
        private void EndGameWindow_KeyPress(View.KeyEventEventArgs args)
        {
            if (args.KeyEvent.Key == Key.Enter)
            {
                // Close the program when Enter key is pressed
                Application.RequestStop();
            }
        }
    }
}