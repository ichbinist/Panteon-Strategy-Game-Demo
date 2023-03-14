using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Systems.Grid;

///INFO
///->Usage of WorldObjectMouseFollow script: 
///ENDINFO

public class WorldObjectMouseFollow : MonoBehaviour
{
    #region Publics
    public Cell TargetCell;
    public float UpdateDensity = 0.05f;

    public Color CorrectPlacementColor, FalsePlacementColor;
    #endregion

    #region Privates
    private float timer = 0;
    #endregion

    #region Cached

    #endregion

    #region Events

    #endregion

    #region Monobehaviours
    private void Update()
    {
        timer += Time.fixedDeltaTime;
        if(timer> UpdateDensity)
        {
            timer = 0;
            TargetCell = GridManager.Instance.Grid.GetCellByPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        if (TargetCell != null)
        {
            transform.position = TargetCell.CellPosition;
        }
    }
    #endregion

    #region Functions

    #endregion
}
