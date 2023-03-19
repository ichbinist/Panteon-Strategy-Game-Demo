using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

///INFO
///->Usage of NonDetailedPanel script: Handles the Changing data on UI when selected any non detailed object (Power Plant)
///ENDINFO

public class NonDetailedPanel : SubBasePanel
{
    #region Publics

    #endregion

    #region Privates

    #endregion

    #region Cached

    #endregion

    #region Events

    #endregion

    #region Monobehaviours
    protected override void OnEnable()
    {
        base.OnEnable();
        BuildingManager.Instance.OnBuildingSelected += InitializePanel;
        if (BuildingManager.Instance.LastClickedBuildingController != null && !BuildingManager.Instance.LastClickedBuildingController.Building.isDetailedInfoBuilding)
        {
            InitializePanel(BuildingManager.Instance.LastClickedBuildingController.Building);
        }
        UnitManager.Instance.OnUnitSelected += UnitSelectionHandle;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if(BuildingManager.Instance)
            BuildingManager.Instance.OnBuildingSelected -= InitializePanel;
        if (UnitManager.Instance)
        {
            UnitManager.Instance.OnUnitSelected -= UnitSelectionHandle;
        }
    }
    #endregion

    #region Functions
    private void UnitSelectionHandle(Unit unit)
    {
        DeactivatePanel();
    }

    private void InitializePanel(Building building)
    {
        if(building == null)
        {
            DeactivatePanel();
            return;
        }
        if (building.BuildingInfoType == BuildingInfoType.Non_Detailed)
        {
            ActivatePanel();
        }
        else
        {
            DeactivatePanel();
        }

    }
    #endregion
}
