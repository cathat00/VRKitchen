using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Oven dial utilizes the IDial base class. Oven dials can only have integer settings (1, 2, 3, etc.), thus the analog value of the IDial needs to be divided by
 * the IDial's snapRotationAmount.
 */

public class OvenDial : MonoBehaviour, IDial
{

    public int currentDialValue = 0; // Current oven setting / dial setting

    public void DialChanged(float analogDialValue, float snapRotationAmount)
    {
        currentDialValue = (int) analogDialValue / (int) snapRotationAmount; // Divide analogVal by snapRotAmt to get the oven setting indicated by the dial
    }

}
