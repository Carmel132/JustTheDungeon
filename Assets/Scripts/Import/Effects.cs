using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffect
{
    public ICooldown duration { get; set; }
    public bool remove { get { return duration.isAvailable; } }

    public void Update(Transform t) { }
    public void Start(Transform t) { }
    public void OnRemove(Transform t) { }
    public void Add(Transform t, IEffect effect) { }
}

public class StunEffect : IEffect
{
    public ICooldown duration { get; set; }
    public void Start(Transform t)
    {
        duration.Reset();
        if (t.gameObject.CompareTag("Player"))
        {

        }
        else if (t.gameObject.CompareTag("Enemy"))
        {
            var controller = t.GetComponent<BasicEnemyController>();
            controller.enabled = false;
        }
    }

    public void OnRemove(Transform t)
    {
        duration.Reset();
        if (t.gameObject.CompareTag("Player"))
        {

        }
        else if (t.gameObject.CompareTag("Enemy"))
        {
            var controller = t.GetComponent<BasicEnemyController>();
            controller.enabled = true;
        }
    }

    public StunEffect(float d)
    {
        duration = new TimeCooldown();
        duration.duration = d;
    }
}

public class AttackSpeedEffect : IEffect
{
    public ICooldown duration { get; set; } = new TimeCooldown();
    public float multiplier;
    private WeaponStats stats;
    public AttackSpeedEffect(float d, float m, ref WeaponStats stats)
    {
        this.stats = stats;
        duration.duration = d;
        multiplier = m;
    }

    public void Start(Transform t)
    {
        duration.Reset();
        //t.GetComponent<PlayerBasicAttack>().cd.duration *= multiplier;
    }

    public void OnRemove(Transform t)
    {
        //t.GetComponent<PlayerBasicAttack>().cd.duration /= multiplier;
    }

    public void Add(Transform t, AttackSpeedEffect effect)
    {
        OnRemove(t);
        multiplier *= effect.multiplier;
        Start(t);
    }
}