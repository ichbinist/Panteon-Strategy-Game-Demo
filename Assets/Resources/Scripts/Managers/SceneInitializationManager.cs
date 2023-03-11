using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
using System;

///INFO
///->Usage of SceneInitializationManager script: This script Loads the UI Scene and Gameplay Scene Asyncronly.
///ENDINFO

public class SceneInitializationManager : Singleton<SceneInitializationManager>
{
    #region Publics

    #endregion

    #region Privates

    #endregion

    #region Cached

    #endregion

    #region Events
    public Action OnUILoaded;
    public Action OnGameplayLoaded;
    #endregion

    #region Monobehaviours
    public void Start()
    {
        Initialization();
    }
    #endregion

    #region Functions
    private void Initialization()
    {
        StartCoroutine(DoLoadScenes());
    }

    private IEnumerator DoLoadScenes()
    {
        yield return StartCoroutine(DoLoadUI());
        yield return StartCoroutine(DoLoadGameplay());
    }

    private IEnumerator DoLoadUI()
    {     
        if (!SceneManager.GetSceneByName("UIScene").isLoaded)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("UIScene", LoadSceneMode.Additive);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            OnUILoaded?.Invoke();
        }
        yield return null;
    }

    private IEnumerator DoLoadGameplay()
    {
        if (!SceneManager.GetSceneByName("Gameplay").isLoaded)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Gameplay", LoadSceneMode.Additive);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            OnGameplayLoaded?.Invoke();
        }
        yield return null;
    }
    #endregion
}
