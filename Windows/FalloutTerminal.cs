﻿using Terminal.Gui;
using fot;
using static Terminal.Gui.Application;

namespace fot
{
    public class FalloutTerminal : Window
    {
        // TODO: Handle resizing
        public static event EventHandler<GameOverEventArgs> GameOver;
        public static Label Title { get; set; }
        public static Label EnterPassword { get; set; }
        public static FrameView HexFrame { get; set; } // this frame is where the buttons are
        private HexFrameLogic hexLogic;

        public static FrameView ConsoleFrame { get; set; } // this frame is for the messages
        private ConsoleFrameLogic consoleLogic; 

        private static Label _attemptsLabel;

        private bool isDevMode = false;
        private MenuBar _devBar;
        

        public FalloutTerminal()
        {
            GameStatistics.GamesPlayed++;

            GameStatistics.RemainingAttempts = 4;
            
            ColorScheme = new ColorScheme
            {
                Normal = Terminal.Gui.Attribute.Make(Color.Green, Color.Black),
            };

            Title = new Label()
            {
                Text = "ROBCO INDUSTRIES (TM) TERMLINK PROTOCOL",
                X = 0, // start at left most at everything relative to this
                Y = isDevMode ? 1 : 0,
            };
            EnterPassword = new Label()
            {
                Text = "ENTER PASSWORD NOW",
                X = Pos.Left(Title),
                Y = Pos.Top(Title) + 1
            };
            _attemptsLabel = new Label()
            {
                Text = GameStatistics.RemainingAttempts + " ATTEMPT(S) LEFT: " + new string('█', GameStatistics.RemainingAttempts),
                X = Pos.Left(Title),
                Y = Pos.Top(EnterPassword) + 3
            };

            HexFrame = new FrameView()
            {
                X = Pos.Left(_attemptsLabel),
                Y = Pos.Bottom(_attemptsLabel) + 2,
                Width = Dim.Percent(70),
                Height = Dim.Fill(),
                ColorScheme = ColorScheme
            };
            hexLogic = new();
            hexLogic.CreateHexFrame(HexFrame);
            hexLogic.ButtonClicked += HexLogic_ButtonClicked; // subscribe to button clicked event

            ConsoleFrame = new FrameView()
            {
                X = Pos.Right(HexFrame),
                Y = Pos.Top(HexFrame),
                Width = Dim.Percent(30),
                Height = Dim.Fill()
            };
            consoleLogic = new();

            _devBar = new MenuBar(new[]
            {
                new MenuBarItem("_File", new[]
                {
                    new MenuItem("_Quit", "", () => Application.RequestStop())
                }),
                new MenuBarItem("_Variables", new[]
                {
                    new MenuItem("_Set Attempts", "", () =>
                    {
                        var inputDialog = new Dialog("Set Attempts", 50, 15);
                        var inputLabel = new Label("Enter the number of attempts:");
                        var inputText = new TextField()
                        {
                            X = Pos.Right(inputLabel) + 1,
                            Y = Pos.Top(inputLabel),
                            Width = 10
                        };
                        inputDialog.Add(inputLabel, inputText);

                        var okButton = new Button("OK");
                        okButton.Clicked += () =>
                        {
                            if (int.TryParse(inputText.Text.ToString(), out int attempts) && attempts > 0)
                            {
                                GameStatistics.RemainingAttempts = attempts;
                                UpdateAttemptsLabel();
                            }
                            else
                            {
                                MessageBox.ErrorQuery("Invalid Input", "Please enter a valid positive integer.", "OK");
                            }

                            Application.RequestStop();
                        };
                        inputDialog.AddButton(okButton);

                        var cancelButton = new Button("Cancel");
                        cancelButton.Clicked += () => Application.RequestStop();
                        inputDialog.AddButton(cancelButton);

                        Application.Run(inputDialog);
                    }),
                    new MenuItem("_Reset Attempts", "", () =>
                    {
                        GameStatistics.RemainingAttempts = 4;
                        UpdateAttemptsLabel();
                    }),
                    new MenuItem("_Show CorrectWord", "", () =>
                    {
                        MessageBox.Query("CorrectWord", $"The correct word is: {GameStatistics.CorrectWord}", "OK");
                    })
                })
            });
            _devBar.Visible = isDevMode;

            Add(_devBar, Title, EnterPassword, _attemptsLabel, HexFrame, ConsoleFrame);
        }
        
        private void HexLogic_ButtonClicked(object sender, string chosenWord)
        {
            // Handle the button click event here
            // For example, check if the chosen word is correct
            if (chosenWord == GameStatistics.CorrectWord)
                GameOver_Handler(true);
            else
            {
                GameStatistics.RemainingAttempts--;
                UpdateAttemptsLabel();
                consoleLogic.UpdateConsoleFrame(chosenWord);
                
                if (GameStatistics.RemainingAttempts <= 0)
                {
                    GameOver_Handler(false); // you lost
                }
            }
        }
        
        public override bool OnKeyDown(KeyEvent keyEvent)
        {
            if (keyEvent.Key == Key.F4)
            {
                isDevMode = !isDevMode;
                _devBar.Visible = isDevMode;
                AdjustUIElements();
                SetNeedsDisplay(); // StateChanged rerender
                return true; // Indicate that the key press was handled
            }
            return base.OnKeyDown(keyEvent);
        } // this handles dev bar
        public void UpdateAttemptsLabel() 
        {
            if (GameStatistics.RemainingAttempts >= 0)
            {
                _attemptsLabel.Text = GameStatistics.RemainingAttempts + " ATTEMPT(S) LEFT: " + new string('█', GameStatistics.RemainingAttempts);
            }
        }
        
        private void AdjustUIElements()
        {
            Title.X = 0;
            Title.Y = isDevMode ? 1 : 0;

            EnterPassword.X = 0;
            EnterPassword.Y = Pos.Top(Title) + 1;

            _attemptsLabel.X = 0;
            _attemptsLabel.Y = Pos.Top(EnterPassword) + 3;

            HexFrame.X = 0;
            HexFrame.Y = Pos.Top(_attemptsLabel) + 1;
            HexFrame.Width = Dim.Percent(70);
            HexFrame.Height = Dim.Fill();

            ConsoleFrame.X = Pos.Right(HexFrame);
            ConsoleFrame.Y = Pos.Top(HexFrame);
            ConsoleFrame.Width = Dim.Percent(30);
            ConsoleFrame.Height = Dim.Fill();
        }
        
        public void GameOver_Handler(bool won)
        {
            GameOver?.Invoke(this, new GameOverEventArgs(won));
        }
    }
}
