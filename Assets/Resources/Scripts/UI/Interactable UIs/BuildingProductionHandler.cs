using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

///INFO
///->Usage of BuildingProductionHandler script: Handles the selection of which building will be produced.
///ENDINFO

public class BuildingProductionHandler : BaseInteractable
{
    #region Publics
    [FoldoutGroup("Handler Settings")]
    public Building Building;
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
        InteractionButton.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        InteractionButton.onClick.RemoveListener(OnClick);
    }
    #endregion

    #region Functions
    private void OnClick()
    {
        BuildingManager.Instance.LastProducedBuilding = Building;
        PoolingManager.Instance.GetObjectFromPool(ProductionType.Building);
    }
    #endregion

}
