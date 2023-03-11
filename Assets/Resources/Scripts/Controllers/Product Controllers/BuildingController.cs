using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

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
        PoolingManager.Instance.ReturnObjectToPool(ProductionType.Building, gameObject);
    }

    private void OnRecycle(GameObject recycledObject)
    {
        if (isGameObjectEqual(recycledObject))
        {
            ResetController();
            InitializeController(BuildingManager.Instance.LastProducedBuilding);
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
        }
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
