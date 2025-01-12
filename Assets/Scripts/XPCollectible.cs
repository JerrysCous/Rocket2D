using UnityEngine;

public class XPCollectible : MonoBehaviour {
    [SerializeField] private float xpAmount = 10f;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            XPManager xpManager = collision.GetComponent<XPManager>();
            if (xpManager != null) {
                xpManager.AddXP(xpAmount);
                Destroy(gameObject);
            }
        }
    }
}
