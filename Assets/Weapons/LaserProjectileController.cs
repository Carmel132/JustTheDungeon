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
        lineRenderer.SetPositions(new Vector3[]{transform.position, (transform.position + target * 20)} );
        Physics2D.queriesHitTriggers = true;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, target, 50, 1<<3);
        Physics2D.queriesHitTriggers = false;
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
