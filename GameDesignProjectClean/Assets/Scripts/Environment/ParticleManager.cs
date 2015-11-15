﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleManager : Singleton<ParticleManager> {
    public GameObject smallExplosion;
    public GameObject largeExplosion;
    public GameObject pulse;

    void Start()
    {

    }

    void Update()
    {
    }

    public void SpawnParticle(ParticleType type, Vector3 pos)
    {
        GameObject system;
        switch(type)
        {
            case ParticleType.SmallExplosion:
                system = smallExplosion;
                break;
            case ParticleType.LargeExplosion:
                system = largeExplosion;
                break;
            case ParticleType.Pulse:
                system = pulse;
                break;
            default:
                system = smallExplosion;
                break;
        }

        GameObject.Instantiate(system, pos, Quaternion.identity);
    }
}

public enum ParticleType
{
    SmallExplosion = 0,
    LargeExplosion = 1,
    Pulse = 2
};