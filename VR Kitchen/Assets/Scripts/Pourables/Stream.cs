using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This is the base class for ANY GIVEN stream, encompassing both streams of spice and streams of liquid. Streams fall to a certain endpoint and try to "fill" objects they collide with.
 * Any given ingredient stream has an ingredient type, as represented by the ingType variable.
 */

public class Stream : MonoBehaviour
{
    [SerializeField] protected LayerMask ignoreMask; // The LayerMask determining which layers the stream raycast will ignore in searching for an endpoint
    public float unitsPerSecond = .138f; // Unit output per second. (Liters for liquids, grams for spices)

    protected string ingType;

    public virtual void Begin() // Virtual function to begin the stream
    {

    }

    public virtual void End() // Virtual function to end the stream
    {

    }

    protected virtual void TryToFill(GameObject obj) // The stream will try to "fill" any object it collides with 
    {

    }

    protected virtual Vector3 FindEndPoint() // The stream will fall to a given endpoint
    {
        Vector3 endPoint;
        RaycastHit hit;
        GameObject obj = null; // Object hit by the stream -> stream will attempt to fill this object

        Ray ray = new Ray(transform.position, Vector3.down);

        Physics.Raycast(ray, out hit, 10.0f, ~ignoreMask);

        if (hit.collider)
        {
            endPoint = hit.point; // Stops at point of collision
            obj = hit.collider.gameObject;
        }
        else endPoint = ray.GetPoint(10.0f);

        if (obj)
        {
            TryToFill(obj); // Try to fill the object hit, if an object was hit
        }

        return endPoint;
    }
}
