using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PourDetector : MonoBehaviour
{

    public Transform origin;

    public bool isPouring;
    private bool pourCheck;

    [SerializeField] private GameObject streamPrefab;
    private Stream currentStream;

    [SerializeField] private float litersPerSecond = .138f;

    private void Awake()
    {
        pourCheck = isPouring;
    }

    // Update is called once per frame
    void Update()
    {
        if (pourCheck != isPouring)
        {
            if (isPouring)
            {
                StartPour();
            }
            else
            {
                EndPour();
            }
            pourCheck = isPouring;
        }
    }

    private void StartPour()
    {
        currentStream = CreateStream();
        currentStream.Begin();
    }

    private void EndPour()
    {
        currentStream.End();
        currentStream = null;
    }

    private Stream CreateStream()
    {
        GameObject streamObj = Instantiate(streamPrefab, origin.position, Quaternion.identity, transform);
        Stream stream = streamObj.GetComponent<Stream>();

        stream.litersPerSecond = litersPerSecond;

        return stream;
    }
}
