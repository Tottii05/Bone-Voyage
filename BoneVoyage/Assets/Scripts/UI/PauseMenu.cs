using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static Player;

public class PauseMenu : MonoBehaviour, IPauseActions
{
    public Canvas pauseMenuUI;
    private Player playerActions;
    private bool isPaused = false;

    private void Start()
    {
        pauseMenuUI.enabled = false;
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        if (isPaused)
        {
            pauseMenuUI.enabled = false;
            Time.timeScale = 1f;
            isPaused = false;
        }
    }

    public void Pause()
    {
        if (!isPaused)
        {
            pauseMenuUI.enabled = true;
            Time.timeScale = 0f;
            isPaused = true;
        }
    }
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pauseMenuUI.enabled = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToWorldMap()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pauseMenuUI.enabled = false;
        SceneManager.LoadScene("WorldMap");
    }
}
