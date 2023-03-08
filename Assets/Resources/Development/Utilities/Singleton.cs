using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Object
{
    #region Member Variables

    private static T instance;

    #endregion

    #region Properties

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<T>();
            }

            return instance;
        }
    }

    #endregion

    #region Unity Methods

    protected virtual void Awake()
    {
        SetInstance();
    }

    #endregion

    #region Public Methods

    public static bool Exists()
    {
        return instance != null;
    }

    public bool SetInstance()
    {
        if (instance != null && instance != gameObject.GetComponent<T>())
        {
            Debug.LogWarning("[SingletonComponent] Instance already set for type " + typeof(T));
            return false;
        }

        instance = gameObject.GetComponent<T>();

        return true;
    }

    #endregion
}

