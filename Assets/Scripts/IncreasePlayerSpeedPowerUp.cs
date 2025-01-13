using UnityEngine;

[CreateAssetMenu(fileName = "SpeedBoostPowerUp", menuName = "PowerUp/SpeedBoost")]
public class IncreasePlayerSpeedPowerUp : PowerUpSO {
    public override void ApplyEffect(GameObject player) {
        base.ApplyEffect(player);
        //logic for increase player speed effect goes here
        Debug.Log("Player speed increased!");
    }
}
