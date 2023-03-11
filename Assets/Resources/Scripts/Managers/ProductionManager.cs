using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

///INFO
///->Usage of ProductionManager script: Handles the requests from Pooling system by producing required product and sends it back to pooling system.
///ENDINFO

public class ProductionManager : Singleton<ProductionManager>
{
    #region Publics
    public ProductionDictionary ProductionDictionary;
    #endregion

    #region Privates

    #endregion

    #region Cached

    #endregion

    #region Events

    #endregion

    #region Monobehaviours

    #endregion

    #region Functions
    public GameObject GetProduct(ProductionType productionType)
    {
        return ProductionDictionary[productionType].Produce();
    }
    #endregion
}
[Serializable]
public enum ProductionType
{
    Building,
    Unit
}
[Serializable]
public class ProductionDictionary : Dictionary<ProductionType, IFactory> { }