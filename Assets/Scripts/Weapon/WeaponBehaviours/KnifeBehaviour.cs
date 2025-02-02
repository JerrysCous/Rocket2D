using UnityEngine;

public class KnifeBehaviour : ProjectileWeaponBehaviour {
    private KnifeController kc;
    [SerializeField] private GameObject xpCollectiblePrefab;

    protected override void Start() {
        base.Start();
        kc = FindObjectOfType<KnifeController>();
        Destroy(gameObject, destroyAfterSeconds);
    }

    void Update() {
        transform.position += direction * kc.speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Enemy")) {
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null) {
                SpawnXPCollectible(enemy.transform.position);
                EnemyWaveSpawner.Instance.EnemyDefeated();
                Destroy(collision.gameObject);
            }

        }
    }

    private void SpawnXPCollectible(Vector2 position) {
        Instantiate(xpCollectiblePrefab, position, Quaternion.identity);
    }
}
