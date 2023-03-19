using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Systems.Grid;
///INFO
///->Usage of UnitMovementController script: 
///ENDINFO

public class UnitMovementController : MonoBehaviour
{
    #region Publics
    [FoldoutGroup("Unit Movement Settings")]
    public float UnitSpeed = 5f;
    public UnitGenericController UnitController;

    public bool IsSelected;
    #endregion

    #region Privates
    [ShowInInspector]
    [ReadOnly]
    private List<Cell> path;

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

    private void Update()
    {
        MoveOnPath();
    }
    #endregion

    #region Functions


    private void MoveOnPath()
    {
        Cell currentNode = UnitController.CurrentCell;

        if(path == null)
        {
            ResetPosition(currentNode, true);
            return;
        }
        Cell nextTarget = null;

        if (path.Count > 0)
        {
            nextTarget = path[0];
        }
        else
        {
            path = null;
            ResetPosition(currentNode, true);
            return;
        }

        if (nextTarget == null) 
        {
            ResetPosition(currentNode, true);
            return;
        }

        if (nextTarget.IsEmpty == false) 
        {
            if (targetCell != null && targetCell.IsEmpty)
            {
                ResetPosition(currentNode, true);
                path = FindPath(UnitController.CurrentCell, targetCell);
            }
            else
            {
                ResetPosition(currentNode, true);
            }
            return;
        }

        if (Vector3.Distance(nextTarget.CellPosition, transform.position) < 0.05f)
        {
            path.Remove(nextTarget);
            ResetPosition(nextTarget, true);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, nextTarget.CellPosition, Time.deltaTime * UnitSpeed);
        }
    }

    public void ResetPosition(Cell resetPositionCell,bool resetPosition)
    {
        if(resetPosition)
            transform.position = resetPositionCell.CellPosition;
        UnitController.ReleaseCells();
        UnitController.ReallocateCells();
    }

    public void ClickControl(Vector2 clickPosition, MouseClickType clickType)
    {
        Cell clickedCell = GridManager.Instance.Grid.GetCellByPosition(Camera.main.ScreenToWorldPoint(clickPosition));

        if(clickedCell == null)
        {
            IsSelected = false;
            UnitManager.Instance.LastClickedUnit = null;
            return;
        }
        if (clickType == MouseClickType.Left)
        {
            //Select Command
            if (clickedCell.IsEmpty == false && clickedCell == UnitController.CurrentCell)
            {
                IsSelected = true;
            }
            else if(clickedCell.IsEmpty == false)
            {
                IsSelected = false;
            }
        }
        else if(clickType == MouseClickType.Right)
        {
            //Move Command
            if (IsSelected && UnitManager.Instance.LastClickedUnit == UnitController)
            {
                targetCell = GridManager.Instance.Grid.GetCellByPosition(Camera.main.ScreenToWorldPoint(clickPosition));
                if (targetCell != null && targetCell.IsEmpty)
                {
                    ResetPosition(UnitController.CurrentCell, false);
                    path = FindPath(UnitManager.Instance.LastClickedUnit.CurrentCell, targetCell);
                }
            }
        }
    }

    public List<Cell> FindPath(Cell startCell, Cell goalCell)
    {
        List<Cell> openSet = new List<Cell>();
        HashSet<Cell> closedSet = new HashSet<Cell>();
        Dictionary<Cell, Cell> cameFrom = new Dictionary<Cell, Cell>();
        Dictionary<Cell, float> gScore = new Dictionary<Cell, float>();
        Dictionary<Cell, float> fScore = new Dictionary<Cell, float>();

        gScore[startCell] = 0f;
        fScore[startCell] = CostEstimate(startCell, goalCell);
        openSet.Add(startCell);

        while (openSet.Count > 0)
        {
            Cell current = GetLowestFScoreCell(openSet, fScore);
            if (current == goalCell)
            {
                return ReconstructPath(cameFrom, current, startCell);
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (Cell neighbor in current.Neighbors)
            {
                if (closedSet.Contains(neighbor))
                {
                    continue;
                }

                float tentativeGScore = gScore[current] + CostEstimate(current, neighbor);
                if (!openSet.Contains(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = tentativeGScore + CostEstimate(neighbor, goalCell);

                    if (!openSet.Contains(neighbor) && neighbor.IsEmpty)
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        return null;
    }

    private float CostEstimate(Cell fromCell, Cell toCell)
    {
        return Vector3.Distance(fromCell.CellPosition, toCell.CellPosition);
    }

    private List<Cell> ReconstructPath(Dictionary<Cell, Cell> cameFrom, Cell current, Cell startCell)
    {
        List<Cell> totalPath = new List<Cell>();
        totalPath.Add(current);

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];

            if(current != startCell)
                totalPath.Insert(0, current);
        }

        return totalPath;
    }

    private Cell GetLowestFScoreCell(List<Cell> openSet, Dictionary<Cell, float> fScore)
    {
        Cell lowestCell = openSet[0];
        float lowestScore = fScore[lowestCell];

        for (int i = 1; i < openSet.Count; i++)
        {
            Cell cell = openSet[i];
            if (fScore[cell] < lowestScore)
            {
                lowestCell = cell;
                lowestScore = fScore[cell];
            }
        }

        return lowestCell;
    }
    #endregion
}
