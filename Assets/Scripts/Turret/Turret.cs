using UnityEngine;

public abstract class Turret : MonoBehaviour
{
    public delegate void OnLevelIncreaseHandler(int levels);
    public event OnLevelIncreaseHandler OnLevelIncreased;

    public int id { get; protected set; }
    public int levels { get; protected set; }

    [Header("Game Specifications", order = 0)]
    [Header("Optional", order = 1)]
    
    public TurretParameter turretParameter;
    [SerializeField]
    public GameObject aura;

    public void IncreaseLevelsTo(int levels)
    {
        this.levels = levels;
        OnLevelIncreased?.Invoke(levels);
    }
}
