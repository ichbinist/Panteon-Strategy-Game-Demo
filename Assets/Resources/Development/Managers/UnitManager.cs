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
    #endregion

    #region Privates

    #endregion

    #region Cached

    #endregion

    #region Events
    public Action<UnitAffinityType> OnUnitAdded;
    public Action<UnitAffinityType> OnUnitRemoved;
    #endregion

    #region Monobehaviours

    #endregion

    #region Functions

    #endregion
}

[System.Serializable]
public class Unit
{
    #region Publics
    [FoldoutGroup("Unit Settings")]
    public UnitCombatType UnitCombatType;
    [FoldoutGroup("Unit Settings")]
    public UnitAffinityType UnitAffinityType;
    [FoldoutGroup("Unit Settings")]
    public Vector2 UnitSize;
    [FoldoutGroup("Unit Settings")]
    public string UnitName;
    [FoldoutGroup("Unit Settings")]
    public Sprite UnitImage;

    [FoldoutGroup("Unit Combat Settings")]
    public int Health;
    [FoldoutGroup("Unit Combat Settings")]
    public int Damage;

    [FoldoutGroup("Unit References")]
    public GameObject UnitPrefab;
    #endregion

    #region Events
    public Action<int> OnDamageTaken;
    public Action OnDestroyed;
    public Action<int> OnDamageDealt;
    #endregion

    #region Functions
    public Unit(UnitCombatType unitCombatType, UnitAffinityType unitAffinityType, Vector2 unitSize, string unitName, Sprite unitImage, int health, int damage, GameObject unitPrefab = null)
    {
        UnitCombatType = unitCombatType;
        UnitAffinityType = unitAffinityType;
        UnitSize = unitSize;
        UnitName = unitName;
        UnitImage = unitImage;
        Health = health;
        Damage = damage;
        UnitPrefab = unitPrefab;
    }

    public void GetDamage(int damage)
    {
        Health -= damage;
        OnDamageTaken.Invoke(damage);

        if (Health <= 0)
        {
            OnDestroyed.Invoke();
        }
    }

    public void DealDamage(int damage, Unit targetUnit)
    {
        targetUnit.GetDamage(damage);
        OnDamageDealt.Invoke(damage);
    }
    #endregion

}

public enum UnitCombatType
{
    Melee
}

public enum UnitAffinityType
{
    Player,
    Enemy
}