using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class LaserGunController : MonoBehaviour, IWeapon
{
    public GameObject projectile;
    public Transform parent;
    public Transform start;
    public float lifetime;
    public bool laserTailFollow = false; // will tail of laser follow tip of gun barrel
    public GunEffectManager stats { get; set; }
    public BasicAmmoManager ammo { get; set; }

    EventManager em;

    public void OnActivation(Vector3 Target)
    {
        if (!stats.stats.fireRate.isAvailable) { return; }
        if (stats.stats.reloadSpeed.isAvailable && ammo.isReloadable && stats.stats.reloadable && ammo.Current <= 0) { ExecuteEvents.Execute<IWeaponMessages>(em.gameObject, null, (x, y) => x.Reload(null)); return; }
        if (ammo.Current <= 0) { return; }

        GameObject newProjectile = GameObject.Instantiate(projectile);
        newProjectile.transform.position = start.position;
        newProjectile.transform.SetParent(parent);
        Vector2 dir = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-stats.stats.bloom, stats.stats.bloom)) * ((Vector2)Target - (Vector2)newProjectile.transform.position).normalized;

        var controller = newProjectile.GetComponent<LaserProjectileController>();
        controller.target = dir;
        controller.damage = stats.stats.damage;
        if (laserTailFollow) controller.gunShootPos = start;
        controller.lifetime = new(lifetime);
        Destroy(newProjectile, lifetime);
        stats.stats.fireRate.Reset();
        ammo.OnActivation();
        Util.CallEvent<IPlayerMessages>((x, y) => x.OnPlayerMainAttack(transform.parent.parent));
    }

    // Start is called before the first frame update
    void Start()
    {
        em = Util.GetEventManager();
        stats = GetComponent<GunEffectManager>();
        em.registerEvent(EventGroup.Weapon, gameObject);
        ammo = GetComponent<BasicAmmoManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
