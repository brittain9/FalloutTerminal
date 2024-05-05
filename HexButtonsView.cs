using Terminal.Gui;

public class SpecialLineView : View
{
    private Button _button;
    private string _text;

    public SpecialLineView(string text)
    {
        _text = text;
        CreateButton();
    }

    private void CreateButton()
    {
        _button = new Button("Click")
        {
            X = Pos.AnchorEnd(1),
            Y = Pos.Center(),
            ColorScheme = new ColorScheme
            {
                Normal = new Terminal.Gui.Attribute(Color.White, Color.Black),
                Focus = new Terminal.Gui.Attribute(Color.Black, Color.Green),
                HotNormal = new Terminal.Gui.Attribute(Color.White, Color.Black),
                HotFocus = new Terminal.Gui.Attribute(Color.Black, Color.Green)
            }
        };

        _button.Clicked += () =>
        {
            // Handle button click event
            MessageBox.Query("Button Clicked", $"You clicked the button in line: {_text}", "OK");
        };

        Add(_button);
    }

    public override void Redraw(Rect bounds)
    {
        base.Redraw(bounds);

        Move(0, 0);
        Driver.AddStr(_text);
    }
}
