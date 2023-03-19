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

    [FoldoutGroup("Building Settings")]
    [SerializeField]
    public Vector2 ProductionLocation;
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

        ClickManager.Instance.OnClickWorld.AddListener(BuildingSelection);
    }

    private void OnDisable()
    {
        if (PoolingManager.Instance)
        {
            PoolingManager.Instance.OnObjectProduced -= OnProduced;
            PoolingManager.Instance.OnObjectRecycled -= OnRecycle;
            PoolingManager.Instance.OnObjectReturned -= OnReturned;

            ClickManager.Instance.OnClickWorld.RemoveListener(BuildingSelection);
        }
    }
    #endregion

    #region Functions

    public Cell GetUnitProductionCell()
    {
        return GridManager.Instance.Grid.GetCellByPosition(AllocatedCells[0].CellPosition + ProductionLocation);
    }

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

    public void GetDamage(int damage)
    {
        localHealth -= damage;

        if (localHealth <= 0)
        {
            Death();
        }
    }

    public void BuildingSelection(Vector2 clickPosition, MouseClickType mouseClickType)
    {
        if (mouseClickType == MouseClickType.Left)
        {
            Cell clickedCell = GridManager.Instance.Grid.GetCellByPosition(Camera.main.ScreenToWorldPoint(clickPosition));
            if(clickedCell == null)
            {
                BuildingManager.Instance.LastClickedBuildingController = null;
                BuildingManager.Instance.OnBuildingSelected.Invoke(null);
            }
            else if (AllocatedCells.Contains(clickedCell))
            {
                BuildingManager.Instance.LastClickedBuildingController = this;
                BuildingManager.Instance.OnBuildingSelected.Invoke(Building);
            }
        }
    }
    #endregion

}
