using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    public GridGenerator grid;
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject gameUI;

    // Start is called before the first frame update
    private void Awake()
    {
        mainMenu.SetActive(true);
        pauseMenu.SetActive(false);
        gameUI.SetActive(false);
        
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            grid.SetGamePaused(true);
            pauseMenu.SetActive(true);
            gameUI.SetActive(false);
        }
    }

    public void OnPlayButtonClicked()
    {
        mainMenu.SetActive(false);
        gameUI.SetActive(true);
        grid.SetGameActive(true);
    }

    public void OnOptionButtonClicked()
    {
        Debug.Log("Options button clicked");
    }

    public void OnHighScoreClicked()
    {
        Debug.Log("High score clicked");
    }

    public void OnResumeClicked()
    {
        Debug.Log("Resume clicked");
        grid.SetGamePaused(false);
        gameUI.SetActive(true);
        pauseMenu.SetActive(false);
  
    }
    public void OnReturnToMenu()
    {
        grid.SetGameActive(false);
        pauseMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}