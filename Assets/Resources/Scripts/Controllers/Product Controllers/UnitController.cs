using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

///INFO
///->Usage of UnitController script: 
///ENDINFO

public class UnitController : MonoBehaviour, ISlotable
{
    #region Publics
    [FoldoutGroup("Data")]
    public Unit Unit;
    [FoldoutGroup("Data")]
    [ReadOnly]
    public bool IsInitialized = false;
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
    #endregion

    #region Functions

    [Button]
    public void Death()
    {
        PoolingManager.Instance.ReturnObjectToPool(ProductionType.Unit, gameObject);
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

    public void GetDamage()
    {
        
    }
    #endregion
}