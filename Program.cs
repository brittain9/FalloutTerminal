using System.Diagnostics.Metrics;
using System.Globalization;
using Terminal.Gui;

Application.Run<FalloutTerminal>();

// Before the application exits, reset Terminal.Gui for clean shutdown
Application.Shutdown();

// Defines a top-level window with border and title
public class FalloutTerminal : Window
{
    private bool isDevMode = true;
    private int remainingAttempts = 4;

    // Components
    private MenuBar _devBar;
    private Label _title;
    private Label _enterPassword;
    private Label _attemptsLabel;
    private FrameView _hexFrame1;
    private FrameView _hexFrame2;
    private FrameView _consoleFrame;

    public FalloutTerminal()
    {
        ColorScheme = new ColorScheme
        {
            Normal = Terminal.Gui.Attribute.Make(Color.Green, Color.Black),
        };

        _devBar = new MenuBar(new[]
        {
            new MenuBarItem("_File", new[]
            {
                new MenuItem("_Quit", "", () => Application.RequestStop())
            }),
            new MenuBarItem("_Variables", new[]
            {
                new MenuItem("_Remaining Attempts", "", () => MessageBox.Query("Remaining Attempts", $"Remaining Attempts: {remainingAttempts}", "OK")),
                new MenuItem("_Reset Attempts", "", () =>
                {
                    remainingAttempts = 4;
                    _updateAttemptsLabel();
                })
            })
        });

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
            Text = remainingAttempts + " ATTEMPT(S) LEFT: " + new string('█', remainingAttempts),
            X = Pos.Left(_title),
            Y = Pos.Top(_enterPassword) + 3
        };

        _hexFrame1 = new FrameView()
        {
            X = Pos.Left(_attemptsLabel),
            Y = Pos.Bottom(_attemptsLabel) + 2,
            Width = Dim.Percent(40),
            Height = Dim.Fill()
        };
        _hexFrame2 = new FrameView()
        {
            X = Pos.Right(_hexFrame1),
            Y = Pos.Top(_hexFrame1),
            Width = Dim.Percent(40),
            Height = Dim.Fill()
        };
        _consoleFrame = new FrameView()
        {
            X = Pos.Right(_hexFrame2),
            Y = Pos.Top(_hexFrame2),
            Width = Dim.Percent(20),
            Height = Dim.Fill()
        };

        Add(_devBar, _title, _enterPassword, _attemptsLabel, _hexFrame1, _hexFrame2, _consoleFrame);
    }
    public override bool OnKeyDown(KeyEvent keyEvent)
    {
        if (keyEvent.Key == Key.F4)
        {
            isDevMode = !isDevMode;
            _devBar.Visible = isDevMode;
            _adjustElementPositions();
            // SetNeedsDisplay(); // StateChanged rerender
            return true; // Indicate that the key press was handled
        }
        return base.OnKeyDown(keyEvent);
    }
    private void _adjustElementPositions()
    {
        _title.Y = isDevMode ? 1 : 0;
        _enterPassword.Y = Pos.Top(_title) + 1;
        _attemptsLabel.Y = Pos.Top(_enterPassword) + 3;
        _hexFrame1.Y = Pos.Top(_attemptsLabel) + 1;
        _hexFrame2.Y = Pos.Top(_attemptsLabel) + 1;
        _consoleFrame.Y = Pos.Top(_attemptsLabel) + 1;
    }
    private void _updateAttemptsLabel()
    {
        _attemptsLabel.Text = remainingAttempts + " ATTEMPT(S) LEFT: " + new string('█', remainingAttempts);
    }
}
