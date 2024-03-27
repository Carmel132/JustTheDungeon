using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectileController : MonoBehaviour
{
    LineRenderer lineRenderer;
    public Vector3 target;
    public float damage;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new(0.2f, 1), Vector2.Angle(transform.position, target), target, 50, 1 << 3);
        lineRenderer.SetPositions(new Vector3[] { transform.position, (transform.position + target * (hit.collider != null && hit.collider.gameObject.CompareTag("Enemy") ? Vector2.Distance(transform.position, hit.point) + 0.1f: 20))});
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("Hit!");
                //hit.collider.gameObject.GetComponent<EnemyHealth>().Damage(damage);
            }
        }
    }
}
