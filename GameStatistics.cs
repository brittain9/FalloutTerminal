namespace fot;

public static class GameStatistics
{
    /*
     * This class will keep track of time, attempts, wins, losses. It will affect how some of the Windows appear like the start and end game window.
     * If this class was just started, start window will just say start, [settings]; if the user has been playing, it will say continue, settings, end game.
     */
    public static bool IsGameOver { get; set; } = false;
    public static int GamesPlayed { get; set; } // inc each time a fallout terminal is created
    public static int RemainingAttempts { get; set; } = 4;
    public static string CorrectWord { get; set; } // in some way this class is now for global variables
}