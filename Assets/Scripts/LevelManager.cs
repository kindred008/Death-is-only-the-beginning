using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager levelManagerInstance;

    [SerializeField] float mapBottomLimit = -15.0f;

    private Dictionary<string, bool> deathDictionary;
    private int deathPotential;
    private int currentDeaths;

    private Transform player;
    private PlayerController playerScript;
    private TextMeshProUGUI deathScoreText;
    private TextMeshProUGUI levelCompleteText;

    private void Awake()
    {
        if (levelManagerInstance == null)
        {
            levelManagerInstance = this;
            DontDestroyOnLoad(this);
        } else
        {
            Destroy(this);
        }

        deathDictionary = new Dictionary<string, bool>();
    }

    private void Start()
    {
        SceneManager.activeSceneChanged += LevelStarted;
        player = GameObject.Find("Player").transform;
        playerScript = player.GetComponent<PlayerController>();
        deathScoreText = GameObject.Find("DeathText").GetComponent<TextMeshProUGUI>();
        levelCompleteText = GameObject.Find("LevelCompleteText").GetComponent<TextMeshProUGUI>();

        InitialiseDeathDictionary();

        UpdateDeathScore();
    }

    private void LevelStarted(Scene current, Scene next)
    {
        player = GameObject.Find("Player").transform;
        playerScript = player.GetComponent<PlayerController>();
        deathScoreText = GameObject.Find("DeathText").GetComponent<TextMeshProUGUI>();
        levelCompleteText = GameObject.Find("LevelCompleteText").GetComponent<TextMeshProUGUI>();

        UpdateDeathScore();
    }

    // Change this later - temporary
    private void InitialiseDeathDictionary()
    {
        deathDictionary.Add("Spike", false);
        deathDictionary.Add("Fall", false);

        deathPotential = deathDictionary.Count;
    }

    public void UpdateDeathDictionary(string deathType)
    {
        deathDictionary[deathType] = true;
        UpdateDeathScore();
    }

    private void UpdateDeathScore()
    {
        currentDeaths = 0;
        foreach(bool value in deathDictionary.Values)
        {
            if (value)
            {
                currentDeaths++;
            }
        }
        deathScoreText.text = "Deaths: " + currentDeaths + "/" + deathPotential;
    }

    public void AttemptFinish()
    {
        if (currentDeaths == deathPotential)
        {
            levelCompleteText.text = "Level Complete";
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void Update()
    {
        if (player.position.y < mapBottomLimit)
        {
            playerScript.PlayerDeath();
            UpdateDeathDictionary("Fall");
        }

        // For testing
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerController.playerSpawned = false;
            RestartLevel();
        }
    }
}
