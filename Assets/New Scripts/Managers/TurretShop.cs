using UnityEngine;

public class TurretShop : MonoBehaviour
{
    public float SellMultiplier { get; private set; }

    // Hidden fields
    private int sellCredits;
    BuildManager buildManager;

    private void Start() 
    {
        buildManager = FindObjectOfType<BuildManager>();
    }

    public void PurchaseTurret(int turretID) 
    {
        TurretBlueprint turretBlueprint = BuildManager.turretBlueprints[turretID];

        if (GameManager.Instance.playerStats.credits < turretBlueprint.turretParameters[0].cost)
        {
            Debug.Log("Not enough credits to build!");
            return;
        }

        GameManager.Instance.playerStats.credits -= turretBlueprint.turretParameters[0].cost;
        sellCredits = turretBlueprint.turretParameters[0].cost;
        buildManager.BuildTurret(turretBlueprint, 1);

    }

    public void UpgradeTurret()
    {
        TurretBlueprint turretBlueprint = BuildManager.turretBlueprints[buildManager.node.turret.id];
        int nextLevel = buildManager.node.turret.levels;

        if(nextLevel > turretBlueprint.turretParameters.Length)
        {
            Debug.Log("Upgrade maxed");
            return;
        }

        if (GameManager.Instance.playerStats.credits < turretBlueprint.turretParameters[nextLevel - 1].cost)
        {
            Debug.Log("Not enough credits to upgrade!");
            return;
        }

        GameManager.Instance.playerStats.credits -= turretBlueprint.turretParameters[nextLevel - 1].cost;
        sellCredits += turretBlueprint.turretParameters[nextLevel - 1].cost;
        buildManager.UpgradeTurret(nextLevel);
    }

    public void SellTurret()
    {
        GameManager.Instance.playerStats.credits += Mathf.RoundToInt(SellMultiplier * sellCredits);
        buildManager.SellTurret();
    }
}
