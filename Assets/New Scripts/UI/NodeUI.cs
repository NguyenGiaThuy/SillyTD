using UnityEngine;

public class NodeUI : MonoBehaviour
{
    public delegate void OnBuildShowHandler();
    public static event OnBuildShowHandler OnBuildShow;
    public delegate void OnModificationShowHandler();
    public static event OnModificationShowHandler OnModificationShow;
    public delegate void OnHideHandler();
    public static event OnHideHandler OnHide;

    // Hidden fields
    private GameObject panel;

    private void Awake()
    {
        Node.OnSelect += Node_OnSelect;
        BuildManager.OnBuild += BuildManager_OnBuild;
        BuildManager.OnDemolish += BuildManager_OnDemolish;
        BuildManager.OnUpgrade += BuildManager_OnUpgrade;
    }

    private void OnDestroy()
    {
        Node.OnSelect -= Node_OnSelect;
        BuildManager.OnBuild -= BuildManager_OnBuild;
        BuildManager.OnDemolish -= BuildManager_OnDemolish;
        BuildManager.OnUpgrade -= BuildManager_OnUpgrade;
    }

    private void Node_OnSelect(Node node)
    {
        HidePanel();
        ShowPanel(node);
    }

    private void BuildManager_OnBuild(Node node, Turret turret)
    {
        HidePanel();
    }

    private void BuildManager_OnDemolish(Node demolishedNode)
    {
        HidePanel();
    }

    private void BuildManager_OnUpgrade(Node upgradedNode)
    {
        HidePanel();
    }

    private void ShowPanel(Node node)
    {
        Vector3 panelPosition = node.transform.position + new Vector3(0f, 5f, 0f);

        if (node.turret == null)
        {
            panel = transform.GetChild(0).GetChild(0).gameObject; // Build panel
            OnBuildShow?.Invoke();
        }
        else
        {
            panel = transform.GetChild(0).GetChild(1).gameObject; // Modification panel
            panelPosition = node.turret.transform.position + new Vector3(0f, 6.5f, 2f);
            OnModificationShow?.Invoke();
        }

        panel.SetActive(true);
        transform.position = panelPosition;
    }

    private void HidePanel()
    {
        if (panel == null) return;

        panel.gameObject.SetActive(false);
        OnHide?.Invoke();
    }
}