using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    public delegate void OnBuildPanelShowedHandler();
    public event OnBuildPanelShowedHandler BuildPanelShowed;
    public delegate void OnModificationPanelShowedHandler();
    public event OnModificationPanelShowedHandler ModificationPanelShowed;
    public delegate void OnPanelHiddenHandler();
    public event OnPanelHiddenHandler PanelHidden;

    [SerializeField]
    private GameObject canvas;

    // Hidden fields
    private Transform canvasTransform;
    private Node selectedNode;
    private GameObject panel;
    private BuildManager buildManager;

    private void Awake()
    {
        buildManager = FindObjectOfType<BuildManager>();
        buildManager.Built += BuildManager_OnBuilt;
        buildManager.Demolished += BuildManager_OnDemolished;
        buildManager.Upgraded += BuildManager_Upgraded;
    }

    private void Start()
    {
        canvasTransform = canvas.GetComponent<RectTransform>();
    }

    private void OnDestroy()
    {
        buildManager.Built -= BuildManager_OnBuilt;
        buildManager.Demolished -= BuildManager_OnDemolished;
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
            BuildPanelShowed?.Invoke();
        }
        else
        {
            panel = transform.GetChild(0).GetChild(1).gameObject; // Modification panel
            panelPosition = selectedNode.turret.transform.position + new Vector3(0f, 6.5f, 2f);
            ModificationPanelShowed?.Invoke();
        }

        panel.gameObject.SetActive(true);
        canvasTransform.position = panelPosition;
    }

    public void HidePanel()
    {
        if (panel != null)
        {
            panel.gameObject.SetActive(false);
            PanelHidden?.Invoke();
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
