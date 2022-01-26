using UnityEngine;

public abstract class Turret : MonoBehaviour
{
    public delegate void OnLevelIncreasedHandler();
    public event OnLevelIncreasedHandler OnLevelIncreased;

    public int ID { get; protected set; }

    [Header("Game Specifications", order = 0)]
    [Header("Optional", order = 1)]
    public int level;
    public TurretStats turretStats;
    [SerializeField]
    public GameObject aura;

    public void IncreaseLevel()
    {
        level++;
        OnLevelIncreased?.Invoke();
    }
}
