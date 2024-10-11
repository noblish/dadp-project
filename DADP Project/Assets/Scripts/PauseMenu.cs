using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public static bool PausedGame;
    public GameObject PauseMenuUI;
    
    // Start is called before the first frame update
    void Start()
    {
        PauseMenuUI.SetActive(false);
        PausedGame = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        var playerInput = new Controls();
        playerInput.Player.Enable();
    
        playerInput.Player.Pause.performed += ctx => PauseGame();
    }

    public void PauseGame()
    {
        if (PausedGame)
        {
            PauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            PausedGame = false;
            Debug.Log("UNPAUSED");
        }
        else
        {
            PauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            PausedGame = true;
            Debug.Log("PAUSED");
        }
    }
}
