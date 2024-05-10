using Terminal.Gui;

namespace fot
{
    public class StartWindow : Window
    {
        public event EventHandler StartClicked;

        public StartWindow()
        {
            ColorScheme = new ColorScheme
            {
                Normal = Terminal.Gui.Attribute.Make(Color.Green, Color.Black),
            };
            
            Title = "Start Window";

            var startButton = new Button("Start")
            {
                X = Pos.Center(),
                Y = Pos.Center()
            };

            startButton.Clicked += StartButton_Clicked;

            Add(startButton);
        }

        private void StartButton_Clicked()
        {
            StartClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}