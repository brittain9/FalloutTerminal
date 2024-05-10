namespace fot;

public class GameOverEventArgs : EventArgs
{
    public bool Won { get; set; }

    public GameOverEventArgs(bool won)
    {
        Won = won;
    }
}