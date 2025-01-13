using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUp", menuName = "ScriptableObjects/PowerUp")]
public class PowerUpSO : ScriptableObject {
    [System.Serializable]
    public class PowerUpLevel {
        public string Name;
        public string Description;
        public Sprite Icon;
        public float EffectValue;
    }

    [SerializeField] private List<PowerUpLevel> levels = new List<PowerUpLevel>();
    [SerializeField] private int maxLevel = 3;

    public int CurrentLevel { get; private set; } = 0;

    public string Name => levels[CurrentLevel].Name;
    public string Description => levels[CurrentLevel].Description;
    public Sprite Icon => levels[CurrentLevel].Icon;
    public float EffectValue => levels[CurrentLevel].EffectValue;

    public bool IsMaxLevel => CurrentLevel >= maxLevel - 1;

    public virtual void ApplyEffect(GameObject player) {
        Debug.Log($"Applying {Name} effect {CurrentLevel + 1}: {Description}");
    }

    public void LevelUp() {
        if (CurrentLevel < maxLevel - 1) {
            CurrentLevel++;
        }
    }

    public void ResetToLevel1() {
        CurrentLevel = 0;
    }
}
