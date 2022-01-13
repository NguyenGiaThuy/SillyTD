using System;

[Serializable]
public class PlayerStats
{
    public int credits;
    public int lives;
    public int researchPoints;

    public PlayerStats(int credits, int lives, int researchPoints)
    {
        this.credits = credits;
        this.lives = lives;
        this.researchPoints = researchPoints;
    }

    public void UpdateStats(int credits, int lives)
    {
        this.credits = credits;
        this.lives = lives;
    }
}
