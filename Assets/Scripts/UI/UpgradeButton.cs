using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject informationPanel;

    private void Start()
    {
        informationPanel = GameObject.Find("InformationCanvas").transform.GetChild(0).gameObject;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }
}
