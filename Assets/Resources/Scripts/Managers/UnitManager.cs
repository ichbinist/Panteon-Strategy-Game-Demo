using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

///INFO
///->Usage of UnitManager script: Holds the Data of Player and Enemy Units, Positions and their current health. 
/// Also Holds the Produceable Unit data which is used by Barracks and can be used by other unit producing buildings in the future.
///ENDINFO

public class UnitManager : Singleton<UnitManager>
{
    
    #region Publics
    [FoldoutGroup("Units")]
    [HideLabel]
    public List<Unit> ProduceableUnits = new List<Unit>();

    [FoldoutGroup("Produced Units")]
    [HideLabel]
    [ReadOnly]
    public List<Unit> ProducedUnits = new List<Unit>();

    public List<Unit> GetPlayerUnits { get { return (ProducedUnits.FindAll(x => x.UnitAffinityType == UnitAffinityType.Player)); } }
    public List<Unit> GetEnemyUnits { get { return (ProducedUnits.FindAll(x => x.UnitAffinityType == UnitAffinityType.Enemy)); } }

    [FoldoutGroup("Config")]
    [ReadOnly]
    public Unit LastProducedUnit;
    [FoldoutGroup("Config")]
    [ReadOnly]
    public UnitGenericController LastClickedUnit;
    #endregion

    #region Privates

    #endregion

    #region Cached

    #endregion

    #region Events
    public Action<UnitAffinityType> OnUnitAdded;
    public Action<UnitAffinityType> OnUnitRemoved;
    public Action<Unit> OnUnitSelected;
    #endregion

    #region Monobehaviours

    #endregion

    #region Functions
    public void AddUnit(Unit unit)
    {
        ProducedUnits.Add(unit);
        OnUnitAdded.Invoke(unit.UnitAffinityType);
    }

    public void RemoveUnit(Unit unit)
    {
        if(unit != null && OnUnitRemoved != null)
        {
            OnUnitRemoved.Invoke(unit.UnitAffinityType);
            ProducedUnits.Remove(unit);
        }
    }
    #endregion

}