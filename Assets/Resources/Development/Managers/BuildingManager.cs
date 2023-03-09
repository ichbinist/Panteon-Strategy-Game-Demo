using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;
using System;

///INFO
///->Usage of BuildingManager script: Holds the Data for Buildings, Number of Player and Enemy Buildings in the scene and their positions, allows Unit AI to reach them more confortable.
///ENDINFO

public class BuildingManager : Singleton<BuildingManager>
{
    #region Publics
    [FoldoutGroup("Produceable Building List")]
    [HideLabel]
    public List<Building> ProduceableBuildings = new List<Building>();

    [FoldoutGroup("Produced Building List")]
    [HideLabel]
    [ReadOnly]
    public List<Building> ProducedBuildings = new List<Building>();

    public List<Building> GetPlayerBuildings { get { return (ProducedBuildings.FindAll(x => x.BuildingAffinityType == BuildingAffinityType.Player)); } }
    public List<Building> GetEnemyBuildings { get { return (ProducedBuildings.FindAll(x => x.BuildingAffinityType == BuildingAffinityType.Enemy)); } }
    #endregion

    #region Privates

    #endregion

    #region Cached

    #endregion

    #region Events
    public Action<BuildingAffinityType> OnBuildingAdded;
    public Action<BuildingAffinityType> OnBuildingRemoved;
    #endregion

    #region Monobehaviours

    #endregion

    #region Functions
    public void AddBuilding(Building building)
    {
        ProducedBuildings.Add(building);
        OnBuildingAdded.Invoke(building.BuildingAffinityType);
    }

    public void RemoveBuilding(Building building)
    {
        OnBuildingRemoved.Invoke(building.BuildingAffinityType);
        ProducedBuildings.Remove(building);
    }
    #endregion
}

[System.Serializable]
public class Building
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
    [ShowIf("IsDetailedInfoBuilding")]
    public List<Unit> ProduceableUnits = new List<Unit>();
    [FoldoutGroup("Building Settings")]
    public int Health;

    [FoldoutGroup("Building References")]
    public GameObject BuildingPrefab;

    [HideInInspector]
    public bool isDetailedInfoBuilding { get { return (BuildingInfoType == BuildingInfoType.Detailed) ? true : false; } }
    #endregion

    #region Events
    public Action<int> OnDamageTaken;
    public Action OnDestroyed;
    #endregion

    #region Privates

    #endregion

    #region Functions
    public Building(BuildingInfoType buildingInfoType, BuildingAffinityType buildingAffinityType, Vector2 buildingSize, string buildingName, Sprite buildingImage, int health, List<Unit> produceableUnits = null, GameObject buildingPrefab = null)
    {
        BuildingInfoType = buildingInfoType;
        BuildingAffinityType = buildingAffinityType;
        BuildingSize = buildingSize;
        BuildingName = buildingName;
        BuildingImage = buildingImage;
        ProduceableUnits = produceableUnits;
        BuildingPrefab = buildingPrefab;
        Health = health;
    }

    public void GetDamage(int damage)
    {
        Health -= damage;
        OnDamageTaken.Invoke(damage);

        if(Health <= 0)
        {
            OnDestroyed.Invoke();
        }
    }
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

