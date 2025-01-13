using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseDamagePowerUp", menuName = "PowerUp/IncreaseDamage")]
public class IncreaseDamagePowerUp : PowerUpSO {
    public override void ApplyEffect(GameObject player) {
        base.ApplyEffect(player);
        //logic for increase dmg effect goes here
        Debug.Log("player damage increased");
    }
}
