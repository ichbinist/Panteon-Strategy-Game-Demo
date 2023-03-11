using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

///INFO
///->Usage of Building script: 
///ENDINFO
///
[CreateAssetMenu(fileName = "Building", menuName = "Data/BuildingScriptableObject", order = 1)]
[System.Serializable]
public class Building : ScriptableObject
{
    #region Publics
    [FoldoutGroup("Building Settings")]
    public BuildingInfoType BuildingInfoType;
    [FoldoutGroup("Building Settings")]
    public BuildingAffinityType BuildingAffinityType;
    [FoldoutGroup("Building Settings")]
    public Vector2 BuildingSize;
    [FoldoutGroup("Building Settings")]
    public string BuildingName;
    [FoldoutGroup("Building Settings")]
    public Sprite BuildingImage;
    [FoldoutGroup("Building Settings")]
    [ShowIf("isDetailedInfoBuilding")]
    public List<Unit> ProduceableUnits = new List<Unit>();
    [FoldoutGroup("Building Settings")]
    public int Health;

    [HideInInspector]
    public bool isDetailedInfoBuilding { get { return (BuildingInfoType == BuildingInfoType.Detailed) ? true : false; } }
    #endregion

    #region Events

    #endregion

    #region Privates

    #endregion

    #region Functions

    #endregion
}

public enum BuildingInfoType
{
    Detailed,
    Non_Detailed
}

public enum BuildingAffinityType
{
    Player,
    Enemy
}
