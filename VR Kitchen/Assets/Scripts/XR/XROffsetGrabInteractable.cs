using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/* NOTE: Red bug highlights are appearing, but the script works just fine. Not sure why this is happening. :/
 * 
 * XROffsetGrabInteractable is a subclass of OpenXR's XRGrabInteractable class that does not snap a "grabbed" object to the player's hand.
 * The player attaches to the object wherever they grab it, creating a slightly more realistic grab effect.
 * 
 */

public class XROffsetGrabInteractable : XRGrabInteractable
{
    void Start()
    {
        if (!attachTransform) // If there is no attach transform on the object, create one
        {
            GameObject grab = new GameObject("Grab Pivot");
            grab.transform.SetParent(transform, false);
            attachTransform = grab.transform;
        }
    }

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        if (interactor is XRDirectInteractor)
        {
            attachTransform.position = interactor.transform.position; // Set the attach transform's pos'n to wherever the player's hand is touching.
            attachTransform.rotation = interactor.transform.rotation; // Set the attach transform's rot'n to the rotation of the player's hand
        }
    }
}
