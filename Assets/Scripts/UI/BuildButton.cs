using UnityEngine;
using UnityEngine.EventSystems;

public class BuildButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject informationPanel;

    private void Start()
    {
        informationPanel = GameObject.Find("InformationCanvas").transform.GetChild(0).gameObject;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        informationPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        informationPanel.SetActive(false);
    }
}
