using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Systems.Grid;

///INFO
///->Usage of UnitController script: 
///ENDINFO

public class UnitGenericController : MonoBehaviour, ISlotable
{
    #region Publics
    [FoldoutGroup("Data")]
    public Unit Unit;
    [FoldoutGroup("Data")]
    [ReadOnly]
    public bool IsInitialized = false;
    [FoldoutGroup("Data")]
    [ReadOnly]
    [ShowInInspector]
    public List<Cell> AllocatedCells;
    [FoldoutGroup("Data")]
    [ReadOnly]
    [ShowInInspector]
    public Cell CurrentCell;

    [FoldoutGroup("References")]
    public SpriteRenderer SpriteRenderer;
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

        UnitManager.Instance.OnUnitAdded += AllocateCells;

        ClickManager.Instance.OnClickWorld.AddListener(UnitSelection);
    }

    private void OnDisable()
    {
        if (PoolingManager.Instance)
        {
            PoolingManager.Instance.OnObjectProduced -= OnProduced;
            PoolingManager.Instance.OnObjectRecycled -= OnRecycle;
            PoolingManager.Instance.OnObjectReturned -= OnReturned;
        }

        if (UnitManager.Instance)
        {
            UnitManager.Instance.OnUnitAdded -= AllocateCells;
        }

        if (ClickManager.Instance)
        {
            ClickManager.Instance.OnClickWorld.RemoveListener(UnitSelection);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if(AllocatedCells != null && AllocatedCells.Count != 0)
        {
            Gizmos.DrawSphere(AllocatedCells[0].CellPosition, 0.2f);
        }
    }
    #endregion

    #region Functions

    [Button]
    public void Death()
    {
        PoolingManager.Instance.ReturnObjectToPool(ProductionType.Unit, gameObject);
        UnitManager.Instance.RemoveUnit(Unit);
        ReleaseCells();
    }

    private void OnRecycle(GameObject recycledObject)
    {
        if (isGameObjectEqual(recycledObject))
        {
            ResetController();
            InitializeController(UnitManager.Instance.LastProducedUnit);
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
            InitializeController(UnitManager.Instance.LastProducedUnit);
        }
    }

    public void InitializeController(Unit unit)
    {
        Unit = unit;
        SpriteRenderer.sprite = unit.UnitImage;
        localHealth = unit.Health;
        IsInitialized = true;
    }

    public void ResetController()
    {
        IsInitialized = false;
        Unit = null;
        SpriteRenderer.sprite = null;
        localHealth = 0;
    }

    public void GetDamage(int damage)
    {
        localHealth -= damage;

        if(localHealth <= 0)
        {
            Death();
        }
    }

    private void AllocateCells(UnitAffinityType unitAffinityType)
    {
        AllocatedCells = new List<Systems.Grid.Cell>();

        Cell searchingCell = GridManager.Instance.Grid.GetCellByPosition(transform.position + new Vector3(UnitConversions.UnityLengthToPixel(16), UnitConversions.UnityLengthToPixel(16),0));

        CurrentCell = searchingCell;

        searchingCell.SlottedObject = this;
        AllocatedCells.Add(searchingCell);
    }
    
    
    public void ReallocateCells()
    {
        AllocatedCells = new List<Systems.Grid.Cell>();

        Cell searchingCell = GridManager.Instance.Grid.GetCellByPosition(transform.position + new Vector3(UnitConversions.UnityLengthToPixel(16), UnitConversions.UnityLengthToPixel(16),0));

        CurrentCell = searchingCell;

        searchingCell.SlottedObject = this;
        AllocatedCells.Add(searchingCell);
    }

    public void ReleaseCells()
    {
        foreach (Systems.Grid.Cell cell in AllocatedCells)
        {
            cell.SlottedObject = null;
        }
        AllocatedCells = null;
    }

    public void UnitSelection(Vector2 clickPosition, MouseClickType mouseClickType)
    {
        if (mouseClickType == MouseClickType.Left)
        {
            Cell clickedCell = GridManager.Instance.Grid.GetCellByPosition(Camera.main.ScreenToWorldPoint(clickPosition));
            if (clickedCell == null)
            {
                UnitManager.Instance.LastClickedUnit = null;
                UnitManager.Instance.OnUnitSelected.Invoke(null);
            }
            else if (AllocatedCells.Contains(clickedCell))
            {
                UnitManager.Instance.LastClickedUnit = this;
                UnitManager.Instance.OnUnitSelected.Invoke(Unit);
            }
        }
    }

    #endregion
}