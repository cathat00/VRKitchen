using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stream : MonoBehaviour
{
    [SerializeField] LayerMask ignoreMask;

    public float litersPerSecond = .138f; // Liter output per second

    private LineRenderer lineRenderer = null;
    private ParticleSystem splashParticle = null;

    private Coroutine pourRoutine = null;
    private Vector3 targetPosition = Vector3.zero;

    private float speed = 1.75f;

    // Start is called before the first frame update
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        splashParticle = GetComponentInChildren<ParticleSystem>();
    }

    private void Start()
    {
        MoveToPosition(0, transform.position);
        MoveToPosition(1, transform.position);
    }

    public void Begin()
    {
        StartCoroutine(UpdateParticle());
        pourRoutine = StartCoroutine(BeginPour());
    }

    private IEnumerator BeginPour()
    {
        while(gameObject.activeSelf)
        {
            targetPosition = FindEndPoint();

            MoveToPosition(0, transform.position);
            AnimateToPosition(1, targetPosition);

            yield return null;
        }
    }

    public void End()
    {
        StopCoroutine(pourRoutine);
        pourRoutine = StartCoroutine(EndPour());
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

    private void TryToFill(GameObject obj)
    {
        GameObject container = null;
        LiquidContainer statLiq;

        if (obj.tag == "StaticLiquid") container = obj.transform.parent.gameObject; // A liquid's parent should be its container

        if (container != null && container.TryGetComponent<LiquidContainer>(out statLiq))
            statLiq.GetComponent<LiquidContainer>().AddToVolume(litersPerSecond * Time.deltaTime);
    }

    private Vector3 FindEndPoint()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);

        Physics.Raycast(ray, out hit, 2.0f, ~ignoreMask);
        Vector3 endPoint = hit.collider ? hit.point : ray.GetPoint(2.0f);

        GameObject obj = hit.collider.gameObject;
        TryToFill(obj);

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
