using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class is used for any streams of LIQUID, as opposed to spice streams. It derives from the base class of Stream and uses Line Renderer to animate a stream head and tail. 
 */

public class LiquidStream : Stream
{

    [SerializeField] private float speed = 1.75f; // Speed at which the stream animates

    private Color color;

    private LineRenderer lineRenderer = null;
    private ParticleSystem splashParticle = null;

    private Coroutine pourRoutine = null;
    private Vector3 targetPosition = Vector3.zero;

    // Start is called before the first frame update
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        splashParticle = GetComponentInChildren<ParticleSystem>();

        color = GetComponent<Renderer>().material.color;
        ingType = GetComponent<Ingredient>().type;
    }

    private void Start() // Set line renderer at start point for the animation
    {
        MoveToPosition(0, transform.position);
        MoveToPosition(1, transform.position);
    }

    public override void Begin() // Override of the parent class Begin -> updated to begin pour animation on instantiation
    {
        StartCoroutine(UpdateParticle());
        pourRoutine = StartCoroutine(BeginPour());
    }

    public override void End() // Override of the parent class Begin -> updated to end pour animation
    {
        StopCoroutine(pourRoutine);
        pourRoutine = StartCoroutine(EndPour());
    }

    private IEnumerator BeginPour() // Begin pour animation. Animate the head of the stream down to the endpoint
    {
        while(gameObject.activeSelf)
        {
            targetPosition = FindEndPoint(); // End point to which the stream should fall

            MoveToPosition(0, transform.position);
            if (lineRenderer.GetPosition(1).y > targetPosition.y) AnimateToPosition(1, targetPosition); // Fall to point lower in Y than current position
            else lineRenderer.SetPosition(1, targetPosition); // Rise to point higher in Y than current position, DO NOT animate to this point

            yield return null;
        }
    }

    private IEnumerator EndPour() // End the pour animation by bringing the tail of the stream down to the stream's endpoint
    {
        while(!HasReachedPosition(0, targetPosition))
        {
            AnimateToPosition(0, targetPosition);
            AnimateToPosition(1, targetPosition);
            yield return null;
        }
        Destroy(gameObject);
    }

    protected override void TryToFill(GameObject obj)
    {
        GameObject container = null;
        LiquidContainer statLiq; // Container to be filled

        if (obj.tag == "StaticLiquid") container = obj.transform.parent.gameObject; // A liquid's parent should be its container

        if (container != null && container.TryGetComponent<LiquidContainer>(out statLiq)) // If the stream hit a liquid container -> fill the container
            statLiq.GetComponent<LiquidContainer>().AddLiquid(unitsPerSecond * Time.deltaTime, ingType, color);
    }

    protected override Vector3 FindEndPoint()
    {
        Vector3 endPoint;
        RaycastHit hit;
        GameObject obj = null; // Object hit by the stream -> stream will attempt to fill this object

        Ray ray = new Ray(transform.position, Vector3.down);

        float rayLength = (transform.position.y - lineRenderer.GetPosition(1).y) + .5f; // Length of stream + .5 to check for collision

        Physics.Raycast(ray, out hit, rayLength, ~ignoreMask);

        if (hit.collider)
        {
            endPoint = hit.point; // Stops at point of collision
            obj = hit.collider.gameObject;
        }
        else endPoint = ray.GetPoint(10.0f);

        if (obj) TryToFill(obj); // Try to fill the object hit, if an object was hit

        return endPoint;
    }

    private void MoveToPosition(int idx, Vector3 targetPosition) // Move line renderer to given position WITHOUT ANIMATION
    {
        lineRenderer.SetPosition(idx, targetPosition);
    }

    private void AnimateToPosition(int idx, Vector3 targetPosition) // Move line renderer to given position WITH ANIMATION
    {
        Vector3 currentPoint = lineRenderer.GetPosition(idx);
        Vector3 newPosition = Vector3.MoveTowards(currentPoint, targetPosition, Time.deltaTime * speed);
        lineRenderer.SetPosition(idx, newPosition);
    }

    private bool HasReachedPosition(int idx, Vector3 targetPosition) // Check if the stream head has reached given pos'n 
    {
        Vector3 currentPosition = lineRenderer.GetPosition(idx);
        return currentPosition == targetPosition;
    }

    private IEnumerator UpdateParticle() // Turn the splash particle effect ON / OFF, depending on position of stream head
    {
        while (gameObject.activeSelf)
        {
            splashParticle.gameObject.transform.position = targetPosition;

            bool isHitting = HasReachedPosition(1, targetPosition);
            splashParticle.gameObject.SetActive(isHitting);

            yield return null;
        }
    }
}
