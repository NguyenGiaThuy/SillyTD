using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
{
    public bool Empty { get { return turret == null; } }

    public Turret turret;
    public int turretID;
    [SerializeField]
    private Color hoverColor;

    // Hidden fields
    private Color initialColor;
    private Renderer nodeRenderer;
    private NodeUI nodeUI;
    private BuildManager buildManager;
    private GameObject informationPanel;

    private void Start()
    {
        buildManager = FindObjectOfType<BuildManager>();
        nodeRenderer = GetComponent<Renderer>();
        initialColor = nodeRenderer.material.color;
        nodeUI = FindObjectOfType<NodeUI>();
        nodeUI.ModificationPanelShowed += NodeUI_ModificationPanelShowed;
        nodeUI.PanelHidden += NodeUI_PanelHidden;
        informationPanel = GameObject.Find("InformationCanvas").transform.GetChild(0).gameObject;
    }

    private void NodeUI_ModificationPanelShowed()
    {
        informationPanel.SetActive(true);
    }

    private void OnDestroy()
    {
        nodeUI.ModificationPanelShowed -= NodeUI_ModificationPanelShowed;
        nodeUI.PanelHidden -= NodeUI_PanelHidden;
    }

    private void NodeUI_PanelHidden()
    {
        informationPanel.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        nodeRenderer.material.color = hoverColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            buildManager.selectedNode = this;
            nodeUI.ShowPanel(this);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        nodeRenderer.material.color = initialColor;
    }

    public void SetTurret(Turret turret)
    {
        this.turret = turret;
        turret.transform.parent = transform;
        turretID = turret.ID;
    }
}

