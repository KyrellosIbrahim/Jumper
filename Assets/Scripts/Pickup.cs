using UnityEngine;

public class Pickup : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var jumper = other.GetComponent<JumperBehaviour>();
        if (jumper == null) return;

        jumper.increaseJumpHeight(3f);

        Destroy(gameObject);
    }
}
