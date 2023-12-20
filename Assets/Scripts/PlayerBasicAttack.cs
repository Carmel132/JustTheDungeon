using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICooldown
{
    bool isAvailable { get; set; }
    float duration { get; set; }

    public void Reset();
}

public class TimeCooldown : ICooldown
{
    float last;
    public float duration { get; set; }
    public bool isAvailable { get { return (Time.time - last) > duration; } set { isAvailable = value; } }

    public void Reset()
    {
        last = Time.time;
    }

}

public class ChargeCooldown : ICooldown
{
    public bool isAvailable { get { return getCharges() > 0; } set { isAvailable = value; } }
    int maxCharges = 0;

    float last;

    int getCharges()
    {
        return (int)Mathf.Clamp(Mathf.Floor((Time.time - last) / duration), 0, maxCharges);
    }

    public ChargeCooldown(int max, int start=0)
    {
        maxCharges = max;
        last -= start * duration;
    }

    public float duration { get; set; }

    public void Reset()
    {
        if (isAvailable)
        {   
            if (getCharges() == maxCharges)
            {
                last = Time.time - duration * (maxCharges - 1);
            }
            else
            {
                last += duration;
            }
            
        }
        else
        {
            last = Time.time;
        }
    }
}

public interface IBasicAbility<T1, T2> : IAbility<T2>
{
    void Activate(T1 args);
    ICooldown cd { get; set; }
}

public interface IGunBasicAbilityInfo
{
    public ICooldown cd { get; set; }
    public IBasicProjectileInfo proj { get; set; }
}

public class GunBasicAbilityInfo : IGunBasicAbilityInfo
{
    public ICooldown cd { get; set; }
    public IBasicProjectileInfo proj { get; set; }
    public GunBasicAbilityInfo(ICooldown _cd, IBasicProjectileInfo _info)
    {
        cd = _cd;
        proj = _info;
    }
}

public class GunAbilityInfo : IBasicProjectileInfo
{
    public float projSpeed { get; set; }
    public Vector3 target { get; set; }
    public GameObject proj { get; set; }
    public float lifetime { get; set; }
    public float bloom { get; set; }
    public Transform parent { get; set; }
    public Vector3 start { get; set; }

    public GunAbilityInfo(float projSpeed, Vector3 target, Vector3 start, GameObject proj, float lifetime, float bloom, Transform parent)
    {
        this.projSpeed = projSpeed;
        this.target = target;
        this.proj = proj;
        this.lifetime = lifetime;
        this.bloom = bloom;
        this.parent = parent;
        this.start = start;
    }
}

public interface IBasicProjectileInfo
{
    public float projSpeed { get; set; }
    public Vector3 target { get; set; }
    public GameObject proj { get; set; }
    public float lifetime { get; set; }
    public float bloom { get; set; }
    public Transform parent { get; set; }
    public Vector3 start { get; set; }
}

public class BasicProjectile
{
    IBasicProjectileInfo info;
    GameObject proj;
    public BasicProjectile(IBasicProjectileInfo _info)
    {
        info = _info;
    }

    public void Start()
    {
        proj = GameObject.Instantiate(info.proj);
        proj.transform.position = info.start;
        proj.transform.SetParent(info.parent);
        Vector2 dir = Quaternion.Euler(0, 0, Random.Range(-info.bloom, info.bloom)) * ((Vector2)info.target - (Vector2)proj.transform.position).normalized * info.projSpeed;
        Debug.Log((info.target - proj.transform.position).normalized);
        proj.GetComponent<Rigidbody2D>().AddForce(dir);
        GameObject.Destroy(proj, info.lifetime);
    }
}

public class GunBasicAbility : IBasicAbility<Vector3, IGunBasicAbilityInfo>
{
    public IGunBasicAbilityInfo info;
    //List<BasicProjectile> bullets = new List<BasicProjectile>();
    public ICooldown cd { get; set; }

    public void Activate(Vector3 target)
    {
        if (cd.isAvailable)
        {
            info.proj.target = target;
            var p = new BasicProjectile(info.proj);
            //bullets.Add(p);
            p.Start();
            cd.Reset();
        }
    }
    public void Start(IGunBasicAbilityInfo proj)
    {
        info = proj;
        cd = info.cd;
        //cd.Reset();
    }
}

public class PlayerBasicAttack : MonoBehaviour
{
    public GameObject proj;
    ICooldown cd = new TimeCooldown();
    public float speed;
    public float lifetime;
    public float bloom;
    public Transform projectileParent;
    public Transform start;
    IBasicAbility<Vector3, IGunBasicAbilityInfo> ability = new GunBasicAbility();

    // Start is called before the first frame update
    void Start()
    {
        cd.duration =0.1f;
        cd.Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (cd.isAvailable)
            {
                ability.Start(new GunBasicAbilityInfo(cd, new GunAbilityInfo(speed, Vector3.up, start.position, proj, lifetime, bloom, projectileParent)));
                ability.Activate(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
        }

    }
}
