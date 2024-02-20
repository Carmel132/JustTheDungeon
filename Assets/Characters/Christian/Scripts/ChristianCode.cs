using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Christian
{
    public class AbilityPayload : IPlayerAbilityPayload
    {
        public HashSet<EnemyHealth> enemiesInEffect { get; set; }
        public Transform player { get; set; }
        public GameObject Ability1Proj { get; set; }
        public EventManager em { get; set; }
    }
    public class Abilities : IPlayerAbilities<AbilityPayload>
    {
        public IPassiveAbility<AbilityPayload> passive { get; set; } = new Passive();
        public IActiveAbility<AbilityPayload> active { get; set; } = new Active2();
        public IUltimateAbility<AbilityPayload> ultimate { get; set; } = new Ultimate();
    }
    /// <summary>
    /// Waking Up to Ash and Dust
    /// </summary>
    public class Passive : IPassiveAbility<AbilityPayload>
    {
        ICooldown cd = new TimeCooldown();
        float Damage = 10;

        public void Start(AbilityPayload t)
        {
            cd.duration = 10;
            cd.Reset();
        }

        public void Update(AbilityPayload t)
        {
            if (cd.isAvailable)
            {
                Debug.Log("Available");
                foreach (var enemy in t.enemiesInEffect)
                {
                    enemy.Damage(Damage);
                }
                cd.Reset();
            }
        }
    }
    /// <summary>
    /// Pooch Pistol
    /// </summary>
    public class Active1 : IActiveAbility<AbilityPayload>
    {
        public ICooldown cd { get; set; } = new ChargeCooldown(2);
        float Damage = 50;
        float dR = 30f;
        float maxRadius = 10;
        float stunDuration = 10f;

        public void Start(AbilityPayload t)
        {
            cd.duration = 3;
            cd.Reset();
        }

        public void OnActivation(AbilityPayload t)
        {
            if (cd.isAvailable)
            {
                var p = GameObject.Instantiate(t.Ability1Proj, t.player.parent.parent.Find("Projectiles"));
                p.transform.position = t.player.position + new Vector3(0, 0, 0.01f);
                var l = p.GetComponent<ChristianAbility1ProjController>();
                l.maxRadius = maxRadius;
                l.dR = dR;
                var h = p.GetComponent<Hitbox>();
                //h.Effects.Add(new StunEffect(stunDuration));
                h.Damage = Damage;
                cd.Reset();
            }
        }
    }

    public class Active2 : IActiveAbility<AbilityPayload>
    {
        public ICooldown cd { get; set; } = new TimeCooldown();

        public void Start(AbilityPayload t)
        {
            cd.duration = 3;
            cd.Reset();
        }

        public void OnActivation(AbilityPayload t)
        {
            if (cd.isAvailable)
            {
                ExecuteEvents.Execute<IGunStatMessages>(t.em.gameObject, null, (x, y) => x.AddStatChange((GunEffectManagerTarget.FIRERATE, new EffectFactor(0.5f, 0), new TimeCooldown(8f))));
                ExecuteEvents.Execute<IGunStatMessages>(t.em.gameObject, null, (x, y) => x.AddStatChange((GunEffectManagerTarget.RELOADSPEED, new EffectFactor(0.5f, 0), new TimeCooldown(8f))));
                ExecuteEvents.Execute<IGunStatMessages>(t.em.gameObject, null, (x, y) => x.AddStatChange((GunEffectManagerTarget.BLOOM, new EffectFactor(1, 4), new TimeCooldown(8f))));
                ExecuteEvents.Execute<IPlayerStatMessages>(t.em.gameObject, null, (x, y) => x.AddStatChange((PlayerEffectTarget.MAXHP, new EffectFactor(1, 2), new TimeCooldown(8f))));
                ExecuteEvents.Execute<IPlayerStatMessages>(t.em.gameObject, null, (x, y) => x.AddStatChange((PlayerEffectTarget.HP, new EffectFactor(1, 2), new TimeCooldown(8f))));
                cd.Reset();
            }
        }
    }

    public class Ultimate : IUltimateAbility<AbilityPayload>
    {
        public float chargeRate { get; set; } = 1f;

        public float maxCharge { get; set; } = 3500f;

        public float currentCharge { get; set; } = 3500f;

        float Damage = 1000f;

        public static EnemyHealth[] GetRenderedEnemies() => UnityEngine.Object.FindObjectsOfType<EnemyHealth>(false).Where(E => E.GetComponentsInChildren<Renderer>(false).Any(r => r.isVisible) || E.GetComponentsInParent<Renderer>(false).Any(r => r.isVisible)).ToArray();

        public void OnActivation(AbilityPayload t)
        {
            if (currentCharge < maxCharge) { return; }

            var enemies = GetRenderedEnemies();
            foreach (var enemy in enemies)
            {
                enemy.Damage(Damage);
            }
            currentCharge = 0;
        }
    }

}
