using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
MainMenu.cs
MainMenu (c) Ominous Games 2017
*/

public class MainMenu : MonoBehaviour
{
    public GameObject mainGroup;
    public GameObject optionGroup;

    public void ToggleOptions(bool options)
    {
        mainGroup.SetActive(!options);
        optionGroup.SetActive(options);
    }
}
