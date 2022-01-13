using UnityEngine;

public class NodeUI : MonoBehaviour
{
    public static NodeUI nodeUI;

    [SerializeField]
    private GameObject canvas;

    // Hidden fields
    private Transform mainCameraTransform;
    private Transform canvasTransform;
    private Node selectedNode;
    private GameObject panel;

    private void Awake()
    {
        if (nodeUI != null) Destroy(gameObject);
        else nodeUI = this;
        BuildManager.buildManager.Built += BuildManager_OnBuilt;
        BuildManager.buildManager.Demolished += BuildManager_OnDemolished;
    }

    private void Start()
    {
        canvasTransform = canvas.GetComponent<RectTransform>();
        mainCameraTransform = GameObject.Find("MainCamera").GetComponent<Transform>();
    }

    private void Update()
    {
        // Rotate NodeUI to follow camera view
        Vector3 cameraDirection = canvasTransform.position - mainCameraTransform.position;
        Quaternion lookRotation = Quaternion.LookRotation(cameraDirection);
        canvasTransform.rotation = Quaternion.Slerp(canvasTransform.rotation, lookRotation, 2f * Time.deltaTime);

        // Hide NodeUI when not selected
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)) HidePanel();
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
        Vector3 panelPosition = selectedNode.transform.position + new Vector3(0f, 3f, 0f);

        // Show NodeUI panel according to node position
        if (selectedNode.Empty) panel = transform.GetChild(0).GetChild(0).gameObject; // Build panel
        else 
        {
            panel = transform.GetChild(0).GetChild(1).gameObject; // Modification panel
            panelPosition = selectedNode.turret.transform.position + new Vector3(0f, 5f, 0f);
        }
       
        canvasTransform.position = panelPosition;
        panel.gameObject.SetActive(true);
    }

    public void HidePanel()
    {
        if(panel != null) panel.gameObject.SetActive(false);
    }

    private void BuildManager_OnBuilt(GameObject builtTurret)
    {
        HidePanel();
    }

    private void BuildManager_OnDemolished(int demolishedTurretID)
    {
        HidePanel();
    }
}
