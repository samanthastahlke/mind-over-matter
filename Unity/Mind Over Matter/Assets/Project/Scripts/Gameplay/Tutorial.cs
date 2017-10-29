using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
Tutorial.cs
Tutorial (c) Ominous Games 2017
*/

public class Tutorial : MonoBehaviour
{
    enum TutorialStage
    {
        NIL = -1,
        SELECT = 0,
        PICKUP,
        DROP,
        WARN,
        FINISH,
        FINISHED
    };

    public MindObject obj;
    private float actionTimer = 0.0f;
    public float actionTime = 1.0f;
    public float promptTime = 5.0f;
    public float fadeTime = 0.5f;
    private TutorialStage stage = TutorialStage.NIL;

    public List<Text> tutorialStages;
    private bool transitioning = false;

    IEnumerator NextStage()
    {
        transitioning = true;
        float timer = 0.0f;

        if(stage != TutorialStage.NIL)
        {
            while (timer < fadeTime)
            {
                timer += Time.deltaTime;
                tutorialStages[(int)stage].color = Color.Lerp(Color.white, Color.clear, timer / fadeTime);
                yield return null;
            }

            tutorialStages[(int)stage].gameObject.SetActive(false);
        }

        ++stage;

        if (stage > TutorialStage.SELECT)
            obj.canBePickedUp = true;

        if (stage == TutorialStage.FINISH)
            ((GameplayState)AppManager.instance.state).TriggerWinState();

        timer = 0.0f;

        if(stage != TutorialStage.FINISHED)
        {
            tutorialStages[(int)stage].gameObject.SetActive(true);
            tutorialStages[(int)stage].color = Color.clear;

            while (timer < fadeTime)
            {
                timer += Time.deltaTime;
                tutorialStages[(int)stage].color = Color.Lerp(Color.clear, Color.white, timer / fadeTime);
                yield return null;
            }
        }

        transitioning = false;
        actionTimer = 0.0f;
    }
    
    void Start() 
    {
        obj.canBePickedUp = false;
        stage = TutorialStage.NIL;
        StartCoroutine(NextStage());
    }
    
    void Update() 
    {
        if(!transitioning)
        {
            switch (stage)
            {
                case TutorialStage.SELECT:

                    if (obj.IsSelected())
                        actionTimer += Time.deltaTime;
                    else
                        actionTimer = 0.0f;

                    if (actionTimer > actionTime)
                        StartCoroutine(NextStage());

                    break;

                case TutorialStage.PICKUP:

                    if (obj.HasFocus())
                        StartCoroutine(NextStage());

                    break;

                case TutorialStage.DROP:

                    if (!obj.HasFocus())
                        StartCoroutine(NextStage());

                    break;

                case TutorialStage.WARN:

                    actionTimer += Time.deltaTime;

                    if (actionTimer > promptTime)
                        StartCoroutine(NextStage());

                    break;
            }
        }
        
    }
}
