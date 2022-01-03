using UnityEngine.XR.Interaction.Toolkit;

public interface IDial
{
    void DialChanged(float analogDialValue, float snapRotationAmount);
}