using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    public GameObject pauseMenuHolder;

    private bool isPaused = false;

    private void Start() {
        pauseMenuHolder.SetActive(false);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) {
                ResumeGame();
            }
            else {
                PauseGame();
            }
        }
    }

    void PauseGame() {
        isPaused = true;
        pauseMenuHolder.SetActive(true);
        Time.timeScale = 0f;
    }

    void ResumeGame() {
        isPaused = false;
        pauseMenuHolder.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnResumeButtonClicked() {
        ResumeGame();
    }

    public void OnSettingsButtonClicked() {
        // implement settings menu functionality here
        Debug.Log("settings button clicked");
    }

    public void OnQuitButtonClicked() {
        Debug.Log("quit button clicked");
        Application.Quit();
    }
}
