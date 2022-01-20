using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
{
    public bool Empty { get { return turret == null; } }

    public GameObject turret;
    [SerializeField]
    private Color hoverColor;

    // Hidden fields
    private Color initialColor;
    private Renderer nodeRenderer;
    private NodeUI nodeUI;

    private void Start()
    {
        nodeRenderer = GetComponent<Renderer>();
        initialColor = nodeRenderer.material.color;
        nodeUI = FindObjectOfType<NodeUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        nodeRenderer.material.color = hoverColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            BuildManager.buildManager.selectedNode = this;
            nodeUI.ShowPanel(this);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        nodeRenderer.material.color = initialColor;
    }
}

