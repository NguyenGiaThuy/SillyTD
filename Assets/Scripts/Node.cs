using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
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

    private void OnMouseEnter()
    {
        // Prevent highlighting ingame objects over UI
        if (EventSystem.current.IsPointerOverGameObject()) return;

        nodeRenderer.material.color = hoverColor;
    }

    private void OnMouseExit()
    {
        nodeRenderer.material.color = initialColor;
    }

    private void OnMouseDown()
    {
        // Prevent pressing ingame objects over UI
        if (EventSystem.current.IsPointerOverGameObject()) return;

        BuildManager.buildManager.selectedNode = this;
        NodeUI.nodeUI.ShowPanel(this);
    }
}

