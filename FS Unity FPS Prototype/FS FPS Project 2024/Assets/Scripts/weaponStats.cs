using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu]

public class weaponStats : ScriptableObject
{
    public GameObject weaponModel;
    public int castDamage;
    public float castRate;
    public int castDist;
    public int cooldown;

    public ParticleSystem hitEffect;
    public AudioClip castSound;
    [Range(0, 1)] public float castSoundVol;
}