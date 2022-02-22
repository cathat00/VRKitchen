using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatableContainer : Heatable
{

    [SerializeField] private float boilingTemp;

    private ParticleSystem smokeParticles;

    private void Start()
    { 
        smokeParticles = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        smokeParticles.gameObject.SetActive(currentTemp >= boilingTemp);
    }

}
