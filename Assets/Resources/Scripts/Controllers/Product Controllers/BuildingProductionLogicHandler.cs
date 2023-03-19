using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Systems.Grid;
///INFO
///->Usage of BuildingProductionLogicHandler script: 
///ENDINFO

public class BuildingProductionLogicHandler : MonoBehaviour
{
    #region Publics
    public SpriteRenderer SpriteRenderer;
    public GameObject BuildingIndicator;
    public WorldObjectMouseFollow WorldObjectMouseFollow;
    #endregion

    #region Privates

    #endregion

    #region Cached

    #endregion

    #region Events

    #endregion

    #region Monobehaviours
    private void OnEnable()
    {
        ClickManager.Instance.OnClickWorld.AddListener(OnClickHandler);
    }

    private void OnDisable()
    {
        if(ClickManager.Instance)
            ClickManager.Instance.OnClickWorld.RemoveListener(OnClickHandler);
    }

    private void Update()
    {
        if(BuildingManager.Instance.LastProducedBuilding != null)
        {
            SpriteRenderer.sprite = BuildingManager.Instance.LastProducedBuilding.BuildingImage;

            if (CheckForAvailibity())
            {
                SpriteRenderer.color = WorldObjectMouseFollow.CorrectPlacementColor;
            }
            else
            {
                SpriteRenderer.color = WorldObjectMouseFollow.FalsePlacementColor;
            }

            BuildingIndicator.SetActive(true);
        }

        if(BuildingManager.Instance.LastProducedBuilding == null)
        {
            SpriteRenderer.sprite = null;
            BuildingIndicator.SetActive(false);
        }
    }
    #endregion

    #region Functions

    public bool CheckForAvailibity()
    {
        Cell searchingCell = WorldObjectMouseFollow.TargetCell;

        if (searchingCell == null|| searchingCell.IsEmpty == false) return false;


        for (int i = 0; i < BuildingManager.Instance.LastProducedBuilding.BuildingSize.x + 1; i++)
        {
            for (int j = 0; j < BuildingManager.Instance.LastProducedBuilding.BuildingSize.y - 1; j++)
            {
                if (searchingCell != null && searchingCell.IsEmpty == true)
                {
                    searchingCell = GetCellAtTop(searchingCell);
                }
                else
                {
                    return false;
                }
            }
            if (searchingCell == null || searchingCell.IsEmpty == false) return false;

            searchingCell = GridManager.Instance.Grid.GetCellByPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            for (int k = 0; k < i; k++)
            {
                searchingCell = GetCellAtRight(searchingCell);
            }
        }
        return true;
    }

    public Cell GetCellAtTop(Cell cell)
    {
        Cell tempCell = cell.Neighbors.Find(x => cell.CellPosition + Vector2.up * UnitConversions.UnityLengthToPixel(x.CellSize) == x.CellPosition);
        return tempCell;
    }

    public Cell GetCellAtRight(Cell cell)
    {
        Cell tempCell = cell.Neighbors.Find(x => cell.CellPosition + Vector2.right * UnitConversions.UnityLengthToPixel(x.CellSize) == x.CellPosition);
        return tempCell;
    }

    private void OnClickHandler(Vector2 clickPosition,MouseClickType mouseClickType)
    {
        if(mouseClickType == MouseClickType.Right)
        {
            ReleaseBuilding();
        }
        else if(BuildingManager.Instance.LastProducedBuilding != null)
        {
            Systems.Grid.Cell cell = GridManager.Instance.Grid.GetCellByPosition(Camera.main.ScreenToWorldPoint(clickPosition));
            if(cell != null && CheckForAvailibity())
                PlaceBuilding(cell);
        }
    }

    private void ReleaseBuilding()
    {
        BuildingManager.Instance.LastProducedBuilding = null;
    }

    private void PlaceBuilding(Systems.Grid.Cell placementCellStart)
    {
        GameObject placementBuilding = PoolingManager.Instance.GetObjectFromPool(ProductionType.Building);
        placementBuilding.transform.position = placementCellStart.CellPosition;
    }
    #endregion

}
