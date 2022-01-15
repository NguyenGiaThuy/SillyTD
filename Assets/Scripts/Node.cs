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

    private void Start()
    {
        nodeRenderer = GetComponent<Renderer>();
        initialColor = nodeRenderer.material.color;
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
            NodeUI.nodeUI.ShowPanel(this);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        nodeRenderer.material.color = initialColor;
    }
}

