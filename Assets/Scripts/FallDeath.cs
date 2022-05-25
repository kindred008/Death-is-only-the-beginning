using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDeath : MonoBehaviour
{
    [SerializeField] float mapBottomLimit;

    private LevelManager levelManager;
    private GameObject player;

    private void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        player = levelManager.player;
        levelManager.AddDeathToDictionary("FallDeath");
    }

    private void Update()
    {
        if (player.transform.position.y < mapBottomLimit)
        {
            player.GetComponent<PlayerController>().PlayerDeath();
            levelManager.UpdateDeathDictionary("FallDeath");
        }
    }
}
