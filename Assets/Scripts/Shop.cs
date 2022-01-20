using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public TurretBlueprint turretBlueprint;

    // Hidden fields
    private float sellMultiplier;

    private void Awake() 
    {
        GameManager.gameManager.SubscribeToGameStateChanged(GameManager_OnStateChanged);
    }

    private void OnDestroy()
    {
        GameManager.gameManager.UnsubscribeToGameStateChanged(GameManager_OnStateChanged);
    }

    public void PurchaseTurret(int turretID) 
    {
        TurretBlueprint.Model model = turretBlueprint.models[turretID];

        if (GameManager.gameManager.playerStats.credits < model.buildCost)
        {
            Debug.Log("Not enough credits to build!");
            return;
        }

        GameManager.gameManager.playerStats.credits -= model.buildCost;
        BuildManager.buildManager.BuildTurret(model);

    }

    public void UpgradeTurret()
    {
        TurretBlueprint.Model model = turretBlueprint.models[BuildManager.buildManager.CurrentTurret.ID];
        int nextLevel = BuildManager.buildManager.CurrentTurret.level;

        if(nextLevel >= model.upgradeCosts.Length)
        {
            Debug.Log("Upgrade maxed");
            return;
        }

        if (GameManager.gameManager.playerStats.credits < model.upgradeCosts[nextLevel])
        {
            Debug.Log("Not enough credits to upgrade!");
            return;
        }

        GameManager.gameManager.playerStats.credits -= model.upgradeCosts[nextLevel];
        BuildManager.buildManager.UpgradeTurret();
    }

    public void SellTurret()
    {
        TurretBlueprint.Model model = turretBlueprint.models[BuildManager.buildManager.CurrentTurret.ID];
        int currentLevel = BuildManager.buildManager.CurrentTurret.level - 1;
        int totalUpgradeCost = 0;
        for (int i = 1; i <= currentLevel; i++) totalUpgradeCost += model.upgradeCosts[i];
        GameManager.gameManager.playerStats.credits += Mathf.RoundToInt(sellMultiplier * (model.buildCost + totalUpgradeCost));
        BuildManager.buildManager.DemolishTurret();
    }

    private void GameManager_OnStateChanged(GameStateManager.GameState gameState)
    {
        if (gameState == GameStateManager.GameState.Preparing) sellMultiplier = 1f;
        else sellMultiplier = 0.5f;
    }
}
