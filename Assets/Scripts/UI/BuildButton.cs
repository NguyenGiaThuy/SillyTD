using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.UI;

public class BuildButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GraphicRaycaster raycaster;

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
