using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager buildManager;

    public delegate void OnBuiltHandler(GameObject builtTurret);
    public event OnBuiltHandler Built;
    public delegate void OnDemolishedHandler(int demolishedTurretID);
    public event OnDemolishedHandler Demolished;

    public Node selectedNode;
    [SerializeField]
    private Vector3 buildOffsets;

    private void Awake()
    {
        if (buildManager != null) Destroy(gameObject);
        else buildManager = this;
    }

    public void BuildTurret(TurretBlueprint.Model turretToBuild)
    {
        selectedNode.turret = Instantiate(turretToBuild.turretPrefab, selectedNode.transform.position + buildOffsets, turretToBuild.turretPrefab.transform.rotation);
        Built?.Invoke(selectedNode.turret);
    }

    public void DemolishTurret()
    {
        Turret turretToDemolish = selectedNode.turret.GetComponent<Turret>();
        int turretToDemolishID = turretToDemolish.ID;
        Destroy(turretToDemolish.gameObject);
        Demolished?.Invoke(turretToDemolishID);
    }

    public void UpgradeTurret()
    {
        //    GameObject turretToUpgrade = selectedNode.GetTurret();
        //    int lvlUp = turretToUpgrade.GetComponent<Turret>().GetLevel() + 1;
        //    if (lvlUp > 3)
        //    {
        //        // Play some effect here
        //        Debug.Log("Max lvl!!!");
        //        return;
        //    }

        //    GameObject tempTurret = null;
        //    for (int i = 0; i < turretBlueprints.Length; i += 4)
        //    {
        //        if (turretToUpgrade.name.Contains(turretBlueprints[i].name))
        //        {
        //            tempTurret = turretBlueprints[i + lvlUp % 4];
        //            Debug.Log(i + lvlUp % 4);
        //            break;
        //        }
        //    }

        //    try
        //    {
        //        // Don't upgrade if not credit
        //        int turretCost = tempTurret.GetComponent<Turret>().GetCost();
        //        if (PlayerStats.credit < turretCost)
        //        {
        //            // Play some effect here
        //            Debug.Log("Not enough credit!!!");
        //            return;
        //        }

        //        // Destroy old turret and replace with new one
        //        tempTurret.GetComponent<Turret>().CopyTransform(turretToUpgrade.GetComponent<Turret>());
        //        Destroy(turretToUpgrade.gameObject);
        //        turretToUpgrade = tempTurret;

        //        // Build turret
        //        PlayerStats.credit -= turretCost;
        //        turretToUpgrade = Instantiate(turretToUpgrade, turretToUpgrade.transform.position, selectedNode.transform.rotation);
        //        selectedNode.SetTurret(turretToUpgrade);
        //        NodeUI.nodeUI.HideModificationPanel();

        //        // Play some effect here
        //    }
        //    catch (MissingReferenceException)
        //    {
        //        Debug.LogError("Null reference at BuildManager in UpgradeTurret method");
        //    }
    }
}
