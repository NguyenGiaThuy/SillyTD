using System;


[Serializable]
public class PlayerData
{
    public int credits;
    public int lives;
    public int researchPoints;

    public void Save(PlayerStats playerStats)
    {
        credits = playerStats.credits;
        lives = playerStats.lives;
        researchPoints = playerStats.researchPoints;
    }

    public void Load(PlayerStats playerStats)
    {
        playerStats.credits = credits;
        playerStats.lives = lives;
        playerStats.researchPoints = researchPoints;
    }
}
