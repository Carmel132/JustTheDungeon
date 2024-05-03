using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LaserProjectileController : MonoBehaviour
{
    LineRenderer lineRenderer;
    public Vector3 target;
    public float damage;
    public Transform gunShootPos;
    RaycastHit2D hit;
    public TimeCooldown lifetime;
    Vector3[] positions;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lifetime.Reset();
        hit = Physics2D.BoxCast(transform.position, new(0.2f, 1), Vector2.Angle(transform.position, target), target, 50, 1 << 3 | 1<<7);
        positions = new Vector3[] { transform.position, (transform.position + target * (hit.collider != null/* && hit.collider.gameObject.CompareTag("Enemy") */? Vector2.Distance(transform.position, hit.point) + 0.1f : 20)) };
        Draw();
        // TODO: Remove when implemented recurring hitbox component
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                hit.collider.gameObject.GetComponent<EnemyHealth>().Damage(damage);
            }
        }
    }

    private void Draw()
    {
        lineRenderer.SetPositions(positions);
    }

    private void Update()
    {

        lineRenderer.widthMultiplier = 1 - lifetime.percentDone;
        if (gunShootPos != null)
        {
            Vector3 prevTailPos = lineRenderer.GetPosition(0);
            for (int i = 0; i < positions.Length; ++i)
            {
                positions[i] += gunShootPos.position - prevTailPos;
            }
            Draw();
        }
    }
}
