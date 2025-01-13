using UnityEngine;

[CreateAssetMenu(fileName = "IncreasePlayerSpeedPowerUp", menuName = "PowerUp/IncreasePlayerSpeed")]
public class IncreasePlayerSpeedPowerUp : PowerUpSO {
    public float speedBoostAmount;
    public override void ApplyEffect(GameObject player) {
        var playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement != null) {
            playerMovement.moveSpeed += speedBoostAmount;
            Debug.Log($"{Name}: Increased player speed by {speedBoostAmount}");
        }
    }
}
