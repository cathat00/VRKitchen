using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stream : MonoBehaviour
{
    [SerializeField] protected LayerMask ignoreMask;
    public float unitsPerSecond = .138f; // Unit output per second. (Liters for liquids, grams for spices)

    protected string ingType;

    public virtual void Begin()
    {

    }

    public virtual void End()
    {

    }

    protected virtual void TryToFill(GameObject obj)
    {

    }

    protected virtual Vector3 FindEndPoint()
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
