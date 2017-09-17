using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
OGDebug.cs
OGDebug (c) Ominous Games 2017
*/

public class OGDebug
{
    public static void LogWarning(string msg, System.Type callerType = null)
    {
        if (callerType != null)
            Debug.LogWarning(string.Format("Warning <{0}>: {1}", callerType, msg));
        else
            Debug.LogWarning(string.Format("Warning: {0}", msg));
    }

    public static void LogError(string msg, System.Type callerType = null)
    {
        if (callerType != null)
            Debug.LogError(string.Format("Error <{0}>: {1}", callerType, msg));
        else
            Debug.LogError(string.Format("Error: {0}", msg));
    }

    public static void LogMessage(string msg, System.Type callerType = null)
    {
        if (callerType != null)
            Debug.Log(string.Format("Message <{0}>: {1}", callerType, msg));
        else
            Debug.Log(string.Format("Message: {0}", msg));
    }
}
