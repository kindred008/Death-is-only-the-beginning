using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    PlayerController playerScript;

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            playerScript = collision.gameObject.GetComponent<PlayerController>();
            playerScript.PlayerDeath();
            LevelManager.levelManagerInstance.UpdateDeathDictionary("Spike");
        }
    }
}