using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IAbility
{ 

};

interface IPassiveAbility : IAbility
{

}

interface IActiveAbility : IAbility
{

}


struct PlayerStats
{
    uint Speed;
    uint HP;
    IPassiveAbility passive;
    IActiveAbility active;
}

public class BasicPlayerController : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
