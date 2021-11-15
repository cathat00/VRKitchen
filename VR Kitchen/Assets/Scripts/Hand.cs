using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Hand : MonoBehaviour
{
    Animator anim;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetTrigger(float v)
    {
        anim.SetFloat("Trigger", v);
    }

    public void SetGrip(float v)
    {
        anim.SetFloat("Grip", v);
    }

    public void grabbing()
    {
        // Make gameobject disappear
        anim.SetFloat("Grip", 1.048617f);
        anim.SetFloat("Trigger", 0.7901064f);
    }
}
