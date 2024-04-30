using Terminal.Gui;

Application.Run<FalloutTerminal>();

// Before the application exits, reset Terminal.Gui for clean shutdown
Application.Shutdown();

// Defines a top-level window with border and title
public class FalloutTerminal : Window
{
    private int remainingAttempts = 4;
    private Label attemptsLabel;

    public FalloutTerminal()
    {
        ColorScheme = new ColorScheme
        {
            Normal = Terminal.Gui.Attribute.Make(Color.Green, Color.Black),
            Focus = Terminal.Gui.Attribute.Make(Color.Black, Color.White),
        };

        var title = new Label("ROBCO INDUSTRIES (TM) TERMLINK PROTOCOL")
        {
            X = 0,
            Y = 0
        };
        var enter = new Label("ENTER PASSWORD NOW")
        {
            X = 0,
            Y = 1
        };

        attemptsLabel = new Label()
        {
            Text = remainingAttempts + " ATTEMPT(S) LEFT: " + new string('█', remainingAttempts),
            X = 0,
            Y = 4
        };

        // Create input components and labels
        var usernameLabel = new Label("Username:")
        {
            X = Pos.Left(attemptsLabel),
            Y = Pos.Bottom(attemptsLabel) + 2
        };

        var usernameText = new TextField("")
        {
            X = Pos.Right(usernameLabel) + 1,
            Y = Pos.Top(usernameLabel),
            Width = Dim.Fill(),
        };

        var passwordLabel = new Label()
        {
            Text = "Password:",
            X = Pos.Left(usernameLabel),
            Y = Pos.Bottom(usernameLabel) + 1
        };

        var passwordText = new TextField("")
        {
            Secret = true,
            X = Pos.Left(usernameText),
            Y = Pos.Top(passwordLabel),
            Width = Dim.Fill(),
        };

        // Create login button
        var btnLogin = new Button()
        {
            Text = "Login",
            Y = Pos.Bottom(passwordLabel) + 1,
            X = Pos.Center(),
            IsDefault = true,
        };

        // When login button is clicked display a message popup
        btnLogin.Clicked += () =>
        {
            if (usernameText.Text == "admin" && passwordText.Text == "password")
            {
                MessageBox.Query("Logging In", "Login Successful", "Ok");
                Application.RequestStop();
            }
            else
            {
                remainingAttempts--;
                UpdateAttemptsLabel();

                if (remainingAttempts == 0)
                {
                    MessageBox.ErrorQuery("Login Failed", "No more attempts remaining", "Ok");
                    Application.RequestStop();
                }
                else
                {
                    MessageBox.ErrorQuery("Logging In", "Incorrect username or password", "Ok");
                }
            }
        };

        // Add the views to the Window
        Add(title, enter, usernameLabel, usernameText, passwordLabel, passwordText, attemptsLabel, btnLogin);
    }

    private void UpdateAttemptsLabel()
    {
        attemptsLabel.Text = remainingAttempts + " ATTEMPT(S) LEFT: " + new string('█', remainingAttempts);
    }
}
