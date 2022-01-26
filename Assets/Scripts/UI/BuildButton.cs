using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.UI;

public class BuildButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public delegate void OnHoveredHandler(int buildButtonID);
    public static OnHoveredHandler OnHovered;

    [SerializeField]
    private GraphicRaycaster raycaster;
    [SerializeField]
    int id;

    private GameObject informationPanel;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;

    private void Start()
    {
        informationPanel = GameObject.Find("InformationCanvas").transform.GetChild(0).gameObject;
        eventSystem = FindObjectOfType<EventSystem>();
        pointerEventData = new PointerEventData(eventSystem);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        informationPanel.SetActive(true);
        OnHovered?.Invoke(id);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        pointerEventData.position = Mouse.current.position.ReadValue();
        raycaster.Raycast(pointerEventData, results);
        if (results.Count > 0) informationPanel.SetActive(true);
        else informationPanel.SetActive(false);
    }
}
