using Terminal.Gui;
using fot;

class Program
{
    static void Main(string[] args)
    {
        Application.Init();
        
        // I need two more windows. New Game/Continue Game window similar to start.
        // Actually just one window, an intermediary window for looping the game and changing settings.
        // Ending window should showcase the stats.

        var gameStats = new GameStatistics();

        var endingWindow = new EndGameWindow(gameStats);

        while (!EndGameWindow.IsGameOver) // the game isn't over. Idk where to put this variable right now
        {
            var startWindow = new StartWindow(gameStats);
            
            // essentially we have these different events in our program we need to subscribe to.
            // Players have buttons they can press that do different things like starting and ending the game
            // Players can also lose or win the fallout terminal game
            
            // depending on these events we need to redirect them to the right window.
            // Start Screen -> Start button-> Game -> Lose -> End button-> Ending Screen
            // Start Screen -> Start button-> Game -> Win -> Continue button -> Game -> Lose -> End button -> Ending Screen
            // These are different paths our player can take.

            startWindow.StartClicked += (sender, e) =>
            {
                Application.RequestStop();
                var falloutTerminal = new FalloutTerminal(gameStats);
                Application.Run(falloutTerminal);
            };
            startWindow.EndClicked += (sender, e) =>
            {
                // We are ending the game here due to the user clicking a button
                EndGameWindow.IsGameOver = true;
                Application.RequestStop();
                Application.Run(endingWindow);
            };
        
            
            // TODO: Provide some post game statistics
            HexFrameLogic.GameEnded += (sender, e) =>
            {
                // we have lost the game
                Application.RequestStop();
                
                startWindow = new StartWindow(gameStats);
                
                
                startWindow.StartClicked += (sender, e) =>
                {
                    Application.RequestStop();
                    var falloutTerminal = new FalloutTerminal(gameStats);
                    Application.Run(falloutTerminal);
                };
                startWindow.EndClicked += (sender, e) =>
                {
                    // We are ending the game here due to the user clicking a button
                    EndGameWindow.IsGameOver = true;
                    Application.RequestStop();
                    Application.Run(endingWindow);
                };
                
                Application.Run(startWindow); // ask them to try again
                
                
            };

            Application.Run(startWindow);
        }

        Application.Shutdown();
    }
    
}