using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
OGSingleton.cs
OGSingleton (c) Ominous Games 2017

Thanks to the Unity wiki for this informative how-to:
http://wiki.unity3d.com/index.php/Singleton
*/

public class OGSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instanceRef;
    private static object lockRef = new object();

    public static T instance
    {
        get
        {
            lock(lockRef)
            {
                if (FindObjectsOfType(typeof(T)).Length > 1)
                    OGDebug.LogError(string.Format("Multiple instances of {0} found!", typeof(T)), typeof(OGSingleton<T>));

                if(instanceRef == null)
                {
                    instanceRef = (T)FindObjectOfType(typeof(T));

                    //if(instanceRef == null)
                    //{
                    //    OGDebug.LogWarning(string.Format("Autoinstantiating {0}.", typeof(T)), typeof(OGSingleton<T>));
                    //
                    //    GameObject newInstance = new GameObject();
                    //    instanceRef = newInstance.AddComponent<T>();
                    //}
                }

                return instanceRef;
            }
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelLoad;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelLoad;
    }

    protected virtual void OnLevelLoad(Scene scene, LoadSceneMode mode)
    {

    }
}
