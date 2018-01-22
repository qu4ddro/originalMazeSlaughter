using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
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

    public string[] scenesToLoad = new string[3];

    private GameObject player;
    private GameObject lightObject;

    public bool PlayerIsAlive = true;

    public bool doingSetup;

    private SoundController soundManager;
    private AudioSource audioSource;

    public AudioClip WinAudioClip;
    public AudioClip LooseAudioClip;

    public AudioClip NoMoreItemsAudioClip;
    public AudioClip NoExitAudioClip;

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

        scenesToLoad[0] = "Level_1";
        scenesToLoad[1] = "Level_2";
        scenesToLoad[2] = "Level_3";

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
        if (scenesToLoad[level - 1] != null)
        {
            Invoke("LoadNewScene", 3);
        }
        else if (scenesToLoad[level - 1] !=null)
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
        StartCoroutine(FadeLight(0,.5f));
        audioSource.PlayOneShot(LooseAudioClip);
        StartCoroutine(soundManager.FadeSound(0, 1f));
    }


    public IEnumerator FadeLight(float _newVolume, float FadeDuration)
    {
        GameObject mask = GameObject.FindGameObjectWithTag("Mask");
        //Light light = lightObject.gameObject.GetComponent<Light>();
        float lightRange = mask.transform.localScale.x;

        float startTime = Time.time;
        float elapsedTime = 0;
        float remaining = _newVolume - lightRange;

        while (Mathf.Abs(remaining) > float.Epsilon)
        {
            float newscale = Mathf.Lerp(lightRange, _newVolume, elapsedTime / FadeDuration);
            mask.transform.localScale = new Vector3(newscale, newscale, newscale);
            elapsedTime = Time.time - startTime;
            remaining = _newVolume - mask.transform.localScale.x;
            yield return null;
        }
        Background.SetActive(true);
        DeathScreen.SetActive(true);
    }

    private void LoadNewScene()
    {
        var scene = scenesToLoad[level - 1];
        WinScreen.SetActive(false);
        LevelImage.GetComponent<Image>().sprite = LevelImages[level-1];
        LevelImage.SetActive(true);
        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        SceneManager.LoadSceneAsync(scene);

        /*
        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            yield return null;
        }
        */

        StartCoroutine(FinishedLoading());
    }

    private IEnumerator FinishedLoading()
    {
        while (Input.anyKey ==false)
        {
            yield return null;
        }
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundController>();
        player = GameObject.FindGameObjectWithTag("Player");
        lightObject = GameObject.FindGameObjectWithTag("Light");
        soundManager.PlayDelayerStartClip();
        Invoke("StartLevel", levelStartDelay);
       
    }

    private void StartLevel()
    {
        doingSetup = false;
        LevelImage.SetActive(false);
    }

    public void PlayItemErrorSound()
    {
        audioSource.PlayOneShot(NoMoreItemsAudioClip);
    }

    public void PlayExitErrorSound()
    {
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(NoExitAudioClip);
    }
}