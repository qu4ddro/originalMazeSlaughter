using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SyringeScript : MonoBehaviour
{

    public float SpeedBoostMultiplier = 2.0f;
    public float SpeedBoostDuration = 5.0f;

    private GameObject Player;

    // Use this for initialization
    void OnEnable()
    {
        Player = GameObject.FindWithTag("Player");
        Player.GetComponent<Player>().MoveTime = Player.GetComponent<Player>().MoveTime / SpeedBoostMultiplier;
        StartCoroutine(DeactivateAfterDuration());
    }

    IEnumerator DeactivateAfterDuration()
    {
        // Wait for an amount of Time before resetting
        yield return new WaitForSeconds(SpeedBoostDuration);
        //Reset MoveTime to standard MoveTime
        Player.GetComponent<Player>().MoveTime = Player.GetComponent<Player>().MoveTime * SpeedBoostMultiplier;
        //Disable this Object so it is hidden
        this.gameObject.SetActive(false);
    }
}
