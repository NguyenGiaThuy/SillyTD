using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static TurretBlueprint[] turretBlueprints;

    public delegate void OnBuiltHandler(Node builtNode);
    public static event OnBuiltHandler OnBuilt;
    public delegate void OnDemolishedHandler(Node demolishedNode);
    public static event OnDemolishedHandler OnDemolished;
    public delegate void OnUpgradedHandler(Node upgradedNode);
    public static event OnUpgradedHandler OnUpgraded;

    public Turret CurrentTurret { get { return selectedNode.turret.GetComponent<Turret>(); } }

    public Node selectedNode;
    [SerializeField]
    private Vector3 buildOffsets;

    public static void InitializeTurretBlueprint()
    {
        turretBlueprints = new TurretBlueprint[4];
        turretBlueprints[0] = Resources.Load("StandardTurret/StandardTurretBlueprint") as TurretBlueprint;
        turretBlueprints[1] = Resources.Load("MissileLauncher/MissileLauncherBlueprint") as TurretBlueprint;
        turretBlueprints[2] = Resources.Load("Artillery/ArtilleryBlueprint") as TurretBlueprint;
        turretBlueprints[3] = Resources.Load("SupportTurret/SupportTurretBlueprint") as TurretBlueprint;
    }

    public void BuildTurret(TurretBlueprint turretToBuild)
    {
        GameObject turret = Instantiate(turretToBuild.turretPrefab, selectedNode.transform.position + buildOffsets, turretToBuild.turretPrefab.transform.rotation);
        selectedNode.SetTurret(turret.GetComponent<Turret>());
        OnBuilt?.Invoke(selectedNode);
    }

    public void DemolishTurret()
    {
        Destroy(selectedNode.turret.gameObject);
        OnDemolished?.Invoke(selectedNode);
    }

    public void UpgradeTurret()
    {
        selectedNode.turret.IncreaseLevel();
        OnUpgraded?.Invoke(selectedNode);
    }
}
