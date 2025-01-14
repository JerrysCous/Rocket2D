using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XPManager : MonoBehaviour
{
    [SerializeField] private float maxXP = 100f;
    [SerializeField] private Slider xpSlider;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text currentLevelText; // New text field for the current level
    [SerializeField] private GameObject powerUpPanel;
    [SerializeField] private Button[] powerUpButtons;
    [SerializeField] private TMP_Text[] powerUpDescriptions;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private List<PowerUpSO> allPowerUps;

    private float currentXP = 0f;
    private int playerLevel = 1;

    private void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        UpdateXPBar();
        UpdateLevelText();
        UpdateCurrentLevelText();
        ResetPowerUpsToLevel1();
        powerUpPanel.SetActive(false);
    }

    private void ResetPowerUpsToLevel1()
    {
        foreach (PowerUpSO powerUp in allPowerUps)
        {
            powerUp.ResetToLevel1();
        }
    }

    public void AddXP(float xpAmount)
    {
        currentXP += xpAmount;

        if (currentXP >= maxXP)
        {
            LevelUp();
        }

        UpdateXPBar();
    }

    private void UpdateXPBar()
    {
        xpSlider.value = currentXP / maxXP;
    }

    private void UpdateLevelText()
    {
        levelText.text = $"Level: {playerLevel}";
    }

    private void UpdateCurrentLevelText()
    {
        currentLevelText.text = $"Current level: {playerLevel}";
    }

    private void LevelUp()
    {
        currentXP = 0f;
        playerLevel++;
        maxXP *= 1.2f; // increase the xp amount needed to level up for the next level

        UpdateLevelText();
        UpdateXPBar();
        UpdateCurrentLevelText();
        ShowPowerUpOptions();
    }

    private void ShowPowerUpOptions()
    {
        Time.timeScale = 0f;
        playerMovement.canMove = false;

        powerUpPanel.SetActive(true);

        // pick random power ups, excluding those that are max level
        List<PowerUpSO> randomPowerUps = GetRandomPowerUpsExcludingMaxLevel(powerUpButtons.Length);

        for (int i = 0; i < powerUpButtons.Length; i++)
        {
            if (i < randomPowerUps.Count)
            {
                PowerUpSO powerUp = randomPowerUps[i];
                powerUpDescriptions[i].text = $"{powerUp.Name} (Level {powerUp.CurrentLevel + 1})\n{powerUp.Description}";
                powerUpButtons[i].image.sprite = powerUp.Icon;
                powerUpButtons[i].image.enabled = true;

                powerUpButtons[i].onClick.RemoveAllListeners();
                powerUpButtons[i].onClick.AddListener(() => {
                    powerUp.ApplyEffect(playerMovement.gameObject);
                    powerUp.LevelUp();
                    ClosePowerUpPanel();
                });
            }
            else
            {
                // disable button if no power up available (they reached max lvl)
                powerUpButtons[i].image.enabled = false;
                powerUpButtons[i].onClick.RemoveAllListeners();
                powerUpDescriptions[i].text = "";
            }
        }
    }

    private List<PowerUpSO> GetRandomPowerUpsExcludingMaxLevel(int count)
    {
        List<PowerUpSO> eligiblePowerUps = new List<PowerUpSO>();

        foreach (var powerUp in allPowerUps)
        {
            if (!powerUp.IsMaxLevel)
            {
                eligiblePowerUps.Add(powerUp);
            }
        }

        // shuffle order of random power-ups using UnityEngine.Random
        eligiblePowerUps = eligiblePowerUps.OrderBy(x => UnityEngine.Random.value).ToList();

        return eligiblePowerUps.Take(count).ToList();
    }

    private List<PowerUpSO> GetRandomPowerUps(int count)
    {
        List<PowerUpSO> randomPowerUps = new List<PowerUpSO>();
        List<PowerUpSO> tempPool = new List<PowerUpSO>(allPowerUps);

        for (int i = 0; i < count; i++)
        {
            if (tempPool.Count == 0) break;
            int randomIndex = UnityEngine.Random.Range(0, tempPool.Count);
            randomPowerUps.Add(tempPool[randomIndex]);
            tempPool.RemoveAt(randomIndex);
        }

        return randomPowerUps;
    }

    private void ClosePowerUpPanel()
    {
        powerUpPanel.SetActive(false);
        Time.timeScale = 1f;
        playerMovement.canMove = true;
    }

    // Debug function to add XP manually for testing purposes
    public void DebugAddXP(float xpAmount)
    {
        AddXP(xpAmount);
    }

    // You can also bind this to a key or button in Update() for easy testing.
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {  // Press 'X' to add 50 XP for testing
            DebugAddXP(50f);
        }
    }
}
