using Terminal.Gui;
using fot;

class Program
{
    static void Main(string[] args)
    {
        Application.Init();
        
        while (!GameStatistics.IsGameOver) // the game isn't over. Idk where to put this variable right now
        {
            var startWindow = new StartWindow();

            StartWindow.StartClicked += (sender, e) =>
            {
                Application.RequestStop();
                var falloutTerminal = new FalloutTerminal();
                Application.Run(falloutTerminal);
            };
            StartWindow.EndClicked += (sender, e) =>
            {
                GameStatistics.IsGameOver = true;
                Application.RequestStop();
                Application.Run<EndGameWindow>();
            };

            FalloutTerminal.GameOver += (object sender, GameOverEventArgs e) =>
            {
                Application.RequestStop();

                var startWindow = new StartWindow();
                Application.Run(startWindow); // ask them to try again
            };


            Application.Run(startWindow);
        }

        Application.Shutdown();
    }
    
}