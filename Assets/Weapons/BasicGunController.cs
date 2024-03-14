using System.Drawing;
using UnityEngine;
#nullable enable

// Vector3 for target
public class BasicGunController : MonoBehaviour, IWeapon
{
    public GameObject projectile;
    //TODO: Add 'lifetime' property to projectile controller
    public float lifetime;
    public Transform parent;
    public Transform start;
    public EventManager em;
    public BasicAmmoManager ammo;
    public GunEffectManager stats { get; set; }

    public void OnActivation(Vector3 target)
    {
        if (!stats.stats.fireRate.isAvailable) { return; }
        if (!stats.stats.reloadSpeed.isAvailable || ammo.Current <= 0) { Reload(null); return; }

        GameObject newProjectile = GameObject.Instantiate(projectile);
        newProjectile.transform.position = start.position;
        newProjectile.transform.SetParent(parent);
        Vector2 dir = Quaternion.Euler(0, 0, Random.Range(-stats.stats.bloom, stats.stats.bloom)) * ((Vector2)target - (Vector2)newProjectile.transform.position).normalized * stats.stats.speed;
        newProjectile.transform.right = dir - (Vector2)newProjectile.transform.position;
        newProjectile.GetComponent<Rigidbody2D>().AddForce(dir);
        Destroy(newProjectile, lifetime);
        stats.stats.fireRate.Reset();
        ammo.OnActivation();
    }

    // Start is called before the first frame update
    void Start()
    {
        em = Util.GetEventManager();
        transform.parent.parent.GetComponent<PlayerBasicAttack>().ability = this;
        stats = GetComponent<GunEffectManager>();
        em.registerEvent(EventGroup.GunStats, gameObject);
        ammo = GetComponent<BasicAmmoManager>();
    }

    public void Reload(in object? payload) 
    {
        if (stats.stats.reloadSpeed.isAvailable)
        {
            stats.stats.reloadSpeed.Reset();
            ammo.Reload();
        }
    }
}
