using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System;

///INFO
///->Usage of UnitProductionHandler script: Handles the selection of which unit will be produced.
///ENDINFO

public class UnitProductionHandler : BaseInteractable
{
    #region Publics
    [FoldoutGroup("Handler Settings")]
    public Unit Unit;
    [FoldoutGroup("Handler Settings")]
    public TMPro.TextMeshProUGUI UnitName;
    [FoldoutGroup("Handler Settings")]
    public Image UnitImage;
    #endregion

    #region Privates

    #endregion

    #region Cached
    private Button interactionButton;
    public Button InteractionButton { get { return (interactionButton == null) ? interactionButton = GetComponent<Button>() : interactionButton; } }
    #endregion

    #region Events

    #endregion

    #region Monobehaviours
    private void OnEnable()
    {
        InitializeHandler();
        InteractionButton.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        InteractionButton.onClick.RemoveListener(OnClick);
    }
    #endregion

    #region Functions

    private void InitializeHandler()
    {
        UnitName.SetText(Unit.UnitName);
        UnitImage.sprite = Unit.UnitImage;
    }

    private void OnClick()
    {
        UnitManager.Instance.LastProducedUnit = Unit;
        PoolingManager.Instance.GetObjectFromPool(ProductionType.Unit);
    }
    #endregion

}
