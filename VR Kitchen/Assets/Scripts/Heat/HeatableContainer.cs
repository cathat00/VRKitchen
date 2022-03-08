using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatableContainer : Heatable
{

    [SerializeField] private float particleTemp; // Temperature at which the container begins emitting particles
    [SerializeField] private Heater heater;

    private ParticleSystem particles;

    private void Start()
    { 
        particles = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        particles.gameObject.SetActive(currentTemp >= particleTemp);
        heater.currentTemp = currentTemp; // Heatable containers heat the objects they contain
    }

}
