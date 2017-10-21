using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
AppManager.cs
AppManager (c) Ominous Games 2017
*/

public class AppManager : OGSingleton<AppManager>
{
    public enum AppState
    {
        NO_STATE = -10,
        MAIN_MENU = 0,
        LEVEL_SELECT,
        TUTORIAL,
        L1,
        L2,
        L3,
        L4,
        L5,
        L6,
        L7,
        L8,
        L9,
        L10,
        L11,
        L12,
        EXIT = 100
    };

    public OGState state { get; protected set; }
    public UIEvents ui { get; protected set; }
    public OGInput input { get; protected set; }

    public TGCConnectionController neurosky;

    public Gradient focusColor;

	private void Awake()
	{
        DontDestroyOnLoad(this);
    }

    private void Start()
    {

    }

    private void OnApplicationQuit()
    {

    }

    protected override void OnLevelLoad(Scene scene, LoadSceneMode mode)
    {
        base.OnLevelLoad(scene, mode);

        if (ui == null)
            ui = UIEvents.instance;

        if (input == null)
            input = OGInput.instance;

        if (state == null)
            ChangeState((AppState)scene.buildIndex);

        state.Init();
    }

    void ChangeState(AppManager.AppState newState)
    {
        Time.timeScale = 1.0f;

        if (state != null)
            state.Cleanup();

        switch(newState)
        {
            case AppState.MAIN_MENU:

                state = new MainMenuState(this, newState);
                break;

            case AppState.LEVEL_SELECT:

                state = new LevelSelectState(this, newState);
                break;

            case AppState.EXIT:

#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                Application.Quit();
                break;

            default:

                state = new GameplayState(this, newState);
                break;
        }

        if (SceneManager.GetActiveScene().buildIndex != (int)newState && newState != AppState.EXIT)
            SceneManager.LoadScene((int)newState);
    }
	
	void Update() 
	{
        AppState stateID = state.Update();

        if (stateID != state.stateID)
            ChangeState(stateID);
	}
}
