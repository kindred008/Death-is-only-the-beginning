using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] float mapBottomLimit;

    private Dictionary<string, bool> deathDictionary;
    private int deathPotential = 0;
    private int currentDeaths = 0;
    
    [SerializeField] TextMeshProUGUI deathScoreText;
    [SerializeField] TextMeshProUGUI levelCompleteText;

    public Transform currentCheckpoint;

    [HideInInspector] public GameObject player;
    [SerializeField] GameObject playerPrefab;


    private void Awake()
    {
        deathDictionary = new Dictionary<string, bool>();
    }

    private void Start()
    {
        player = Instantiate(playerPrefab, currentCheckpoint.position, playerPrefab.transform.rotation);
        GetComponent<FallDeath>().enabled = true;
    }

    public void AddDeathToDictionary(string deathType)
    {
        if (!deathDictionary.ContainsKey(deathType))
        {
            deathDictionary.Add(deathType, false);
            deathPotential++;
            UpdateDeathScore();
        }
    }

    public void UpdateDeathDictionary(string deathType)
    {
        if (!deathDictionary[deathType])
        {
            deathDictionary[deathType] = true;
            currentDeaths++;
            UpdateDeathScore();
        }
    }

    private void UpdateDeathScore()
    {
        deathScoreText.text = "Deaths: " + currentDeaths + "/" + deathPotential;
    }

    public void NewCheckPoint(Transform checkpoint)
    {
        currentCheckpoint = checkpoint;
    }

    public bool AttemptFinish()
    {
        if (currentDeaths == deathPotential)
        {
            levelCompleteText.text = "Level Complete";
            StartCoroutine(LevelFinished());
            return true;
        }

        return false;
    }

    private IEnumerator LevelFinished()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadSceneAsync(0);
    }
}
