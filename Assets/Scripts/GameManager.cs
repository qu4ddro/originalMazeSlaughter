﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

using System.Collections.Generic;       //Allows us to use Lists. 
using UnityEngine.UI;                   //Allows us to use UI.

public class GameManager : MonoBehaviour
{
    public float levelStartDelay = 2f;                      //Time to wait before starting level, in seconds.
    public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

    public GameObject LevelImage;
    public Sprite[] LevelImages;
    public GameObject DeathScreen;
    public GameObject WinScreen;
    //private bool doingSetup = true;                         //Boolean to check if we're setting up board, prevent Player from moving during setup.

    public int level = 1;
    public Object[] scenesToLoad;
    public GameObject player;
    public bool PlayerIsAlive = true;

    public bool doingSetup;

    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        //Call the InitGame function to initialize the first level 
        InitGame();
    }

    //this is called only once, and the paramter tell it to be called only after the scene was loaded
    //(otherwise, our Scene Load callback would be called the very first load, and we don't want that)
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static public void CallbackInitialization()
    {
        //register the callback to be called everytime the scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //This is called each time a scene is loaded.
    static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        //instance.InitGame();
    }


    void InitGame()
    {
        doingSetup = true;

        StartCoroutine(LoadNewScene(scenesToLoad[level-1]));
    }


    //GameOver is called when the player reaches 0 food points
    public void GameOver()
    {
        PlayerIsAlive = false;
        DeathScreen.SetActive(true);
        //Disable this GameManager.
    }

    public void NextLevel()
    {
        level++;
        if (scenesToLoad[level - 1])
        {
            StartCoroutine(LoadNewScene(scenesToLoad[level - 1]));
        }
        else if (scenesToLoad[level - 1])
        {
            GameWon();
        }
    }

    private void GameWon()
    {
        WinScreen.SetActive(true);
    }

    // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
    IEnumerator LoadNewScene(Object scene)
    {
        LevelImage.GetComponent<Image>().sprite = LevelImages[level-1];
        LevelImage.SetActive(true);

        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
         AsyncOperation async = SceneManager.LoadSceneAsync(scene.name);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            yield return null;
        }

        Invoke("FinishedLoading",levelStartDelay);
    }

    private void FinishedLoading()
    {
        doingSetup = false;
        LevelImage.SetActive(false);
    }
}
