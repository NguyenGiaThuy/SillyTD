using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
{
    public delegate void OnSelectHandler(Node node);
    public static event OnSelectHandler OnSelect;

    public Turret turret { get; private set; }
    public int totalCost { get; private set; }

    [SerializeField]
    private Color hoverColor;

    // Hidden fields
    private Color initialColor;
    private Renderer nodeRenderer;

    private void Awake()
    {
        BuildManager.OnBuild += BuildManager_OnBuild;
        BuildManager.OnDemolish += BuildManager_OnDemolish;
        BuildManager.OnUpgrade += BuildManager_OnUpgrade;
    }

    private void OnDestroy()
    {
        BuildManager.OnBuild -= BuildManager_OnBuild;
        BuildManager.OnDemolish -= BuildManager_OnDemolish;
        BuildManager.OnUpgrade -= BuildManager_OnUpgrade;
    }

    private void BuildManager_OnBuild(Node node, Turret turret)
    {
        if (node != this) return;

        this.turret = turret;
        for (int levels = 1; levels <= turret.levels; levels++)
            totalCost += BuildManager.turretBlueprints[turret.id].turretParameters[levels - 1].cost;
    }

    private void BuildManager_OnDemolish(Node node)
    {
        if (node != this) return;

        this.turret = null;
        totalCost = 0;
    }

    private void BuildManager_OnUpgrade(Node node)
    {
        if (node != this) return;

        totalCost += BuildManager.turretBlueprints[turret.id].turretParameters[node.turret.levels - 1].cost;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        nodeRenderer.material.color = hoverColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) OnSelect?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        nodeRenderer.material.color = initialColor;
    }
}

