using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : Wall
{
    public List<GameObject> SubordinateGameObjects;
    public int Timer;

    private DateTime cooldown = DateTime.Now;
    private bool active = false;
    private bool canBeActivated = true;

    private SpriteRenderer spriteRenderer;
    private AudioSource audiosource;
    
    public Sprite SwitchActiveSprite;
    public Sprite SwitchInactiveSprite;

    public AudioClip SwitchActivateAudioClip;
    
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audiosource = GetComponent<AudioSource>();
    }

    public void Activate()
    {
        if (((DateTime.Now-cooldown).Seconds > Timer) && canBeActivated)
        {
            audiosource.PlayOneShot(SwitchActivateAudioClip);
            if (active)
                spriteRenderer.sprite = SwitchActiveSprite;
            else
                spriteRenderer.sprite = SwitchInactiveSprite;
            active = !active;
            var timer = 0.1f;
            foreach (GameObject subordinateGameObject in SubordinateGameObjects)
            {
                if (subordinateGameObject)
                {
                    if (subordinateGameObject.GetComponent<Water>())
                    {
                        StartCoroutine(ToggleWater(subordinateGameObject, timer));
                        timer += 0.1f;
                    }
                    else if (subordinateGameObject.GetComponent<Stone>())
                    {
                        subordinateGameObject.SetActive(true);
                        canBeActivated = false;
                    }
                }
            }
            cooldown = DateTime.Now;
        }
    }

    private IEnumerator ToggleWater(GameObject water, float wait)
    {
        yield return new WaitForSeconds(wait);
        water.SetActive(!water.activeSelf);
    }
}
