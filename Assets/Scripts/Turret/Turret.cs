using UnityEngine;

public abstract class Turret : MonoBehaviour
{
    public delegate void OnLevelIncreasedHandler();
    public event OnLevelIncreasedHandler LevelIncreased;

    public int ID { get; protected set; }

    [Header("Game Specifications", order = 0)]
    [Header("Optional", order = 1)]
    public int level;

    public void IncreaseLevel()
    {
        level++;
        LevelIncreased?.Invoke();
    }
}
