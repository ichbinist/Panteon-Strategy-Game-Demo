using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;
using System;

///INFO
///->Usage of PoolingManager script: When initialized, creates multiple object pools by instantiating them, enables and disables them, resets them and reuses them when needed, basicly pooling system.
///ENDINFO

public class PoolingManager : Singleton<PoolingManager>
{
    #region Publics
    [ReadOnly]
    public PoolDictionary PoolDictionary = new PoolDictionary();
    public int DefaultObjectPoolSize = 15;
    #endregion

    #region Privates

    #endregion

    #region Cached

    #endregion

    #region Events
    public Action<GameObject> OnObjectRecycled;
    public Action<GameObject> OnObjectReturned;
    public Action<GameObject> OnObjectProduced;
    #endregion

    #region Monobehaviours
    [Button]
    private void OnEnable()
    {
        InitializeBuildingPools();
    }
    #endregion

    #region Functions
    private void InitializeBuildingPools()
    {
        PoolDictionary.Add(ProductionType.Building, new Pool(ProductionType.Building, DefaultObjectPoolSize));
    }
    public void InitializeUnitPools()
    {
        PoolDictionary.Add(ProductionType.Unit, new Pool(ProductionType.Unit, DefaultObjectPoolSize));
    }

    public GameObject GetObjectFromPool(ProductionType productionType)
    {
        return PoolDictionary[productionType].GetPoolObject();
    }

    public void ReturnObjectToPool(ProductionType productionType, GameObject inUsePoolObject)
    {
        PoolDictionary[productionType].ReturnPoolObject(inUsePoolObject);
    }
    #endregion
}
[System.Serializable]
public class Pool
{
    public ProductionType ProductionType;
    public int DefaultPoolObjectCount;
    [FoldoutGroup("Pool Objects")]
    [ReadOnly]
    public List<GameObject> InUsePoolObjects = new List<GameObject>();
    [FoldoutGroup("Pool Objects")]
    [ReadOnly]
    public List<GameObject> AvailablePoolObjects = new List<GameObject>();
    public bool IsPoolObjectAvailable { get { return (AvailablePoolObjects.Count > 0) ? true : false; } }

    internal Pool(ProductionType productionType, int defaultPoolObjectCount)
    {
        ProductionType = productionType;
        DefaultPoolObjectCount = defaultPoolObjectCount;
        InitializePool();
    }

    internal void InitializePool()
    {
        for (int i = 0; i < DefaultPoolObjectCount; i++)
        {
            GameObject createdPoolObject = ProductionManager.Instance.GetProduct(ProductionType);
            createdPoolObject.SetActive(false);
            AvailablePoolObjects.Add(createdPoolObject);
        }
    }

    [Button]
    public GameObject GetPoolObject()
    {
        if (IsPoolObjectAvailable)
        {
            GameObject cachedPoolObject = AvailablePoolObjects.FirstOrDefault();
            if (AvailablePoolObjects.Contains(cachedPoolObject))
            {
                AvailablePoolObjects.Remove(cachedPoolObject);
                InUsePoolObjects.Add(cachedPoolObject);
                cachedPoolObject.SetActive(true);
                //Reset and Initialize
                PoolingManager.Instance.OnObjectRecycled.Invoke(cachedPoolObject);
                return cachedPoolObject;
            }
            else
            {
                return GetPoolObject();
            }
        }
        else
        {
            GameObject producedPoolObject = ProductionManager.Instance.GetProduct(ProductionType);
            InUsePoolObjects.Add(producedPoolObject);
            producedPoolObject.SetActive(true);
            //Initialize
            PoolingManager.Instance.OnObjectProduced.Invoke(producedPoolObject);
            return producedPoolObject;
        }
    }

    [Button]
    public void ReturnPoolObject(GameObject inUsePoolObject)
    {
        if(InUsePoolObjects.Contains(inUsePoolObject) && !AvailablePoolObjects.Contains(inUsePoolObject))
        {
            InUsePoolObjects.Remove(inUsePoolObject);
            AvailablePoolObjects.Add(inUsePoolObject);
            PoolingManager.Instance.OnObjectReturned.Invoke(inUsePoolObject);
            inUsePoolObject.SetActive(false);
            //Reset Object
        }
    }
}
[Serializable]
public class PoolDictionary : Dictionary<ProductionType, Pool> { }