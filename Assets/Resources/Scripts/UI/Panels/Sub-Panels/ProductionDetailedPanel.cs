using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

///INFO
///->Usage of ProductionDetailedPanel script: Handles the Changing data on UI when selected any detailed object (Barracks)
///ENDINFO

public class ProductionDetailedPanel : SubBasePanel
{
    #region Publics
    [FoldoutGroup("Production Handlers")]
    [ReadOnly]
    public List<UnitProductionHandler> UnitProductionHandlers = new List<UnitProductionHandler>();
    [FoldoutGroup("References")]
    public UnitProductionHandler UnitProductionHandlerPrefab;
    [FoldoutGroup("References")]
    public Transform LayoutGrid;
    #endregion

    #region Privates
    private Building localBuilding;
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
        if (BuildingManager.Instance.LastClickedBuildingController != null && BuildingManager.Instance.LastClickedBuildingController.Building.isDetailedInfoBuilding)
        {
            InitializePanel(BuildingManager.Instance.LastClickedBuildingController.Building);
        }

        UnitManager.Instance.OnUnitSelected += UnitSelectionHandle;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (BuildingManager.Instance)
            BuildingManager.Instance.OnBuildingSelected -= InitializePanel;

        if(UnitManager.Instance)
            UnitManager.Instance.OnUnitSelected -= UnitSelectionHandle;
    }
    #endregion

    #region Functions
    private void UnitSelectionHandle(Unit unit)
    {
        DeactivatePanel();
    }

    private void InitializePanel(Building building)
    {
        if (building == null)
        {
            DeactivatePanel();
            return;
        }

        if (building.BuildingInfoType == BuildingInfoType.Detailed)
        {
            ActivatePanel();

            localBuilding = building;

            foreach (UnitProductionHandler child in UnitProductionHandlers)
            {
                Destroy(child.gameObject);
            }

            UnitProductionHandlers.Clear();

            for (int i = 0; i < localBuilding.ProduceableUnits.Count; i++)
            {
                UnitProductionHandler localHandler = Instantiate(UnitProductionHandlerPrefab, LayoutGrid);
                localHandler.InitializeHandler(localBuilding.ProduceableUnits[i]);
                UnitProductionHandlers.Add(localHandler);
            }
        }
        else
        {
            DeactivatePanel();
        }

    }
    #endregion

}
