using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenDial : MonoBehaviour, IDial
{

    public int currentDialValue = 0;

    public void DialChanged(float analogDialValue, float snapRotationAmount)
    {
        currentDialValue = (int) analogDialValue / (int) snapRotationAmount;
    }

}
