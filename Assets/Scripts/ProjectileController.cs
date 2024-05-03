using UnityEngine;
public class ProjectileController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // TODO: Fix this
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
