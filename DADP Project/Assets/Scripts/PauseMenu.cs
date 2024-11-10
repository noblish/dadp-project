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
    private void OnEnable()
    {
        var playerInput = new Controls();
        playerInput.Player.Enable();
    
        playerInput.Player.Pause.performed += ctx => PauseGame();
    }

    public void PauseGame()
    {
            GameState currentGameState = GameStateManager.Instance.CurrentGameState;
            GameState newGameState = currentGameState == GameState.Gameplay
                ? GameState.Paused
                : GameState.Gameplay;
 
            GameStateManager.Instance.SetState(newGameState);
            
            if (GameStateManager.Instance.CurrentGameState == GameState.Paused)
            {
                PauseMenuUI.SetActive(true);
            }
            else
            {
                PauseMenuUI.SetActive(false);
            }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
