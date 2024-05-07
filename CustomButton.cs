using System;
using NStack;
using Terminal.Gui;

namespace fot
{
    // This is just the Terminal GUI Button, but I stripped the brackets and formatting.
    // Button Class implementation makes a button with brackets: [ Click me ]
    // This new class just makes a button with text only: Click me

    public class CustomButton : View
    {
        public string CodeWord { get; set; } // each button will be associated with the CodeWord for comparison
        public CustomButton(string text) : base(text)
        {
            Initialize(text);
        }

        public CustomButton(int x, int y, string text) : base(new Rect(x, y, text.Length, 1), text)
        {
            Initialize(text);
        }

        void Initialize(ustring text)
        {
            CodeWord = text.ToString();

            ColorScheme = new ColorScheme
            {
                Normal = new Terminal.Gui.Attribute(Color.White, Color.Black),
                Focus = new Terminal.Gui.Attribute(Color.Black, Color.Green),
                HotNormal = new Terminal.Gui.Attribute(Color.White, Color.Black),
                HotFocus = new Terminal.Gui.Attribute(Color.Black, Color.Green)
            };

            TextAlignment = TextAlignment.Centered;
            VerticalTextAlignment = VerticalTextAlignment.Middle;

            HotKeySpecifier = new Rune('_');

            CanFocus = true;
            AutoSize = true;
            Text = text ?? string.Empty;
            UpdateTextFormatterText();
            ProcessResizeView();

            // Things this view knows how to do
            AddCommand(Command.Accept, () => AcceptKey());

            // Default keybindings for this view
            AddKeyBinding(Key.Enter, Command.Accept);
            AddKeyBinding(Key.Space, Command.Accept);
            if (HotKey != Key.Null)
            {
                AddKeyBinding(Key.Space | HotKey, Command.Accept);
            }
        }

        protected override void UpdateTextFormatterText()
        {
            TextFormatter.Text =  Text;
        }

        public override bool ProcessHotKey(KeyEvent kb)
        {
            if (!Enabled)
            {
                return false;
            }

            return ExecuteHotKey(kb);
        }

        public override bool ProcessColdKey(KeyEvent kb)
        {
            if (!Enabled)
            {
                return false;
            }

            return ExecuteColdKey(kb);
        }

        public override bool ProcessKey(KeyEvent kb)
        {
            if (!Enabled)
            {
                return false;
            }

            var result = InvokeKeybindings(kb);
            if (result != null)
                return (bool)result;

            return base.ProcessKey(kb);
        }

        bool ExecuteHotKey(KeyEvent ke)
        {
            if (ke.Key == (Key.AltMask | HotKey))
            {
                return AcceptKey();
            }
            return false;
        }

        // two methods below used is default in if statement?
        bool ExecuteColdKey(KeyEvent ke)
        {
            if (ke.KeyValue == '\n')
            {
                return AcceptKey();
            }
            return ExecuteHotKey(ke);
        }

        bool AcceptKey()
        {
            if (!HasFocus)
            {
                SetFocus();
            }
            OnClicked();
            return true;
        }

        public virtual void OnClicked()
        {
            Clicked?.Invoke();
        }

        public event Action Clicked;

        public override bool MouseEvent(MouseEvent me)
        {
            if (me.Flags == MouseFlags.Button1Clicked)
            {
                if (CanFocus && Enabled)
                {
                    if (!HasFocus)
                    {
                        SetFocus();
                        SetNeedsDisplay();
                        Redraw(Bounds);
                    }
                    OnClicked();
                }

                return true;
            }
            return false;
        }

        public override void PositionCursor()
        {
            if (HotKey == Key.Unknown && Text != "")
            {
                for (int i = 0; i < TextFormatter.Text.RuneCount; i++)
                {
                    if (TextFormatter.Text[i] == Text[0])
                    {
                        Move(i, 0);
                        return;
                    }
                }
            }
            base.PositionCursor();
        }

        public override bool OnEnter(View view)
        {
            Application.Driver.SetCursorVisibility(CursorVisibility.Invisible);

            return base.OnEnter(view);
        }
    }
}
