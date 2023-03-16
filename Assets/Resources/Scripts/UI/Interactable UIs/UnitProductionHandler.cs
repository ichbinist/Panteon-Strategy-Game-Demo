using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System;

///INFO
///->Usage of UnitProductionHandler script: Handles the selection of which unit will be produced.
///ENDINFO

public class UnitProductionHandler : BaseInteractable
{
    #region Publics
    [FoldoutGroup("Handler Settings")]
    public Unit Unit;
    [FoldoutGroup("Handler Settings")]
    public TMPro.TextMeshProUGUI UnitName;
    [FoldoutGroup("Handler Settings")]
    public Image UnitImage;
    #endregion

    #region Privates

    #endregion

    #region Cached
    private Button interactionButton;
    public Button InteractionButton { get { return (interactionButton == null) ? interactionButton = GetComponent<Button>() : interactionButton; } }
    #endregion

    #region Events

    #endregion

    #region Monobehaviours
    private void OnEnable()
    {
        InteractionButton.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        InteractionButton.onClick.RemoveListener(OnClick);
    }
    #endregion

    #region Functions

    public void InitializeHandler(Unit unit)
    {
        Unit = unit;
        UnitName.SetText(Unit.UnitName);
        UnitImage.sprite = Unit.UnitImage;
    }

    private void OnClick()
    {
        UnitManager.Instance.LastProducedUnit = Unit;
        BuildingController buildingController = BuildingManager.Instance.LastClickedBuildingController;
        Systems.Grid.Cell targetCell = buildingController.GetUnitProductionCell();
        PlaceUnit(targetCell);
    }

    private void PlaceUnit(Systems.Grid.Cell targetCell)
    {
        if (targetCell.IsEmpty)
        {
            GameObject unitGameObject = PoolingManager.Instance.GetObjectFromPool(ProductionType.Unit);
            unitGameObject.transform.position = targetCell.CellPosition;
            UnitManager.Instance.AddUnit(Unit);
        }
        else
        {
            PlaceUnit(targetCell.Neighbors[UnityEngine.Random.Range(0,targetCell.Neighbors.Count-1)]);
        }
    }
    #endregion

}
