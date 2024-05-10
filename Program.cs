using Terminal.Gui;
using fot;

class Program
{
    static void Main(string[] args)
    {
        Application.Init();
        
        StartWindow.StartClicked += (sender, e) =>
        {
            Application.RequestStop();
            Application.Run<FalloutTerminal>();
        };
        StartWindow.EndClicked += (sender, e) =>
        {
            Application.RequestStop();
            Application.Run<EndGameWindow>();
        };

        FalloutTerminal.GameOver += (object sender, GameOverEventArgs e) =>
        {
            Application.RequestStop();
            Application.Run<StartWindow>();
        };
        
        Application.Run<StartWindow>();
        
        Application.Shutdown();
    }
    
}