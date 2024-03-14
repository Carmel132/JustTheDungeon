using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Noah {
    public class Noah : MonoBehaviour, IPlayerMessages
    {
        public EventManager EM;
        Abilities ps = new Abilities();
        const int PASSIVEKILLSREQ = 50;
        int currentKillCount = 0;
        public GameObject ability1Gun;
        

        // Start is called before the first frame update
        void Start()
        {
            EM.registerEvent(EventGroup.Player, gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            ActiveUpdate();
        }
        public void OnPlayerKill(Transform player, Transform target)
        {
            currentKillCount++;
            if (currentKillCount >= PASSIVEKILLSREQ)
            {
                currentKillCount %= PASSIVEKILLSREQ;
                var payload = new AbilityPayload();
                payload.getShield = true; //TODO: Add damage interception
                ps.passive.OnActivation(payload);
            }
        }

        public void OnPlayerActiveAbility(Transform player)
        {
            AbilityPayload cap = new();

            cap.player = transform.parent;
            cap.Ability1Gun = ability1Gun;
            Debug.Log(transform.parent.childCount);
            cap.gunManager = transform.parent.GetChild(0).gameObject.GetComponent<GunManager>();

            ps.active.OnActivation(cap);
        }

        public void ActiveUpdate()
        {
            AbilityPayload cap = new();

            cap.gunManager = transform.parent.GetChild(0).gameObject.GetComponent<GunManager>();
            ps.active.Update(cap);
        }
    }
}
