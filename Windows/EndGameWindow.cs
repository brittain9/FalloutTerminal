using Terminal.Gui;

namespace fot
{
    public class EndGameWindow : Window
    {
        public EndGameWindow()
        {
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

            Add(endingLabel);
            
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