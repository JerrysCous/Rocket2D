using UnityEngine;

[CreateAssetMenu(fileName = "DecreaseProjectileCooldownPowerUp", menuName = "PowerUp/DecreaseProjectileCooldown")]
public class DecreaseProjectileCooldownPowerUp : PowerUpSO {
    public float projectileCooldownDecreaseAmount;
    public override void ApplyEffect(GameObject player) {
        var attackComponent = player.GetComponentInChildren<KnifeController>();
        if (attackComponent != null) {
            attackComponent.cooldownDuration -= projectileCooldownDecreaseAmount;
            Debug.Log($"{Name}: Decreased cooldown by {projectileCooldownDecreaseAmount}");
        }
    }
}