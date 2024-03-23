using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyHealth : MonoBehaviour
{

    public float HP = 100f;
    public EventManager EM;

    void Update()
    {
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player Projectile"))
        {
            Hitbox p = collision.gameObject.GetComponent<Hitbox>();
            if (p != null)
            {
                Damage(p.Damage);
                ExecuteEvents.Execute<IPlayerMessages>(EM.gameObject, null, (x, y) => x.PlayerChargeUlt(p.Damage));
            }
        }
    }

    public void Damage(float d)
    {
        HP -= d;
    }
}
