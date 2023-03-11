using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

///INFO
///->Usage of Cell script: 
///ENDINFO
namespace Systems.Grid
{
    public class Cell
    {
        public List<Cell> Neighbors = new List<Cell>();
        public Vector2 CellPosition;
        public float CellSize = 1f;

        public ISlotable SlottedObject;
        public float StraightPathLength;
        public float DiagonalPathLength; //Equivelant of Square root 2.

        public bool IsEmpty { get { return (SlottedObject == null) ? true : false; } }

        public void InitializeCell()
        {
            StraightPathLength = UnitConversions.UnityLengthToPixel(CellSize);
            DiagonalPathLength = UnitConversions.UnityLengthToPixel(Mathf.Sqrt(Mathf.Pow(CellSize,2) + Mathf.Pow(CellSize, 2)));
        }

        public void InitializeNeighbors(List<Cell> allCells)
        {
            InitializeCell();
            Neighbors = allCells.FindAll(x => Vector2.Distance(x.CellPosition, CellPosition) <= DiagonalPathLength).ToList();
        }

        public float CalculateDistanceCostToTarget(Cell targetCell)
        {
            float xDistance = Mathf.Abs(UnitConversions.UnityPositionToPixel(CellPosition).x - UnitConversions.UnityPositionToPixel(targetCell.CellPosition).x);
            float yDistance = Mathf.Abs(UnitConversions.UnityPositionToPixel(CellPosition).y - UnitConversions.UnityPositionToPixel(targetCell.CellPosition).y);
            float remainingDistance = Mathf.Abs(xDistance - yDistance);
            return DiagonalPathLength * Mathf.Min(xDistance, yDistance) + StraightPathLength * remainingDistance;
        }
    }
}