using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidStream : Stream
{
    //[SerializeField] LayerMask ignoreMask;

    [SerializeField] private float speed = 1.75f;
    //public float unitsPerSecond = .138f; // Unit output per second. (Liters for liquids, grams for spices)

    //private string ingType;
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

    private void Start()
    {
        MoveToPosition(0, transform.position);
        MoveToPosition(1, transform.position);
    }

    public override void Begin()
    {
        StartCoroutine(UpdateParticle());
        pourRoutine = StartCoroutine(BeginPour());
    }

    public override void End()
    {
        StopCoroutine(pourRoutine);
        pourRoutine = StartCoroutine(EndPour());
    }

    private IEnumerator BeginPour()
    {
        while(gameObject.activeSelf)
        {
            targetPosition = FindEndPoint();

            MoveToPosition(0, transform.position);
            if (lineRenderer.GetPosition(1).y > targetPosition.y) AnimateToPosition(1, targetPosition); // Fall to point lower in Y than current position
            else lineRenderer.SetPosition(1, targetPosition); // Rise to point higher in Y than current position, DO NOT animate to this point

            yield return null;
        }
    }

    private IEnumerator EndPour()
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
        LiquidContainer statLiq;

        if (obj.tag == "StaticLiquid") container = obj.transform.parent.gameObject; // A liquid's parent should be its container

        if (container != null && container.TryGetComponent<LiquidContainer>(out statLiq))
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

    private void MoveToPosition(int idx, Vector3 targetPosition)
    {
        lineRenderer.SetPosition(idx, targetPosition);
    }

    private void AnimateToPosition(int idx, Vector3 targetPosition)
    {
        Vector3 currentPoint = lineRenderer.GetPosition(idx);
        Vector3 newPosition = Vector3.MoveTowards(currentPoint, targetPosition, Time.deltaTime * speed);
        lineRenderer.SetPosition(idx, newPosition);
    }

    private bool HasReachedPosition(int idx, Vector3 targetPosition)
    {
        Vector3 currentPosition = lineRenderer.GetPosition(idx);
        return currentPosition == targetPosition;
    }

    private IEnumerator UpdateParticle()
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
