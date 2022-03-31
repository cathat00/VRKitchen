using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace LevelUP.Dial
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] Transform linkedDial;
        [SerializeField] private int snapRotationAmount = 25;
        [SerializeField] private float angleTolerance;

        [SerializeField] private bool rotationConstraints = false;
        [SerializeField] private float maxRotations = 0;

        private XRBaseInteractor interactor;

        private float startAngle;
        private bool requiresStartAngle = true;
        private bool shouldGetHandRotation = false;
        private XRGrabInteractable grabInteractor => GetComponent<XRGrabInteractable>();

        private void OnEnable()
        {
            grabInteractor.selectEntered.AddListener(GrabbedBy);
            grabInteractor.selectExited.AddListener(GrabEnd);
        }
        private void OnDisable()
        {
            grabInteractor.selectEntered.RemoveListener(GrabbedBy);
            grabInteractor.selectExited.RemoveListener(GrabEnd);
        }

        private void GrabEnd(SelectExitEventArgs arg0)
        {
            shouldGetHandRotation = false;
            requiresStartAngle = true;
        }

        private void GrabbedBy(SelectEnterEventArgs arg0)
        {
            interactor = GetComponent<XRGrabInteractable>().selectingInteractor;
            interactor.GetComponent<XRDirectInteractor>().hideControllerOnSelect = true;

            shouldGetHandRotation = true;
            startAngle = 0f;
        }

        void Update()
        {
            if (shouldGetHandRotation)
            {
                var rotationAngle = GetInteractorRotation(); //gets the current controller angle
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

                if (angleDifference > angleTolerance)
                {
                    if (angleDifference > 270f) //checking to see if the user has gone from 0-360 - a very tiny movement but will trigger the angletolerance
                    {
                        float angleCheck;

                        if (startAngle < currentAngle) 
                        {
                            angleCheck = CheckAngle(currentAngle, startAngle);

                            if (angleCheck < angleTolerance)
                                return;
                            else
                            {
                                if (isValidClockWiseRotation())
                                {
                                    RotateDialClockwise();
                                    startAngle = currentAngle;
                                }
                            }
                        }
                        else if (startAngle > currentAngle) 
                        {
                            angleCheck = CheckAngle(currentAngle, startAngle);

                            if (angleCheck < angleTolerance)
                                return;
                            else
                            {
                                if (isValidAntiClockWiseRotation())
                                {
                                    RotateDialAntiClockwise();
                                    startAngle = currentAngle;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (startAngle < currentAngle)
                        {
                            if (isValidAntiClockWiseRotation())
                            {
                                RotateDialAntiClockwise();
                                startAngle = currentAngle;
                            }
                        }
                        else if (startAngle > currentAngle)
                        {
                            if (isValidClockWiseRotation())
                            {
                                RotateDialClockwise();
                                startAngle = currentAngle;
                            }
                        }
                    }
                }
            }
            else
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

            if (TryGetComponent<IDial>(out IDial dial))
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

            if(TryGetComponent<IDial>(out IDial dial))
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
