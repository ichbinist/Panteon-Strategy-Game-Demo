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

    [FoldoutGroup("Config")]
    [ReadOnly]
    public Building LastProducedBuilding;

    [FoldoutGroup("Config")]
    [ReadOnly]
    public BuildingController LastClickedBuildingController;
    #endregion

    #region Privates

    #endregion

    #region Cached

    #endregion

    #region Events
    public Action<BuildingAffinityType> OnBuildingAdded;
    public Action<BuildingAffinityType> OnBuildingRemoved;

    public Action<Building> OnBuildingSelected;
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