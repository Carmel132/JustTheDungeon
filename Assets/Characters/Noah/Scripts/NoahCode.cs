using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noah
{
    public class AbilityPayload : IPlayerAbilityPayload
    {
        public Transform player { get; set; }
        public bool getShield = false;
    }

    public class Abilities : IPlayerAbilities<AbilityPayload>
    {
        public IPassiveAbility<AbilityPayload> passive { get; set; } = new Passive();
        public IActiveAbility<AbilityPayload> active { get; set; }
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
}