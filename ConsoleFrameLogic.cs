using Terminal.Gui;

namespace fot
{
    internal class ConsoleFrameLogic
    {
        public static bool ShowWrongAnswer()
        {
            if (FalloutTerminal.ConsoleFrame.Subviews.Count > 1) // Content view is default subview
                FalloutTerminal.ConsoleFrame.Subviews.Clear();

            var wrongLabel = new Label()
            {
                Text = "Wrong attempt.",
                X = 1,
                Y = 6
            };
            FalloutTerminal.ConsoleFrame.Add(wrongLabel);
            return false;
        }
    }
}
