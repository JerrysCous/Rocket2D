using UnityEngine;
using UnityEngine.UI;

public class XPManager : MonoBehaviour {
    [SerializeField] private float maxXP = 100f;
    [SerializeField] private Slider xpSlider;
    [SerializeField] private Text levelText;
    [SerializeField] private float currentXP = 0f;
    [SerializeField] private int playerLevel = 1;

    private void Start() {
        UpdateXPBar();
        UpdateLevelText();
    }

    public void AddXP(float xpAmount) {
        currentXP += xpAmount;
        Debug.Log($"Player gained {xpAmount} xp. Current xp: {currentXP}");

        if (currentXP >= maxXP) {
            LevelUp();
        }

        UpdateXPBar();
    }

    private void UpdateXPBar() {
        xpSlider.value = currentXP / maxXP;
    }

    private void UpdateLevelText() {
        levelText.text = $"Level: {playerLevel}";
    }

    private void LevelUp() {
        currentXP = 0;
        playerLevel++;
        Debug.Log($"Player leveled up, current level: {playerLevel}");

        UpdateLevelText();
        UpdateXPBar();
    }
}
