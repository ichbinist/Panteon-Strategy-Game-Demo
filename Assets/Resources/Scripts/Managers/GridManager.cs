using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Systems.Grid;
///INFO
///->Usage of GridManager script: Handles the Communication between Building Placement Controllers, Unit Controllers and UI.
///ENDINFO

public class GridManager : Singleton<GridManager>
{
    #region Publics
    [SerializeField]
    public Systems.Grid.Grid Grid;
    public GridRenderer GridRenderer;
    #endregion

    #region Privates

    #endregion

    #region Cached

    #endregion

    #region Events

    #endregion

    #region Monobehaviours
    private void Start()
    {
        CreateGrid();
        GridRenderer.DrawGrid(Grid.grid);
    }
    #endregion

    #region Functions
    public void CreateGrid()
    {
        Grid.GenerateGrid();
    }
    #endregion
}
