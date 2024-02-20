using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Christian
{
    public class Christian : MonoBehaviour, IPlayerMessages
    {
        public GameObject Ability1Proj;
        public EventManager EM;
        Abilities ps = new Abilities();

        HashSet<EnemyHealth> EnemiesInEffect = new HashSet<EnemyHealth>();
        // Start is called before the first frame update
        void Start()
        {
            transform.parent.GetComponent<BasicPlayerController>().ps = ps.stats;

            AbilityStart();
            EM.registerEvent(EventGroup.Player, gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            AbilityUpdate();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {

            if (collision.gameObject.CompareTag("Enemy"))
            {
                EnemiesInEffect.Add(collision.gameObject.GetComponent<EnemyHealth>());
            }
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                EnemiesInEffect.Remove(collision.gameObject.GetComponent<EnemyHealth>());
            }
        }

        void AbilityUpdate()
        {
            var cap = new AbilityPayload();
            cap.enemiesInEffect = EnemiesInEffect;
            cap.em = EM;
            ps.passive.Update(cap);
            ps.active.Update(cap);
            //ps.ultimate.Update(cap);
        }

        void AbilityStart()
        {
            var cap = new AbilityPayload();
            cap.em = EM;
            ps.passive.Start(cap);
            ps.active.Start(cap);
        }

        public void OnPlayerActiveAbility(Transform t)
        {
            var cap = new AbilityPayload();
            cap.Ability1Proj = Ability1Proj;
            cap.player = transform;
            cap.em = EM;

            ps.active.OnActivation(cap);
        }

        public void PlayerChargeUlt(float damage)
        {
            ps.ultimate.Charge(damage);
        }

        public void OnPlayerUltimateAbility(Transform t)
        {
            var cap = new AbilityPayload();
            ps.ultimate.OnActivation(cap);
        }
    }
}