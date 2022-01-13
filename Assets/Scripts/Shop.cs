using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public static Shop turretShop;

    public TurretBlueprint turretBlueprint;

    // Hidden fields
    private float sellMultiplier;

    private void Awake() 
    {
        if (turretShop != null) Destroy(gameObject);
        else turretShop = this;

        BuildManager.buildManager.Built += BuilManager_OnBuilt;
        BuildManager.buildManager.Demolished += BuildManager_OnDemolished;
        GameManager.gameManager.StateChanged += GameManager_OnStateChanged;
    }

    private void OnDestroy()
    {
        BuildManager.buildManager.Built -= BuilManager_OnBuilt;
        BuildManager.buildManager.Demolished -= BuildManager_OnDemolished;
        GameManager.gameManager.StateChanged -= GameManager_OnStateChanged;
    }

    public void PurchaseTurret(int turretID) 
    {
        if(GameManager.gameManager.playerStats.credits < turretBlueprint.models[turretID].cost)
        {
            Debug.Log("Not enough credits!");
            return;
        }

        BuildManager.buildManager.BuildTurret(turretBlueprint.models[turretID]);

    }

    public void SellTurret()
    {
        BuildManager.buildManager.DemolishTurret();
    }

    private void BuilManager_OnBuilt(GameObject builtTurret)
    {
        TurretBlueprint.Model model = turretBlueprint.models[builtTurret.GetComponent<Turret>().ID];
        GameManager.gameManager.playerStats.credits -= model.cost;
        // Play sound and effect here
    }

    private void BuildManager_OnUpgraded()
    {
        //TurretBlueprint.Model model = turretBlueprint.models[e.ModelID];
        //Debug.Log(model.cost);
    }

    private void BuildManager_OnDemolished(int demolishedTurretID) 
    {
        TurretBlueprint.Model model = turretBlueprint.models[demolishedTurretID];
        GameManager.gameManager.playerStats.credits += Mathf.RoundToInt(sellMultiplier * model.cost);
        // Play sound and effect here
    }

    private void GameManager_OnStateChanged(GameManager.GameState gameState)
    {
        if (gameState == GameManager.GameState.Preparing) sellMultiplier = 1f;
        else sellMultiplier = 0.5f;
    }
}
