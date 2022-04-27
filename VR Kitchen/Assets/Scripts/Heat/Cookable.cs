using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * This class is used on objects that are cookable insofar that they change color as they heat up. This includes objects like solid ingredients (chicken, tofu, etc.)
 * The material color of the object is directly modified as the object heats up, linearly interpolating between rawColor and cookedColor
 */

public class Cookable : Heatable
{
    [SerializeField] private Color rawColor; // Raw material color of the cooked object
    [SerializeField] private Color cookedColor;

    private Material mat; // Object's material 

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        mat.color = Color.Lerp(rawColor, cookedColor, currentTemp / maxTemp); // Linearly interpolate between raw and cooked color, based on current temperature
    }
}
