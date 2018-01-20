using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float levelStartDelay = 2f;                      //Time to wait before starting level, in seconds.
    public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

    public GameObject LevelImage;
    public Sprite[] LevelImages;
    public GameObject DeathScreen;
    public GameObject WinScreen;

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
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
        
        //Call the InitGame function to initialize the first level 
        InitGame();
    }

    void InitGame()
    {
        doingSetup = true;
        StartCoroutine("LoadNewScene");
    }
    
    public void NextLevel()
    {
        this.WinScreen.SetActive(true);
        doingSetup = true;
        level++;
        if (scenesToLoad[level - 1])
        {
            Invoke("LoadNewScene", 3);
        }
        else if (scenesToLoad[level - 1])
        {
            doingSetup = true;
            GameWon();
        }
    }

    private void GameWon()
    {
        WinScreen.SetActive(true);
        doingSetup = true;
    }

    public void GameOver()
    {
        PlayerIsAlive = false;
        DeathScreen.SetActive(true);
    }

    private void LoadNewScene()
    {
        Object scene = scenesToLoad[level - 1];
        WinScreen.SetActive(false);
        LevelImage.GetComponent<Image>().sprite = LevelImages[level-1];
        LevelImage.SetActive(true);

        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
         AsyncOperation async = SceneManager.LoadSceneAsync(scene.name);

        /*
        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            yield return null;
        }
        */

        Invoke("FinishedLoading", levelStartDelay);

    }

    private void FinishedLoading()
    {
        Debug.Log("Finished");
        doingSetup = false;
        LevelImage.SetActive(false);
    }
}
