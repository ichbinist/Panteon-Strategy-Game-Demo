using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

///INFO
///->Usage of Unit script: 
///ENDINFO
///
[CreateAssetMenu(fileName = "Unit", menuName = "Data/UnitScriptableObject", order = 2)]
[System.Serializable]
public class Unit : ScriptableObject
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
    #endregion

    #region Events

    #endregion

    #region Functions

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