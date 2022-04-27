using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * This class defines a stream of spice (salt, pepper, basil, etc.) Unlike a liquid stream, spice streams do not use a line renderer for animation. Rather, spice streams 
 * use a particle system for animation. Animation for spice streams is handled through the basic update loop, as opposed to a coroutine (SEE LiquidStream class).
 * 
 * (SEE Stream class for documentation of base class functionality)
 */

public class SpiceStream : Stream
{
    [SerializeField] private GameObject particleObj; // Particle gameObject
    private ParticleSystem particleSys;

    private bool started; // Has the stream started? 

    void Awake()
    {
        ingType = GetComponent<Ingredient>().type;
        particleSys = particleObj.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (started) FindEndPoint();
    }

    public override void Begin()
    {
        started = true;
    }

    public override void End()
    {
        particleObj.SetActive(false);
        started = false;
    }

    protected override void TryToFill(GameObject obj)
    {
        GameObject container = null;
        LiquidContainer statLiq; // Container to be filled

        if (obj.tag == "StaticLiquid") container = obj.transform.parent.gameObject; // A liquid's parent should be its container

        if (container != null && container.TryGetComponent<LiquidContainer>(out statLiq)) // If the stream hit a liquid container -> fill the container
            statLiq.GetComponent<LiquidContainer>().AddSpice(unitsPerSecond * Time.deltaTime, ingType);
    }

    protected override Vector3 FindEndPoint()
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
            var main = particleSys.main; // Change the lifetime of the particles in the system, so that they don't fall through the object
            main.startLifetime = LifetimeToPoint(hit.point);
        }
        else endPoint = ray.GetPoint(10.0f);

        if (obj)
        {
            TryToFill(obj); // Try to fill the object hit, if an object was hit
        }

        return endPoint;
    }

    /* Lifetime to point calculates the amount of lifetime a particle needs to have to fall to a given point and no further.
     * It would make sense that this lifetime = distance of point from origin / speed of particle
     * HOWEVER, what works in this calculation, for some reason, is lifetime = distance of point from origin, for any speed!
     * I have no idea why this is the case... further research needs to be done... but for now, it works */
    private float LifetimeToPoint(Vector3 point)
    {
        float distance = particleObj.transform.position.y - point.y; // Distance between source of particle and endpoint
        // float speed = particleSys.main.startSpeed.constant; // Speed of particles
        return distance;
    }
}
