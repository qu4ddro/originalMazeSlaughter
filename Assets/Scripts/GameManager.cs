using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public int level = 1;

    public float levelStartDelay = 2f;                      //Time to wait before starting level, in seconds.
    public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

    public GameObject LevelImage;
    public Sprite[] LevelImages;
    public GameObject DeathScreen;
    public GameObject WinScreen;
    public GameObject Background;

    public Object[] scenesToLoad;
    private GameObject player;
    private GameObject lightObject;

    public bool PlayerIsAlive = true;

    public bool doingSetup;

    private SoundController soundManager;
    private AudioSource audioSource;

    public AudioClip WinAudioClip;
    public AudioClip LooseAudioClip;

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

        audioSource = GetComponent<AudioSource>();
    }

    void InitGame()
    {
        doingSetup = true;
        StartCoroutine("LoadNewScene");
    }
    
    public void NextLevel()
    {
        audioSource.PlayOneShot(WinAudioClip);
        StartCoroutine(soundManager.FadeSound(0, 1f));
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
        Background.SetActive(true);
        doingSetup = true;
    }

    public IEnumerator ShowScreen(GameObject screenToShow, int delayTime, int fadeTime)
    {
        float startTime = Time.time;
        while (startTime - Time.time < delayTime)
        {
            yield return null;
        }
        screenToShow.SetActive(true);
    }

    public void GameOver()
    {
        PlayerIsAlive = false;
        doingSetup = true;
        //StartCoroutine(FadeLight());
        audioSource.PlayOneShot(LooseAudioClip);
        Background.SetActive(true);
    }

    /*
    IEnumerator FadeLight()
    {
        float startTime = Time.time;
        Light light = lightObject.gameObject.GetComponent<Light>();
        float lightRange = light.range;
        while (lightRange > Mathf.Epsilon)
        {
            Debug.Log(lightRange);
            lightRange = Mathf.Lerp(lightRange, 0, 1/startTime-Time.time);
            light.range = lightRange;
        }
        yield return null;
    }
    */
    private void LoadNewScene()
    {
        Object scene = scenesToLoad[level - 1];
        WinScreen.SetActive(false);
        LevelImage.GetComponent<Image>().sprite = LevelImages[level-1];
        LevelImage.SetActive(true);

        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        SceneManager.LoadSceneAsync(scene.name);

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
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundController>();
        player = GameObject.FindGameObjectWithTag("Player");
        lightObject = GameObject.FindGameObjectWithTag("Light");
        doingSetup = false;
        LevelImage.SetActive(false);
    }
}