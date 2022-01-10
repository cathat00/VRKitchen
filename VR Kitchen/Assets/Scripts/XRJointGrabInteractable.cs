using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XRJointGrabInteractable : XRGrabInteractable
{

    private Rigidbody rb;
    private FixedJoint joint;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        //base.OnSelectEntered(interactor);

        Debug.Log("connecting a joint!");
        if (interactor.GetComponent<Rigidbody>() != null)
        {
            transform.position = interactor.transform.position;
            transform.eulerAngles = interactor.transform.eulerAngles;

            rb.useGravity = false;
            //rb.isKinematic = true;

            joint = this.gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = interactor.GetComponent<Rigidbody>();
        }
    }

    protected override void Drop()
    {
        base.Drop();
        transform.parent = null;
        Destroy(this.GetComponent<FixedJoint>());
    }
}
