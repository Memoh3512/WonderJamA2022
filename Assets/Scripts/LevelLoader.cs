﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    //transition times for every transition
    public float CrossfadeTime = 1f;
    public Animator Crossfade;

    public float CoolTransitionTime = 1f;
    public Animator CoolTransition;
    
    //singleton
    public static LevelLoader instance;

    private void Awake()
    {

        Crossfade.gameObject.SetActive(false);
        CoolTransition.gameObject.SetActive(false);

        if (instance == null)
        {

            instance = this;
            
            //SPAGHETT
            GameManager.characters = new List<Character>()
            {
                new Character()
                {
                    spr = Resources.Load<Sprite>("Characters/Coccinelle"),
                    name = "Ladybug"
                },
                new Character()
                {
                    spr = Resources.Load<Sprite>("Characters/Sauterelle"),
                    name = "Grasshopper"
                },
                new Character()
                {
                    spr = Resources.Load<Sprite>("Characters/Chenille"),
                    name = "Caterpillar"
                },
            };

        }
        else
        {
            
            Debug.Log("There is already an instance of LevelLoader in the game, destroying this!");
            Destroy(gameObject);
            
        }
        
    }

    private void Start()
    {
        
        SoundPlayer.instance.SetMusic(Songs.Crickets);
        
    }

    public void LoadScene(SceneTypes scene, TransitionTypes transition)
    {

        float time = 0f;
        Animator transitionObj = Crossfade;

        switch (transition)
        {
            
            case TransitionTypes.CrossFade:

                time = CrossfadeTime;
                transitionObj = Crossfade;
                
                break;
            case TransitionTypes.CoolTransition:

                time = CoolTransitionTime;
                transitionObj = CoolTransition;

                break;
            
        }

        StartCoroutine(TransitionRoutine((int) scene, transitionObj, time));


    }

    IEnumerator TransitionRoutine(int scene, Animator transition, float transitionTime)
    {
        
        transition.gameObject.SetActive(true);
        
        transition.SetTrigger("Start");
        
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(scene);

    }
    
}
