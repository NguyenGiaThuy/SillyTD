using System;

[Serializable]
public class PlayerStats
{
    public int credits;
    public int lives;
    public int researchPoints;

    public PlayerStats()
    {
        researchPoints = 0;
    }

    public void LoadLevelStats(LevelStats levelStats)
    {
        credits = levelStats.initialCredits;
        lives = levelStats.initialLives;
    }
}
