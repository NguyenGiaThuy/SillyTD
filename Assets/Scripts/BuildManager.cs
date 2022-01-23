using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public delegate void OnBuiltHandler(Node builtNode);
    public event OnBuiltHandler Built;
    public delegate void OnDemolishedHandler(Node demolishedNode);
    public event OnDemolishedHandler Demolished;
    public delegate void OnUpgradedHandler(Node upgradedNode);
    public event OnUpgradedHandler Upgraded;

    public Turret CurrentTurret { get { return selectedNode.turret.GetComponent<Turret>(); } }

    public Node selectedNode;
    [SerializeField]
    private Vector3 buildOffsets;

    public void BuildTurret(TurretBlueprint.Model turretToBuild)
    {
        GameObject turret = Instantiate(turretToBuild.turretPrefab, selectedNode.transform.position + buildOffsets, turretToBuild.turretPrefab.transform.rotation);
        selectedNode.SetTurret(turret.GetComponent<Turret>());
        Built?.Invoke(selectedNode);
    }

    public void DemolishTurret()
    {
        Destroy(selectedNode.turret.gameObject);
        Demolished?.Invoke(selectedNode);
    }

    public void UpgradeTurret()
    {
        selectedNode.turret.IncreaseLevel();
        Upgraded?.Invoke(selectedNode);
    }
}
