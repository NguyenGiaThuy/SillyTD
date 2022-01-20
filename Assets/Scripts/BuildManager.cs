using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager buildManager;

    public delegate void OnBuiltHandler(GameObject builtTurret);
    public event OnBuiltHandler Built;
    public delegate void OnDemolishedHandler(Vector3 demolishedTurretPosition);
    public event OnDemolishedHandler Demolished;
    public delegate void OnUpgradedHandler(GameObject upgradedTurret);
    public event OnUpgradedHandler Upgraded;

    public Turret CurrentTurret { get { return selectedNode.turret.GetComponent<Turret>(); } }

    public Node selectedNode;
    [SerializeField]
    private Vector3 buildOffsets;

    private void Awake()
    {
        if (buildManager != null) Destroy(gameObject);
        else
        {
            buildManager = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void BuildTurret(TurretBlueprint.Model turretToBuild)
    {
        selectedNode.turret = Instantiate(turretToBuild.turretPrefab, selectedNode.transform.position + buildOffsets, turretToBuild.turretPrefab.transform.rotation);
        selectedNode.turret.transform.parent = selectedNode.transform;
        Built?.Invoke(selectedNode.turret);
    }

    public void DemolishTurret()
    {
        Vector3 turretToDemolishPosition = selectedNode.turret.transform.position;
        Destroy(selectedNode.turret);
        Demolished?.Invoke(turretToDemolishPosition);
    }

    public void UpgradeTurret()
    {
        Turret turretToUpgrade = selectedNode.turret.GetComponent<Turret>();
        turretToUpgrade.IncreaseLevel();
        Upgraded?.Invoke(selectedNode.turret);
    }
}
