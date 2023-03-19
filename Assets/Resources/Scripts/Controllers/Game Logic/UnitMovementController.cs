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
    public UnitController UnitController;
    #endregion

    #region Privates
    [ShowInInspector]
    [ReadOnly]
    private List<Cell> path;

    [ReadOnly]
    private Cell targetCell;

    private bool isSelected;
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
            isSelected = false;
            UnitManager.Instance.LastClickedUnit = null;
            return;
        }

        if(clickedCell.IsEmpty == false && clickedCell == UnitController.CurrentCell)
        {
            isSelected = true;
        }
        else if(clickedCell.IsEmpty == false)
        {
            isSelected = false;
        }

        if (isSelected && UnitManager.Instance.LastClickedUnit == UnitController)
        {
            if (clickType == MouseClickType.Left)
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

    public List<Cell> FindPath(Cell startNode, Cell goalNode)
    {
        List<Cell> openSet = new List<Cell>();
        HashSet<Cell> closedSet = new HashSet<Cell>();
        Dictionary<Cell, Cell> cameFrom = new Dictionary<Cell, Cell>();
        Dictionary<Cell, float> gScore = new Dictionary<Cell, float>();
        Dictionary<Cell, float> fScore = new Dictionary<Cell, float>();

        gScore[startNode] = 0f;
        fScore[startNode] = HeuristicCostEstimate(startNode, goalNode);
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Cell current = GetLowestFScoreNode(openSet, fScore);
            if (current == goalNode)
            {
                return ReconstructPath(cameFrom, current, startNode);
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
                    fScore[neighbor] = tentativeGScore + HeuristicCostEstimate(neighbor, goalNode);

                    if (!openSet.Contains(neighbor) && neighbor.IsEmpty)
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        return null;
    }

    private float HeuristicCostEstimate(Cell fromNode, Cell toNode)
    {
        return Vector3.Distance(fromNode.CellPosition, toNode.CellPosition);
    }

    private float CostEstimate(Cell fromNode, Cell toNode)
    {
        return Vector3.Distance(fromNode.CellPosition, toNode.CellPosition);
    }

    private List<Cell> ReconstructPath(Dictionary<Cell, Cell> cameFrom, Cell current, Cell startNode)
    {
        List<Cell> totalPath = new List<Cell>();
        totalPath.Add(current);

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];

            if(current != startNode)
                totalPath.Insert(0, current);
        }

        return totalPath;
    }

    private Cell GetLowestFScoreNode(List<Cell> openSet, Dictionary<Cell, float> fScore)
    {
        Cell lowestNode = openSet[0];
        float lowestScore = fScore[lowestNode];

        for (int i = 1; i < openSet.Count; i++)
        {
            Cell node = openSet[i];
            if (fScore[node] < lowestScore)
            {
                lowestNode = node;
                lowestScore = fScore[node];
            }
        }

        return lowestNode;
    }
    #endregion
}
