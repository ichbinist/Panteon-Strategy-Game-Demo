using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Systems.Grid;

///INFO
///->Usage of UnitAttackController script: 
///ENDINFO

public class UnitAttackController : MonoBehaviour
{
    #region Publics
    public UnitMovementController UnitMovementController;
    public float AttackRateAsSeconds = 0.5f;
    #endregion

    #region Privates
    [ReadOnly]
    private Cell targetCell;
    #endregion

    #region Cached

    #endregion

    #region Events

    #endregion

    #region Monobehaviours
    private void OnEnable()
    {
        ClickManager.Instance.OnClickWorld.AddListener(ClickControl);
    }

    private void OnDisable()
    {
        if (ClickManager.Instance)
            ClickManager.Instance.OnClickWorld.RemoveListener(ClickControl);
    }

    #endregion

    #region Functions
    public void ClickControl(Vector2 clickPosition, MouseClickType clickType)
    {
        Cell clickedCell = GridManager.Instance.Grid.GetCellByPosition(Camera.main.ScreenToWorldPoint(clickPosition));

        if (clickType == MouseClickType.Right)
        {
            if (UnitMovementController.IsSelected && UnitManager.Instance.LastClickedUnit == UnitMovementController.UnitController)
            {
                targetCell = clickedCell;
                if (targetCell != null && !targetCell.IsEmpty)
                {
                    if (UnitMovementController.UnitController.CurrentCell.Neighbors.Contains(targetCell))
                    {
                        StartCoroutine(AttackSequenceCoroutine());
                    }
                }
            }
        }
    }

    private IEnumerator AttackSequenceCoroutine()
    {
        if(UnitMovementController.UnitController.CurrentCell.Neighbors.Contains(targetCell) == false)
        {
            yield break;
        }

        while (UnitMovementController.UnitController.CurrentCell.Neighbors.Contains(targetCell) == true)
        {
            Debug.Log("Attacked Character Name: " + targetCell.SlottedObject + "\n" + "Damage: " + UnitMovementController.UnitController.Unit.Damage);

            if (targetCell.IsEmpty == false)
                targetCell.SlottedObject.GetDamage(UnitMovementController.UnitController.Unit.Damage);
            else
                yield break;

            yield return new WaitForSeconds(AttackRateAsSeconds);
        }

        if (UnitMovementController.UnitController.CurrentCell.Neighbors.Contains(targetCell) == false)
        {
            yield break;
        }
    }
    #endregion

}
