﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : Wall {

    public Sprite dmgSprite;
    public int health;                          //hit points for the wall.

    private SpriteRenderer spriteRenderer;

    public AudioClip DestroyWallAudioClip;

    // Use this for initialization
    void Awake ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DamageWall(int damage)
    {
        //ToDo: exchange sprite to dmgsprite
        //SpriteRenderer.sprite = dmgSprite;
        health -= damage;
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
