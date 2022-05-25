using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] string trapName;

    private PlayerController playerScript;
    private LevelManager levelManager;

    private void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

        levelManager.AddDeathToDictionary(trapName);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerScript = collision.gameObject.GetComponent<PlayerController>();
            playerScript.PlayerDeath();
            levelManager.UpdateDeathDictionary(trapName);
        }
    }
}
