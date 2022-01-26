using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    public delegate void OnBuildPanelShowedHandler();
    public static event OnBuildPanelShowedHandler OnBuildPanelShowed;
    public delegate void OnModificationPanelShowedHandler();
    public static event OnModificationPanelShowedHandler OnModificationPanelShowed;
    public delegate void OnPanelHiddenHandler();
    public static event OnPanelHiddenHandler OnPanelHidden;

    [SerializeField]
    private GameObject canvas;

    // Hidden fields
    private Transform canvasTransform;
    private Node selectedNode;
    private GameObject panel;

    private void Awake()
    {
        BuildManager.OnBuilt += BuildManager_OnBuilt;
        BuildManager.OnDemolished += BuildManager_OnDemolished;
        BuildManager.OnUpgraded += BuildManager_Upgraded;
    }

    private void Start()
    {
        canvasTransform = canvas.GetComponent<RectTransform>();
    }

    private void OnDestroy()
    {
        BuildManager.OnBuilt -= BuildManager_OnBuilt;
        BuildManager.OnDemolished -= BuildManager_OnDemolished;
    }

    public void ShowPanel(Node nodeToSelect)
    {
        HidePanel();      
        selectedNode = nodeToSelect;
        Vector3 panelPosition = selectedNode.transform.position + new Vector3(0f, 5f, 0f);

        // Show NodeUI panel according to node position
        if (selectedNode.Empty)
        {
            panel = transform.GetChild(0).GetChild(0).gameObject; // Build panel
            OnBuildPanelShowed?.Invoke();
        }
        else
        {
            panel = transform.GetChild(0).GetChild(1).gameObject; // Modification panel
            panelPosition = selectedNode.turret.transform.position + new Vector3(0f, 6.5f, 2f);
            OnModificationPanelShowed?.Invoke();
        }

        panel.gameObject.SetActive(true);
        canvasTransform.position = panelPosition;
    }

    public void HidePanel()
    {
        if (panel != null)
        {
            panel.gameObject.SetActive(false);
            OnPanelHidden?.Invoke();
        }
    }

    private void BuildManager_OnBuilt(Node builtNode)
    {
        HidePanel();
    }

    private void BuildManager_OnDemolished(Node demolishedNode)
    {
        HidePanel();
    }

    private void BuildManager_Upgraded(Node upgradedNode)
    {
        HidePanel();
    }
}
