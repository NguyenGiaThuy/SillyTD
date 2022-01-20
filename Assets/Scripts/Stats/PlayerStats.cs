using System;

[Serializable]
public class PlayerStats
{
    public int levelIndex;
    public int credits;
    public int lives;
    public int researchPoints;

    public PlayerStats(int levelIndex)
    {
        this.levelIndex = levelIndex;
    }

    public void UpdateStats(int credits, int lives)
    {
        this.credits = credits;
        this.lives = lives;
    }
}
