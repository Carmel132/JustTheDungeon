using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noah
{
    public class AbilityPayload : IPlayerAbilityPayload
    {
        public Transform player { get; set; }
        public bool getShield = false;
        public GameObject Ability1Gun { get; set; }
        public GameObject Ability2Gun { get; set; }
        public GunManager gunManager { get; set; }
        public bool didRoll = false;
    }

    public class Abilities : IPlayerAbilities<AbilityPayload>
    {
        public IPassiveAbility<AbilityPayload> passive { get; set; } = new Passive();
        public IActiveAbility<AbilityPayload> active { get; set; } = new Active2();
        public IUltimateAbility<AbilityPayload> ultimate { get; set; }
    }

    public class Passive : IPassiveAbility<AbilityPayload>
    {
        public int shields = 0;
        const int MAX_SHIELDS = 3;

        public void OnActivation(AbilityPayload payload)
        {
            shields = Mathf.Clamp(shields + 1, 0, MAX_SHIELDS);
        }
    }

    public class Active1 : IActiveAbility<AbilityPayload>
    {
        public ICooldown cd { get; set; } = new TimeCooldown(10);
        GameObject gun;
        bool isActive = false;
        TimeCooldown duration = new(6);

        public void OnActivation(AbilityPayload payload)
        {
            if (!cd.isAvailable || isActive) { return; }
            gun = GameObject.Instantiate(payload.Ability1Gun, payload.player.GetChild(0));
            //gun.GetComponent<BasicGunController>().parent = payload.player.parent.GetChild(1);

            IWeapon weapon = Util.GetComponentThatImplements<IWeapon>(gun);
            if (weapon != null)
            {
                isActive = true;

                payload.gunManager.weapons.Add(weapon);
                payload.gunManager.LastWeapon();
                payload.gunManager.locked = true;
                payload.player.gameObject.GetComponent<PlayerController>().canMove = false;
                duration.Reset();
            }
        }

        public void Update(AbilityPayload payload)
        {
            if (payload.didRoll && isActive) { duration.Finish(); Debug.Log(duration.isAvailable); }
            if (isActive && (duration.isAvailable || gun.GetComponent<BasicAmmoManager>().Current == 0))
            {
                isActive = false;
                payload.gunManager.locked = false;
                payload.gunManager.FirstWeapon();
                payload.gunManager.weapons.RemoveAt(payload.gunManager.weapons.Count - 1);
                payload.gunManager.transform.parent.gameObject.GetComponent<PlayerController>().canMove = true;
                GameObject.Destroy(gun);
                cd.Reset();
            }
        }

        public void Start(AbilityPayload payload)
        {
            cd.Reset();
        }
    }

    public class Active2 : IActiveAbility<AbilityPayload>
    {
        public ICooldown cd { get; set; } = new TimeCooldown(4);
        GameObject gunPrefab;
        NoahGunController gunController;
        bool active = false;
        int gunIdx;
        public void OnActivation(AbilityPayload payload)
        {
            if (!cd.isAvailable) { return; }
            if (gunController != null)
            {
                if (payload.gunManager.Current() is NoahGunController controller)
                {
                    payload.gunManager.NextWeapon();
                }
                else
                {
                    payload.gunManager.SetWeaponIndex(gunIdx);
                }
            }
            else
            {
                if (active) 
                {
                    active = false;
                    cd.Reset(); 
                } 
                else
                {
                    GameObject gun = GameObject.Instantiate(gunPrefab, payload.player.GetChild(0));

                    IWeapon weapon = Util.GetComponentThatImplements<IWeapon>(gun);
                    payload.gunManager.weapons.Add(weapon);
                    payload.gunManager.LastWeapon();
                    gunController = gun.GetComponent<NoahGunController>();
                    gunIdx = payload.gunManager.GetWeaponIndex();
                    active = true;
                }
                
            }
        }

        public void Start(AbilityPayload payload)
        {
            cd.Reset();
            gunPrefab = payload.Ability2Gun;
            Debug.Log(payload.Ability2Gun);
        }
    }

}