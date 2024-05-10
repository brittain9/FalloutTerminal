using Terminal.Gui;

namespace fot
{
    public class StartWindow : Window
    {
        public static event EventHandler StartClicked;
        public static event EventHandler EndClicked;

        public StartWindow()
        {
            ColorScheme = new ColorScheme
            {
                Normal = Terminal.Gui.Attribute.Make(Color.Green, Color.Black),
            };
            
            if (GameStatistics.GamesPlayed > 0)
            {
                var startButton = new CustomButton("Play Again")
                {
                    X = Pos.Center(),
                    Y = Pos.Center()
                };
                startButton.Clicked += StartButton_Clicked;
                
                var endButton = new CustomButton("End Game")
                {
                    X = Pos.Right(startButton) + 5,
                    Y = Pos.Center()
                };
                endButton.Clicked += EndButton_Clicked;
                
                Add(startButton, endButton);
                
            }
            else
            {
                // eventually id like to do a cool intro like fallout
                var startButton = new CustomButton("Start")
                {
                    X = Pos.Center(),
                    Y = Pos.Center()
                };
                startButton.Clicked += StartButton_Clicked;
                Add(startButton);
            }
        }

        private void StartButton_Clicked(object sender)
        {
            StartClicked?.Invoke(sender, EventArgs.Empty);
        }
        
        private void EndButton_Clicked(object sender)
        {
            EndClicked?.Invoke(sender, EventArgs.Empty);
        }
    }
}