using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

///INFO
///->Usage of Grid script: 
///ENDINFO
namespace Systems.Grid
{
    [System.Serializable]
    public class Grid
    {
        public int GridSizeX;
        public int GridSizeY;
        public float CellSize;
        [ShowInInspector]
        [ReadOnly]
        public List<Cell> grid = new List<Cell>();
    
        public void GenerateGrid()
        {
            for (int y = 0; y < GridSizeY; y++)
            {
                for (int x = 0; x < GridSizeX; x++)
                {
                    Vector2 cellPosition = new Vector2(UnitConversions.UnityLengthToPixel(x * CellSize), UnitConversions.UnityLengthToPixel(y * CellSize));
                    Cell cell = new Cell();
                    cell.CellPosition = cellPosition;
                    cell.CellSize = CellSize;
                    grid.Add(cell);
                }
            }

            foreach (Cell cell in grid)
            {
                cell.InitializeNeighbors(grid);
            }
        }
    }
}