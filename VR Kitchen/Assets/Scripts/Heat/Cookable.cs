using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cookable : Heatable
{
    [SerializeField] private Color rawColor;
    [SerializeField] private Color cookedColor;

    private Material mat;

    // Start is called before the first frame update
    private void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        mat.color = Color.Lerp(rawColor, cookedColor, currentTemp / maxTemp);
    }
}
