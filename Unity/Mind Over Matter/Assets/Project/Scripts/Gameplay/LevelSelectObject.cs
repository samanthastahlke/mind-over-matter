using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
LevelSelectObject.cs
LevelSelectObject (c) Ominous Games 2017
*/

public class LevelSelectObject : MindObject
{
    public int levelSelect;

    void Update()
    {
        PlayUpdate();
        OutlineUpdate();
        AudioUpdate();
    }
}
