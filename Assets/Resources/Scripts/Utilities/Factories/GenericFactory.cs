using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

///INFO
///->Usage of GenericFactory script: Generic script for factories that produces produceables(Buildings, Units etc.)
///ENDINFO

public abstract class GenericFactory<T> : MonoBehaviour, IFactory where T : MonoBehaviour

{
    #region Publics
    public T ProductionPrefab;
    public ProductionType ProductionType;
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
        ProductionManager.Instance.ProductionDictionary.Add(ProductionType, this);
    }
    #endregion

    #region Functions
    public T GetInstance()
    {
        return Instantiate(ProductionPrefab,transform);
    }

    public GameObject Produce()
    {
        return GetInstance().gameObject;
    }
    #endregion

}
