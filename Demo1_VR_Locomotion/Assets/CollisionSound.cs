using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    public AudioSource collisionSound;
    public float minImpactForce = 0.3f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hand")) return;

        if (collision.relativeVelocity.magnitude > minImpactForce && collisionSound != null)
        {
            collisionSound.Play();
        }
    }
}
