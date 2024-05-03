using System;
using UnityEngine;
using UnityEngine.EventSystems;
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
    public BasicAmmoManager ammo { get; set; }
    public GunEffectManager stats { get; set; }
    public WeaponAnimation weaponAnimation {get;set;}

    public AttackInputManagers.IAttackInputManager attackInputManager { get; set; }
    public void OnActivation(Vector3 target)
    {
        if (!stats.stats.fireRate.isAvailable) { return; }
        if (stats.stats.reloadSpeed.isAvailable && ammo.isReloadable && stats.stats.reloadable && ammo.Current <= 0) { ExecuteEvents.Execute<IWeaponMessages>(em.gameObject, null, (x, y) => x.Reload(null)); return; }
        if (ammo.Current <= 0) { return; }
        GameObject newProjectile = GameObject.Instantiate(projectile);
        newProjectile.transform.position = start.position;
        newProjectile.transform.SetParent(parent);
        Vector2 dir = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-stats.stats.bloom, stats.stats.bloom)) * ((Vector2)target - (Vector2)newProjectile.transform.position).normalized * stats.stats.speed;
        newProjectile.transform.right = dir - (Vector2)newProjectile.transform.position;
        newProjectile.GetComponent<Rigidbody2D>().AddForce(dir);
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
        if (GetComponent<WeaponStats>().hasWeaponAnimations) { weaponAnimation = GetComponent<WeaponAnimation>(); }
        attackInputManager = new AttackInputManagers.AutomaticAttack();
    }
}
