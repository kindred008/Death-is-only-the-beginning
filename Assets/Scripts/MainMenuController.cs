using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] GameObject loadingUI;

    public void LoadLevel(int level)
    {
        loadingUI.SetActive(true);
        SceneManager.LoadSceneAsync(level);
    }
}
