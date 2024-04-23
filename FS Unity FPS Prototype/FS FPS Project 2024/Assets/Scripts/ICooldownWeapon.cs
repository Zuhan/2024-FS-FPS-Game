using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICooldownWeapon
{
    float FireCooldown { get; }
    float LastFireTime { get; }
}