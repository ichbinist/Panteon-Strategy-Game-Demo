using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

///INFO
///->Usage of ObjectSelectionPanel script: Handles the Opening and Closing actions of Detailed and Non-Detailed panels when selected objects (Buildings or Units)
///ENDINFO

public class ObjectSelectionPanel : SubBasePanel
{
    #region Publics
    [FoldoutGroup("References")]
    public Image InformationImage;
    [FoldoutGroup("References")]
    public TMPro.TextMeshProUGUI InformationText;

    #endregion

    #region Privates
    private Building localBuilding;
    private Unit localUnit;
    #endregion

    #region Cached

    #endregion

    #region Events

    #endregion

    #region Monobehaviours
    protected override void OnEnable()
    {
        base.OnEnable();
        BuildingManager.Instance.OnBuildingSelected += BuildingSelection;
        UnitManager.Instance.OnUnitSelected += UnitSelection;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if(BuildingManager.Instance)
            BuildingManager.Instance.OnBuildingSelected -= BuildingSelection;
        if(UnitManager.Instance)
            UnitManager.Instance.OnUnitSelected -= UnitSelection;
    }
    #endregion

    #region Functions
    private void BuildingSelection(Building building)
    {
        if(building != null)
        {
            ActivatePanel();
            InformationImage.enabled = true;
            InformationText.enabled = true;
            InformationImage.sprite = building.BuildingImage;
            InformationText.SetText(building.BuildingName);
            localBuilding = building;
        }
        else
        {
            localBuilding = null;

            if (localUnit == null)
            {
                DeactivatePanel();
                InformationImage.enabled = false;
                InformationText.enabled = false;
            }
        }
    }

    private void UnitSelection(Unit unit)
    {
        if (unit != null)
        {
            ActivatePanel();
            InformationImage.enabled = true;
            InformationText.enabled = true;
            InformationImage.sprite = unit.UnitImage;
            InformationText.SetText(unit.UnitName);
            localUnit = unit;
        }
        else
        {
            unit = null;

            if (localBuilding == null)
            {
                DeactivatePanel();
                InformationImage.enabled = false;
                InformationText.enabled = false;
            }
        }
    }
    #endregion

}
