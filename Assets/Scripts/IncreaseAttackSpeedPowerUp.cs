using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseAttackSpeedPowerUp", menuName = "PowerUp/IncreaseAttackSpeed")]
public class IncreaseAttackSpeedPowerUp : PowerUpSO {
    public float attackSpeedBoostAmount;
    public override void ApplyEffect(GameObject player) {
        var attackComponent = player.GetComponentInChildren<KnifeController>();
        if (attackComponent != null) {
            attackComponent.speed += attackSpeedBoostAmount;
            Debug.Log($"{Name}: Decreased cooldown by {attackSpeedBoostAmount}");
        }
    }
}
