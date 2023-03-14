using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Systems.Grid;

///INFO
///->Usage of BuildingController script: 
///ENDINFO

public class BuildingController : MonoBehaviour, ISlotable
{
    #region Publics
    public Building Building;

    [FoldoutGroup("Data")]
    public int Health;
    [FoldoutGroup("Data")]
    [ReadOnly]
    public bool IsInitialized = false;
    [FoldoutGroup("References")]
    public SpriteRenderer SpriteRenderer;

    [FoldoutGroup("Data")]
    [ReadOnly]
    [ShowInInspector]
    public List<Cell> AllocatedCells;

    #endregion

    #region Privates
    private int localHealth;
    private bool isGameObjectEqual(GameObject target) { return (gameObject.Equals(target)); }
    #endregion

    #region Cached

    #endregion

    #region Events

    #endregion

    #region Monobehaviours
    private void OnEnable()
    {
        PoolingManager.Instance.OnObjectProduced += OnProduced;
        PoolingManager.Instance.OnObjectRecycled += OnRecycle;
        PoolingManager.Instance.OnObjectReturned += OnReturned;
    }

    private void OnDisable()
    {
        if (PoolingManager.Instance)
        {
            PoolingManager.Instance.OnObjectProduced -= OnProduced;
            PoolingManager.Instance.OnObjectRecycled -= OnRecycle;
            PoolingManager.Instance.OnObjectReturned -= OnReturned;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(AllocatedCells != null)
        {
            foreach (Cell cell in AllocatedCells)
            {
                Gizmos.DrawCube(cell.CellPosition, Vector3.one * 0.075f);
            }
        }
    }

    #endregion

    #region Functions

    [Button]
    public void Death()
    {
        ReleaseCells();
        PoolingManager.Instance.ReturnObjectToPool(ProductionType.Building, gameObject);
    }

    private void OnRecycle(GameObject recycledObject)
    {
        if (isGameObjectEqual(recycledObject))
        {
            ResetController();
            InitializeController(BuildingManager.Instance.LastProducedBuilding);
            AllocateCells();
        }
    }

    private void OnReturned(GameObject returnedObject)
    {
        if (isGameObjectEqual(returnedObject))
        {
            ResetController();
        }
    }

    private void OnProduced(GameObject reproducedObject)
    {
        if (isGameObjectEqual(reproducedObject))
        {
            InitializeController(BuildingManager.Instance.LastProducedBuilding);
            AllocateCells();
        }
    }

    public void AllocateCells()
    {
        AllocatedCells = new List<Systems.Grid.Cell>();

        Cell searchingCell = GridManager.Instance.Grid.GetCellByPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        //searchingCell.SlottedObject = this;
        //AllocatedCells.Add(searchingCell);


        for (int i = 0; i < BuildingManager.Instance.LastProducedBuilding.BuildingSize.x+1; i++)
        {
            for (int j = 0; j < BuildingManager.Instance.LastProducedBuilding.BuildingSize.y-1; j++)
            {
                if (searchingCell.IsEmpty == true)
                {
                    searchingCell.SlottedObject = this;
                    AllocatedCells.Add(searchingCell);
                    searchingCell = GetCellAtTop(searchingCell);
                }
            }
            searchingCell.SlottedObject = this;
            AllocatedCells.Add(searchingCell);
            searchingCell = GridManager.Instance.Grid.GetCellByPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            for (int k = 0; k < i; k++)
            {
                searchingCell = GetCellAtRight(searchingCell);
            }
        }
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


    public void ReleaseCells()
    {
        foreach (Systems.Grid.Cell cell in AllocatedCells)
        {
            cell.SlottedObject = null;
        }
        AllocatedCells = null;
    }

    public void InitializeController(Building building)
    {
        Building = building;
        SpriteRenderer.sprite = building.BuildingImage;
        localHealth = building.Health;
        IsInitialized = true;
    }

    public void ResetController()
    {
        IsInitialized = false;
        Building = null;
        SpriteRenderer.sprite = null;
        localHealth = 0;
    }

    public void GetDamage()
    {
        
    }
    #endregion

}
