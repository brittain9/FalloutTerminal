using Terminal.Gui;
using fot;

namespace fot
{
    public class TextButton : View
    {
        private string _text;
        private Action _clickAction;
        private Button _button;
        public TextButton(string text, int x, int y, Action clickAction)
        {
            _text = text ?? throw new ArgumentNullException(nameof(text));
            _clickAction = clickAction ?? throw new ArgumentNullException(nameof(clickAction));

            X = x;
            Y = y;
        }
        public override void Redraw(Rect bounds)
        {
            base.Redraw(bounds);

            // Ensure the drawing does not exceed the bounds
            Driver.SetAttribute(ColorScheme.Normal);
            Driver.Move(bounds.X, bounds.Y); // Position the cursor at the start of the bounds
            string displayText = _text.Length > bounds.Width ? _text.Substring(0, bounds.Width) : _text;
            Driver.AddStr(displayText);
        }

        public override bool MouseEvent(MouseEvent me)
        {
            // Check if the click is within the button's bounds
            if (me.Flags == MouseFlags.Button1Clicked && this.Bounds.Contains(me.X, me.Y))
            {
                _clickAction.Invoke();
                return true;
            }

            return false;
        }
    }

    public class FalloutTerminal : Window
    {
        private bool isDevMode = true;

        // Components
        private MenuBar _devBar;
        private Label _title;
        private Label _enterPassword;
        private Label _attemptsLabel;
        private FrameView _hexFrame;
        private FrameView _consoleFrame;

        public FalloutTerminal()
        {
            ColorScheme = new ColorScheme
            {
                Normal = Terminal.Gui.Attribute.Make(Color.Green, Color.Black),
            };

            _title = new Label()
            {
                Text = "ROBCO INDUSTRIES (TM) TERMLINK PROTOCOL",
                X = 0, // start at left most at everything relative to this
                Y = isDevMode ? 1 : 0,
            };
            _enterPassword = new Label()
            {
                Text = "ENTER PASSWORD NOW",
                X = Pos.Left(_title),
                Y = Pos.Top(_title) + 1
            };
            _attemptsLabel = new Label()
            {
                Text = AppLogic.RemainingAttempts + " ATTEMPT(S) LEFT: " + new string('█', AppLogic.RemainingAttempts),
                X = Pos.Left(_title),
                Y = Pos.Top(_enterPassword) + 3
            };

            _hexFrame = new FrameView()
            {
                X = Pos.Left(_attemptsLabel),
                Y = Pos.Bottom(_attemptsLabel) + 2,
                Width = Dim.Percent(70),
                Height = Dim.Fill(),
                ColorScheme = ColorScheme
            };

            var btton = new CustomButton()
            {
                Text = "hello",
                X = 1,
                Y = 2
            };

            btton.Clicked += () =>
            {
                AppLogic.RemainingAttempts--;
            };

            var btton1 = new CustomButton()
            {
                Text = "safsaf",
                X = 0,
                Y = 1
            };

            btton1.Clicked += () =>
            {
                AppLogic.RemainingAttempts--;
            };

            _hexFrame.Add(btton1);
            _hexFrame.Add(btton);


            _consoleFrame = new FrameView()
            {
                X = Pos.Right(_hexFrame),
                Y = Pos.Top(_hexFrame),
                Width = Dim.Percent(30),
                Height = Dim.Fill()
            };

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
                                AppLogic.RemainingAttempts = attempts;
                                _updateAttemptsLabel();
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
                        AppLogic.RemainingAttempts = 4;
                        _updateAttemptsLabel();
                    })
                })
            });

            Add(_devBar, _title, _enterPassword, _attemptsLabel, _hexFrame, _consoleFrame);
        }

        public override bool OnKeyDown(KeyEvent keyEvent)
        {
            if (keyEvent.Key == Key.F4)
            {
                isDevMode = !isDevMode;
                _devBar.Visible = isDevMode;
                _adjustElementPositions();
                SetNeedsDisplay(); // StateChanged rerender
                return true; // Indicate that the key press was handled
            }
            return base.OnKeyDown(keyEvent);
        }

        private void _adjustElementPositions()
        {
            _title.Y = isDevMode ? 1 : 0;
            _enterPassword.Y = Pos.Top(_title) + 1;
            _attemptsLabel.Y = Pos.Top(_enterPassword) + 3;
            _hexFrame.Y = Pos.Top(_attemptsLabel) + 1;
            _consoleFrame.Y = Pos.Top(_attemptsLabel) + 1;
        }

        private void _updateAttemptsLabel()
        {
            if(AppLogic.RemainingAttempts >= 0)
            {
                _attemptsLabel.Text = AppLogic.RemainingAttempts + " ATTEMPT(S) LEFT: " + new string('█', AppLogic.RemainingAttempts);
            }
            // TODO: Handle no more attempts
        }
    }
}
