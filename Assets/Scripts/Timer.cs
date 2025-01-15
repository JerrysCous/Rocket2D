using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI GameTime;  // Reference to the GameTime TextMeshPro text component
    private float timeElapsed = 0f;   // Time in seconds

    void Start()
    {
        // Initialize the timer text to 00:00
        GameTime.text = "00:00";
        StartGame();
    }

    public float GetCurrentTime() {
        return timeElapsed;
    }

    public void SetTime(float time) {
        timeElapsed = time;
    }

    void Update()
    {
        // Increment the time regardless of game state
        timeElapsed += Time.deltaTime;

        // Format the time to minutes and seconds
        int minutes = Mathf.FloorToInt(timeElapsed / 60);
        int seconds = Mathf.FloorToInt(timeElapsed % 60);
        GameTime.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
    }

    // Call this function when the game starts
    public void StartGame()
    {
        timeElapsed = 0f;  // Reset time when the game starts
    }

    // Call this function when the game ends (optional)
    public void EndGame()
    {
        // If you want to do anything when the game ends, you can put logic here
        // but the timer will keep running in the background.
    }

    // Call this function when restarting the game
    public void RestartGame()
    {
        timeElapsed = 0f;
        GameTime.text = "00:00";
        StartGame();
    }
}

