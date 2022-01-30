using UnityEngine;

public class BarrelObstacle : MonoBehaviour
{
    private void FixedUpdate()
    {
        // Destroys the barrel if the level has not started to remove clutter
        if (!GameManager.Instance.LevelStarted)
        {
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject, 5f);
        }
    }

    // Apply a slowing effect when the barrel is collided with
    private void OnCollisionEnter(Collision collision)
    {
        // Only AI players can be affected by barrels
        GameObject other = collision.gameObject;
        if (!other.CompareTag("AI")) return;
        
        other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Destroy(gameObject);
    }
}
