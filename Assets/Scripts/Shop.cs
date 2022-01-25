using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public TurretBlueprint turretBlueprint;

    // Hidden fields
    private float sellMultiplier;
    BuildManager buildManager;

    private void Awake() 
    {
        buildManager = FindObjectOfType<BuildManager>();
        GameManager.Instance.SubscribeToOnGameStateChanged(GameManager_OnStateChanged);
    }

    private void OnDestroy()
    {
        GameManager.Instance.UnsubscribeToOnGameStateChanged(GameManager_OnStateChanged);
    }

    public void PurchaseTurret(int turretID) 
    {
        TurretBlueprint.Model model = turretBlueprint.models[turretID];

        if (GameManager.Instance.playerStats.credits < model.buildCost)
        {
            Debug.Log("Not enough credits to build!");
            return;
        }

        GameManager.Instance.playerStats.credits -= model.buildCost;
        buildManager.BuildTurret(model);

    }

    public void UpgradeTurret()
    {
        TurretBlueprint.Model model = turretBlueprint.models[buildManager.CurrentTurret.ID];
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
        buildManager.UpgradeTurret();
    }

    public void SellTurret()
    {
        TurretBlueprint.Model model = turretBlueprint.models[buildManager.CurrentTurret.ID];
        int currentLevel = buildManager.CurrentTurret.level - 1;
        int totalUpgradeCost = 0;
        for (int i = 1; i <= currentLevel; i++) totalUpgradeCost += model.upgradeCosts[i];
        GameManager.Instance.playerStats.credits += Mathf.RoundToInt(sellMultiplier * (model.buildCost + totalUpgradeCost));
        buildManager.DemolishTurret();
    }

    private void GameManager_OnStateChanged(GameStateManager.GameState gameState)
    {
        if (gameState == GameStateManager.GameState.Preparing) sellMultiplier = 1f;
        else sellMultiplier = 0.5f;
    }
}
