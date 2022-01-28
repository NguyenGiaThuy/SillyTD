using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public delegate void OnBuildHandler(Node node, Turret turret);
    public static event OnBuildHandler OnBuild;
    public delegate void OnDemolishHandler(Node node);
    public static event OnDemolishHandler OnDemolish;
    public delegate void OnUpgradeHandler(Node node);
    public static event OnUpgradeHandler OnUpgrade;

    public static TurretBlueprint[] turretBlueprints = new TurretBlueprint[4];

    public Node node { get; private set; }

    [SerializeField]
    private Vector3 buildOffsets;

    private void Awake()
    {
        turretBlueprints[0] = Resources.Load("StandardTurret/StandardTurretBlueprint") as TurretBlueprint;
        turretBlueprints[1] = Resources.Load("MissileLauncher/MissileLauncherBlueprint") as TurretBlueprint;
        turretBlueprints[2] = Resources.Load("Artillery/ArtilleryBlueprint") as TurretBlueprint;
        turretBlueprints[3] = Resources.Load("SupportTurret/SupportTurretBlueprint") as TurretBlueprint;

        Node.OnSelect += Node_OnSelect;
    }

    private void OnDestroy()
    {
        Node.OnSelect -= Node_OnSelect;
    }

    private void Node_OnSelect(Node node)
    {
        this.node = node;
    }

    public void BuildTurret(TurretBlueprint turretBlueprint, int turretLevels, Node node = null)
    {
        if (node == null) node = this.node;

        GameObject turretGameObject = Instantiate(turretBlueprint.turretPrefab, node.transform.position + buildOffsets, turretBlueprint.turretPrefab.transform.rotation);
        Turret turret = turretGameObject.GetComponent<Turret>();
        turret.IncreaseLevelsTo(turretLevels);
        OnBuild?.Invoke(node, turretGameObject.GetComponent<Turret>());
    }

    public void SellTurret()
    {
        Destroy(node.turret.gameObject);
        OnDemolish?.Invoke(node);
    }

    public void UpgradeTurret(int levels)
    {
        node.turret.IncreaseLevelsTo(levels);
        OnUpgrade?.Invoke(node);
    }
}
