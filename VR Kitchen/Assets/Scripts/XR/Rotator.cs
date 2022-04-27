using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace LevelUP.Dial
{
    /* 
     * A rotator is an object, like a stove dial or a doorknob, that the player can grab and rotate. 
     */

    public class Rotator : MonoBehaviour
    {
        [SerializeField] Transform linkedDial;
        [SerializeField] private int snapRotationAmount = 25; // Rotational increment
        [SerializeField] private float angleTolerance; // Amount that the player has to rotate their hand in order to rotate the object by snapRotationAmount

        [SerializeField] private bool rotationConstraints = false; // Max / min rotation values
        [SerializeField] private float maxRotations = 0; // Max rotation value, if rotationContstraints are enabled

        private XRBaseInteractor interactor; // Player's hand

        private float startAngle;
        private bool requiresStartAngle = true;
        private bool shouldGetHandRotation = false;
        private XRGrabInteractable grabInteractor => GetComponent<XRGrabInteractable>(); // Grab interactable attached to this object

        private void OnEnable() // When the object is enabled...
        {
            grabInteractor.selectEntered.AddListener(GrabbedBy); // Begin listening for player hand interaction
            grabInteractor.selectExited.AddListener(GrabEnd); // Prepare listener for ending the interaction
        }
        private void OnDisable() // When the object is disabled...
        {
            grabInteractor.selectEntered.RemoveListener(GrabbedBy); // Shutdown listeners
            grabInteractor.selectExited.RemoveListener(GrabEnd);
        }

        private void GrabEnd(SelectExitEventArgs arg0)
        {
            shouldGetHandRotation = false; // Stop listening to the rotation of the player's hand
            requiresStartAngle = true;
        }

        private void GrabbedBy(SelectEnterEventArgs arg0) // When grabbed, find and store player's hand in interactor
        {
            interactor = GetComponent<XRGrabInteractable>().selectingInteractor; // Store the player's hand in interactor
            interactor.GetComponent<XRDirectInteractor>().hideControllerOnSelect = true;

            shouldGetHandRotation = true; // Start listening to the rotation of the player's hand
            startAngle = 0f; // Reset angle of rotation
        }

        void Update()
        {
            if (shouldGetHandRotation)
            {
                var rotationAngle = GetInteractorRotation(); // Gets the current controller angle
                GetRotationDistance(rotationAngle);
            }
        }

        public float GetInteractorRotation() => interactor.GetComponent<Transform>().eulerAngles.z;

        #region TheMath!
        private void GetRotationDistance(float currentAngle)
        {
            if (!requiresStartAngle)
            {
                var angleDifference = Mathf.Abs(startAngle - currentAngle); 

                if (angleDifference > angleTolerance) // If player has rotated hand past angleTolerance...
                {
                    if (angleDifference > 270f) // Checking to see if the user has gone from 0-360 - a very tiny movement but will trigger the angletolerance
                    {
                        float angleCheck;

                        if (startAngle < currentAngle) // Clockwise
                        {
                            angleCheck = CheckAngle(currentAngle, startAngle);

                            if (angleCheck < angleTolerance)
                                return;
                            else
                            {
                                if (isValidClockWiseRotation()) // Check if valid rotation
                                {
                                    RotateDialClockwise(); // Rotate
                                    startAngle = currentAngle;
                                }
                            }
                        }
                        else if (startAngle > currentAngle) // Anticlockwise
                        {
                            angleCheck = CheckAngle(currentAngle, startAngle);

                            if (angleCheck < angleTolerance)
                                return;
                            else
                            {
                                if (isValidAntiClockWiseRotation()) // Check if valid rotation
                                {
                                    RotateDialAntiClockwise(); // Rotate
                                    startAngle = currentAngle;
                                }
                            }
                        }
                    }
                    else // If angleDifference <= 270f
                    {
                        if (startAngle < currentAngle) // Anticlockwise
                        {
                            if (isValidAntiClockWiseRotation()) // Check if valid rotation
                            {
                                RotateDialAntiClockwise(); // Rotate
                                startAngle = currentAngle;
                            }
                        }
                        else if (startAngle > currentAngle) // Clockwise
                        {
                            if (isValidClockWiseRotation()) // Check if valid rotation
                            {
                                RotateDialClockwise(); // Rotate
                                startAngle = currentAngle;
                            }
                        }
                    }
                }
            }
            else // Rotation just began and the dial requires a starting angle
            {
                requiresStartAngle = false;
                startAngle = currentAngle;
            }
        }
        #endregion

        private float CheckAngle(float currentAngle, float startAngle) => (360f - currentAngle) + startAngle;

        private void RotateDialClockwise()
        {
            linkedDial.localEulerAngles = new Vector3(linkedDial.localEulerAngles.x, 
                                                      linkedDial.localEulerAngles.y, 
                                                      linkedDial.localEulerAngles.z + snapRotationAmount);

            if (TryGetComponent<IDial>(out IDial dial)) // Send rotation information to IDial
                dial.DialChanged(linkedDial.localEulerAngles.z, snapRotationAmount); // The different dial positions: 0 -> 0, 25 -> 1, 50 -> 2, etc.
        }

        private bool isValidClockWiseRotation()
        {
            if (rotationConstraints)
                return (linkedDial.localEulerAngles.z + snapRotationAmount) <= (maxRotations * snapRotationAmount);
            else
                return true;
        }

        private void RotateDialAntiClockwise()
        {
            linkedDial.localEulerAngles = new Vector3(linkedDial.localEulerAngles.x, 
                                                      linkedDial.localEulerAngles.y, 
                                                      linkedDial.localEulerAngles.z - snapRotationAmount);

            if(TryGetComponent<IDial>(out IDial dial)) // Sned rotation information to IDial
                dial.DialChanged(linkedDial.localEulerAngles.z, snapRotationAmount); // The different dial positions: 0 -> 0, 25 -> 1, 50 -> 2, etc.
        }

        private bool isValidAntiClockWiseRotation()
        {
            if (rotationConstraints)
                return (linkedDial.localEulerAngles.z - snapRotationAmount) >= 0;
            else
                return true;
        }

    }
}
