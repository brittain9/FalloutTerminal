using Terminal.Gui;
using fot;

class Program
{
    static void Main(string[] args)
    {
        Application.Init();

        var startWindow = new StartWindow();
        var falloutTerminal = new FalloutTerminal();
        var endingWindow = new EndGameWindow();

        // TODO: Allow user to select a difficulties or settings in the start menu
        startWindow.StartClicked += (sender, e) =>
        {
            Application.RequestStop();
            Application.Run(falloutTerminal);
        };
        
        // TODO: Provide some post game statistics
        HexFrameLogic.GameEnded += (sender, e) =>
        {
            Application.RequestStop();
            Application.Run(endingWindow);
        };

        Application.Run(startWindow);
        Application.Shutdown();
    }
}