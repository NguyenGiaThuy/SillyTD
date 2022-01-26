using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
{
    public delegate void OnMouseDownHandler(Node node);
    public static event OnMouseDownHandler OnMouseDown;

    public bool Empty { get { return turret == null; } }

    public Turret turret;
    public int sellCredits;
    public int turretID;
    [SerializeField]
    private Color hoverColor;

    // Hidden fields
    private Color initialColor;
    private Renderer nodeRenderer;
    private NodeUI nodeUI;
    private BuildManager buildManager;
    private GameObject informationPanel;
    private Shop shop;

    private void Start()
    {
        shop = FindObjectOfType<Shop>();

        nodeRenderer = GetComponent<Renderer>();
        initialColor = nodeRenderer.material.color;

        buildManager = FindObjectOfType<BuildManager>();
        BuildManager.OnBuilt += BuildManager_OnBuilt;
        BuildManager.OnUpgraded += BuildManager_OnUpgraded;
        BuildManager.OnDemolished += BuildManager_OnDemolished;

        nodeUI = FindObjectOfType<NodeUI>();
        NodeUI.OnModificationPanelShowed += NodeUI_ModificationPanelShowed;
        NodeUI.OnPanelHidden += NodeUI_PanelHidden;

        informationPanel = GameObject.Find("InformationCanvas").transform.GetChild(0).gameObject;

        GameManager.Instance.SubscribeToOnStateChanged(GameManager_OnStateChanged);
    }

    private void GameManager_OnStateChanged(GameStateManager.GameState gameState)
    {
        if (gameState == GameStateManager.GameState.Playing) sellCredits = Mathf.RoundToInt(shop.SellMultiplier * sellCredits);
    }

    private void BuildManager_OnDemolished(Node demolishedNode)
    {
        if (demolishedNode == this) sellCredits = 0;
    }

    private void BuildManager_OnUpgraded(Node upgradedNode)
    {
        if (upgradedNode == this) sellCredits = Mathf.RoundToInt(shop.SellMultiplier * shop.SellCredits);
    }

    private void BuildManager_OnBuilt(Node builtNode)
    {
        if (builtNode == this) sellCredits = Mathf.RoundToInt(shop.SellMultiplier * shop.SellCredits); 
    }

    private void NodeUI_ModificationPanelShowed()
    {
        informationPanel.SetActive(true);
    }

    private void OnDestroy()
    {
        BuildManager.OnBuilt -= BuildManager_OnBuilt;
        BuildManager.OnUpgraded -= BuildManager_OnUpgraded;
        BuildManager.OnDemolished -= BuildManager_OnDemolished;

        NodeUI.OnModificationPanelShowed -= NodeUI_ModificationPanelShowed;
        NodeUI.OnPanelHidden -= NodeUI_PanelHidden;

        GameManager.Instance.UnsubscribeToOnStateChanged(GameManager_OnStateChanged);
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
            informationPanel.SetActive(false);
            nodeUI.ShowPanel(this);
            OnMouseDown?.Invoke(this);
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

