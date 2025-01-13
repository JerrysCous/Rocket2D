using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseAttackSpeedPowerUp", menuName = "PowerUp/AttackSpeed")]
public class IncreaseAttackSpeedPowerUp : PowerUpSO {
    public override void ApplyEffect(GameObject player) {
        base.ApplyEffect(player);
        //logic for increase attack speed effect goes here
        Debug.Log("attack speed increased");
    }
}
