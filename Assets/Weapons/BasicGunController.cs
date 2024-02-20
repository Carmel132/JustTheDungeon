using UnityEngine;

// Vector3 for target
public class BasicGunController : MonoBehaviour, IWeapon
{
    public GameObject proj;
    //TODO: Add 'lifetime' property to projectile controller
    public float lifetime;
    public Transform parent;
    public Transform start;
    public EventManager em;
    public GunEffectManager stats { get; set; }

    public void OnActivation(Vector3 target)
    {
        if (!stats.stats.fireRate.isAvailable) { return; }
        GameObject newproj = GameObject.Instantiate(proj);
        newproj.transform.position = start.position;
        newproj.transform.SetParent(parent);
        Vector2 dir = Quaternion.Euler(0, 0, Random.Range(-stats.stats.bloom, stats.stats.bloom)) * ((Vector2)target - (Vector2)newproj.transform.position).normalized * stats.stats.speed;
        newproj.transform.right = dir - (Vector2)newproj.transform.position;
        newproj.GetComponent<Rigidbody2D>().AddForce(dir);
        Destroy(newproj, lifetime);
        stats.stats.fireRate.Reset();
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.parent.parent.GetComponent<PlayerBasicAttack>().ability = this;
        stats = GetComponent<GunEffectManager>();
        em.registerEvent(EventGroup.GunStats, gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
