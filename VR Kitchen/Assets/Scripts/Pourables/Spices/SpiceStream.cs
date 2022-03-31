using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiceStream : Stream
{
    [SerializeField] private GameObject particleObj;
    private ParticleSystem particleSys;

    private bool started;

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
        LiquidContainer statLiq;

        if (obj.tag == "StaticLiquid") container = obj.transform.parent.gameObject; // A liquid's parent should be its container

        if (container != null && container.TryGetComponent<LiquidContainer>(out statLiq))
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
