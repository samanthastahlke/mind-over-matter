using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Monitor.cs
Monitor (c) Ominous Games 2017
*/

public class Monitor : MonoBehaviour 
{
    public List<GameObject> globalPrefabs;
	
	private void Awake() 
	{
        if (FindObjectOfType<AppManager>() == null)
        {
            for (int i = 0; i < globalPrefabs.Count; ++i)
            {
                Instantiate(globalPrefabs[i]);
            }
        }
    }
}
