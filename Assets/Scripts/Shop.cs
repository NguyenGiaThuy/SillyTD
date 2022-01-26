using UnityEngine;

public class Shop : MonoBehaviour
{
    public int SellCredits { get; private set; }
    public float SellMultiplier { get; private set; }

    // Hidden fields
    BuildManager buildManager;

    private void Awake() 
    {
        buildManager = FindObjectOfType<BuildManager>();
        GameManager.Instance.SubscribeToOnStateChanged(GameManager_OnStateChanged);
    }

    private void OnDestroy()
    {
        GameManager.Instance.UnsubscribeToOnStateChanged(GameManager_OnStateChanged);
    }

    public void PurchaseTurret(int turretID) 
    {
        TurretBlueprint model = BuildManager.turretBlueprints[turretID];

        if (GameManager.Instance.playerStats.credits < model.buildCost)
        {
            Debug.Log("Not enough credits to build!");
            return;
        }

        GameManager.Instance.playerStats.credits -= model.buildCost;
        SellCredits = model.buildCost;
        buildManager.BuildTurret(model);

    }

    public void UpgradeTurret()
    {
        TurretBlueprint model = BuildManager.turretBlueprints[buildManager.CurrentTurret.ID];
        int nextLevel = buildManager.CurrentTurret.level;

        if(nextLevel >= model.upgradeCosts.Length)
        {
            Debug.Log("Upgrade maxed");
            return;
        }

        if (GameManager.Instance.playerStats.credits < model.upgradeCosts[nextLevel])
        {
            Debug.Log("Not enough credits to upgrade!");
            return;
        }

        GameManager.Instance.playerStats.credits -= model.upgradeCosts[nextLevel];
        SellCredits += model.upgradeCosts[nextLevel];
        buildManager.UpgradeTurret();
    }

    public void SellTurret()
    {
        GameManager.Instance.playerStats.credits += Mathf.RoundToInt(SellMultiplier * SellCredits);
        buildManager.DemolishTurret();
    }

    private void GameManager_OnStateChanged(GameStateManager.GameState gameState)
    {
        if (gameState == GameStateManager.GameState.Preparing) SellMultiplier = 1f;
        else SellMultiplier = 0.5f;
    }
}
