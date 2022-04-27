using UnityEngine.XR.Interaction.Toolkit;

/*
 * Interface by which a rotator can enact certain functionalities based on its rotation. 
 * Analog dial value represents the euler angle of the dial
 * SnapRotationAmount is the increment by which the dial rotates
 */

public interface IDial
{
    void DialChanged(float analogDialValue, float snapRotationAmount);
}