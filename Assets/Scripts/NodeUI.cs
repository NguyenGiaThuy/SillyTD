using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    [SerializeField]
    private GameObject canvas;

    // Hidden fields
    private Transform canvasTransform;
    private Node selectedNode;
    private GameObject panel;

    private void Awake()
    {
        BuildManager.buildManager.Built += BuildManager_OnBuilt;
        BuildManager.buildManager.Demolished += BuildManager_OnDemolished;
        BuildManager.buildManager.Upgraded += BuildManager_Upgraded;
    }

    private void Start()
    {
        canvasTransform = canvas.GetComponent<RectTransform>();
    }

    private void OnDestroy()
    {
        BuildManager.buildManager.Built -= BuildManager_OnBuilt;
        BuildManager.buildManager.Demolished -= BuildManager_OnDemolished;
    }

    public void ShowPanel(Node nodeToSelect)
    {
        HidePanel();      
        selectedNode = nodeToSelect;
        Vector3 panelPosition = selectedNode.transform.position + new Vector3(0f, 5f, 0f);

        // Show NodeUI panel according to node position
        if (selectedNode.Empty) panel = transform.GetChild(0).GetChild(0).gameObject; // Build panel
        else 
        {
            panel = transform.GetChild(0).GetChild(1).gameObject; // Modification panel
            panelPosition = selectedNode.turret.transform.position + new Vector3(0f, 6.5f, 2f);
        }

        panel.gameObject.SetActive(true);
        canvasTransform.position = panelPosition;
    }

    public void HidePanel()
    {
        if(panel != null) panel.gameObject.SetActive(false);
    }

    private void BuildManager_OnBuilt(GameObject builtTurret)
    {
        HidePanel();
    }

    private void BuildManager_OnDemolished(Vector3 demolishedTurretPosition)
    {
        HidePanel();
    }

    private void BuildManager_Upgraded(GameObject upgradedTurret)
    {
        HidePanel();
    }
}
