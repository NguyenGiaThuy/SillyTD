using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InformationMenu : MonoBehaviour
{
    [SerializeField]
    private Image avatar;
    [SerializeField]
    private TMP_Text description;
    [SerializeField]
    private TMP_Text cost;
    [SerializeField]
    private TMP_Text sellCredits;
    [SerializeField]
    private Image damageMaskWhite;
    [SerializeField]
    private Image fireRateMaskWhite;
    [SerializeField]
    private Image rangeMaskWhite;
    [SerializeField]
    private Image damageMaskGreen;
    [SerializeField]
    private Image fireRateMaskGreen;
    [SerializeField]
    private Image rangeMaskGreen;

    [SerializeField]
    private int maxDamage;
    [SerializeField]
    private float maxFireRate;
    [SerializeField]
    private float maxRange;

    private float damageMaskInitialWidth;
    private float fireRateMaskInitialWidth;
    private float rangeMaskInitialWidth;
    private InformationStats informationStats;
    private Node selectedNode;

    private void Start()
    {
        damageMaskInitialWidth = damageMaskWhite.rectTransform.rect.width;
        fireRateMaskInitialWidth = fireRateMaskWhite.rectTransform.rect.width;
        rangeMaskInitialWidth = rangeMaskWhite.rectTransform.rect.width;
        BuildButton.OnHovered += BuildButton_OnHovered;
        Node.OnMouseDown += Node_OnMouseDown;
        GameManager.Instance.SubscribeToOnStateChanged(GameManager_OnStateChanged);
    } 

    private void OnDestroy()
    {
        BuildButton.OnHovered -= BuildButton_OnHovered;
        Node.OnMouseDown -= Node_OnMouseDown;
    }

    private void GameManager_OnStateChanged(GameStateManager.GameState gameState)
    {
        if(gameState == GameStateManager.GameState.Playing && selectedNode != null) sellCredits.text = selectedNode.sellCredits.ToString();
    }

    private void Node_OnMouseDown(Node node)
    {
        if (node.turret != null)
        {
            selectedNode = node;

            switch (node.turretID)
            {
                case 0:
                    informationStats = Resources.Load("UI/StandardTurretInformation") as InformationStats;
                    break;
                case 1:
                    informationStats = Resources.Load("UI/MissileLauncherInformation") as InformationStats;
                    break;
                case 2:
                    informationStats = Resources.Load("UI/ArtilleryInformation") as InformationStats;
                    break;
                case 3:
                    informationStats = Resources.Load("UI/SupportTurretInformation") as InformationStats;
                    break;
            }

            avatar.sprite = informationStats.avatar;
            description.text = informationStats.description;
            sellCredits.text = node.sellCredits.ToString();

            damageMaskWhite.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
                damageMaskInitialWidth * ((float)node.turret.turretStats.damage / maxDamage));
            fireRateMaskWhite.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
                fireRateMaskInitialWidth * (maxFireRate / node.turret.turretStats.fireRate));
            rangeMaskWhite.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
                rangeMaskInitialWidth * (node.turret.turretStats.maxRange / maxRange));

            if (node.turret.level < 4)
            {
                cost.text = informationStats.blueprint.upgradeCosts[node.turret.level].ToString();

                damageMaskGreen.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
                    damageMaskInitialWidth * ((float)informationStats.turretStats[node.turret.level].damage / maxDamage));
                fireRateMaskGreen.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
                    fireRateMaskInitialWidth * (maxFireRate / informationStats.turretStats[node.turret.level].fireRate));
                rangeMaskGreen.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
                    rangeMaskInitialWidth * (informationStats.turretStats[node.turret.level].maxRange / maxRange));
            }
            else cost.text = null;
        }
    }

    private void BuildButton_OnHovered(int buildButtonID)
    {
        switch(buildButtonID)
        {
            case 0:
                informationStats = Resources.Load("UI/StandardTurretInformation") as InformationStats;
                break;
            case 1:
                informationStats = Resources.Load("UI/MissileLauncherInformation") as InformationStats;
                break;
            case 2:
                informationStats = Resources.Load("UI/ArtilleryInformation") as InformationStats;
                break;
            case 3:
                informationStats = Resources.Load("UI/SupportTurretInformation") as InformationStats;
                break;
        }

        avatar.sprite = informationStats.avatar;
        description.text = informationStats.description;
        cost.text = informationStats.blueprint.buildCost.ToString();
        sellCredits.text = cost.text;

        damageMaskWhite.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
            damageMaskInitialWidth * ((float)informationStats.turretStats[0].damage / maxDamage));
        fireRateMaskWhite.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
            fireRateMaskInitialWidth * (maxFireRate / informationStats.turretStats[0].fireRate));
        rangeMaskWhite.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
            rangeMaskInitialWidth * (informationStats.turretStats[0].maxRange / maxRange));

        damageMaskGreen.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
            damageMaskInitialWidth * ((float)informationStats.turretStats[0].damage / maxDamage));
        fireRateMaskGreen.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
            fireRateMaskInitialWidth * (maxFireRate / informationStats.turretStats[0].fireRate));
        rangeMaskGreen.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
            rangeMaskInitialWidth * (informationStats.turretStats[0].maxRange / maxRange));
    }
}
