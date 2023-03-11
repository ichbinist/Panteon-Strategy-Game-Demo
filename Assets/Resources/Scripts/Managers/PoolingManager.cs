using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

///INFO
///->Usage of PoolingManager script: When initialized, creates multiple object pools by instantiating them, enables and disables them, resets them and reuses them when needed, basicly pooling system.
///ENDINFO

public class PoolingManager : Singleton<PoolingManager>
{
    #region Publics
    public List<Pool> Pools = new List<Pool>();
    #endregion

    #region Privates

    #endregion

    #region Cached

    #endregion

    #region Events

    #endregion

    #region Monobehaviours
    private void OnEnable()
    {
        InitializePools();
    }
    #endregion

    #region Functions
    private void InitializePools()
    {
        foreach (Pool pool in Pools)
        {
            pool.InitializePool();
        }
    }
    #endregion
}
[System.Serializable]
public class Pool
{
    public ProductionType ProductionType;
    public int DefaultPoolObjectCount = 10;
    [FoldoutGroup("Pool Objects")]
    [ReadOnly]
    public List<GameObject> InUsePoolObjects;
    [FoldoutGroup("Pool Objects")]
    [ReadOnly]
    public List<GameObject> AvailablePoolObjects;
    public bool IsPoolObjectAvailable { get { return (AvailablePoolObjects.Count > 0) ? true : false; } }

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
                //Reset Object
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
            //Reset Object
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
            inUsePoolObject.SetActive(false);
            //Reset Object
        }
    }
}