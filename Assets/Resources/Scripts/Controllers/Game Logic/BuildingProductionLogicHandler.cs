using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

///INFO
///->Usage of BuildingProductionLogicHandler script: 
///ENDINFO

public class BuildingProductionLogicHandler : MonoBehaviour
{
    #region Publics
    public ClickLogicHandler ClickLogicHandler;
    public SpriteRenderer SpriteRenderer;
    public GameObject BuildingIndicator;
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
        ClickLogicHandler.OnClickWorld.AddListener(OnClickHandler);
    }

    private void OnDisable()
    {
        ClickLogicHandler.OnClickWorld.RemoveListener(OnClickHandler);
    }

    private void Update()
    {
        if(BuildingManager.Instance.LastProducedBuilding != null && BuildingIndicator.activeSelf == false)
        {
            SpriteRenderer.sprite = BuildingManager.Instance.LastProducedBuilding.BuildingImage;
            BuildingIndicator.SetActive(true);
        }

        if(BuildingManager.Instance.LastProducedBuilding == null && BuildingIndicator.activeSelf == true)
        {
            SpriteRenderer.sprite = null;
            BuildingIndicator.SetActive(false);
        }
    }
    #endregion

    #region Functions
    private void OnClickHandler(Vector2 clickPosition,MouseClickType mouseClickType)
    {
        if(mouseClickType == MouseClickType.Right)
        {
            ReleaseBuilding();
        }
        else if(BuildingManager.Instance.LastProducedBuilding != null)
        {
            PlaceBuilding(GridManager.Instance.Grid.GetCellByPosition(Camera.main.ScreenToWorldPoint(clickPosition)));
        }
    }

    private void ReleaseBuilding()
    {
        BuildingManager.Instance.LastProducedBuilding = null;
    }

    private void PlaceBuilding(Systems.Grid.Cell placementCellStart)
    {
        GameObject placementBuilding = PoolingManager.Instance.GetObjectFromPool(ProductionType.Building);
        placementBuilding.transform.position = placementCellStart.CellPosition; //Geçici
    }
    #endregion

}
