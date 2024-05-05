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
        public CustomButton() : this(text: string.Empty, is_default: false) { }

        public CustomButton(ustring text, bool is_default = false) : base(text)
        {
            Initialize(text, is_default);
        }

        public CustomButton(int x, int y, ustring text) : this(x, y, text, false) { }

        public CustomButton(int x, int y, ustring text, bool is_default)
            : base(new Rect(x, y, text.RuneCount + 4 + (is_default ? 2 : 0), 1), text)
        {
            Initialize(text, is_default);
        }

        void Initialize(ustring text, bool is_default)
        {
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

        ///<inheritdoc/>
        public override bool ProcessHotKey(KeyEvent kb)
        {
            if (!Enabled)
            {
                return false;
            }

            return ExecuteHotKey(kb);
        }

        ///<inheritdoc/>
        public override bool ProcessColdKey(KeyEvent kb)
        {
            if (!Enabled)
            {
                return false;
            }

            return ExecuteColdKey(kb);
        }

        ///<inheritdoc/>
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

        /// <summary>
        /// Virtual method to invoke the <see cref="Clicked"/> event.
        /// </summary>
        public virtual void OnClicked()
        {
            Clicked?.Invoke();
        }

        /// <summary>
        ///   Clicked <see cref="Action"/>, raised when the user clicks the primary mouse CustomButton within the Bounds of this <see cref="View"/>
        ///   or if the user presses the action key while this view is focused. (TODO: IsDefault)
        /// </summary>
        /// <remarks>
        ///   Client code can hook up to this event, it is
        ///   raised when the CustomButton is activated either with
        ///   the mouse or the keyboard.
        /// </remarks>
        public event Action Clicked;

        ///<inheritdoc/>
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

        ///<inheritdoc/>
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

        ///<inheritdoc/>
        public override bool OnEnter(View view)
        {
            Application.Driver.SetCursorVisibility(CursorVisibility.Invisible);

            return base.OnEnter(view);
        }
    }
}
