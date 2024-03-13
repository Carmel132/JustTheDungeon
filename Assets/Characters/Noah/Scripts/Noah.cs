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

        // Start is called before the first frame update
        void Start()
        {
            EM.registerEvent(EventGroup.Player, gameObject);
        }

        // Update is called once per frame
        void Update()
        {
        
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
    }
}
