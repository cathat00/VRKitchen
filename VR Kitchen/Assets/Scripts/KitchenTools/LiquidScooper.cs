using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Liquid Scooper class is for objects like spoons or ladles. Can be used to scoop up liquid. 
 * Can be brought to the player's mouth and eaten. */
public class LiquidScooper : MonoBehaviour
{
    [SerializeField] private GameObject liquid;
    private Renderer liqRenderer;

    private void Start()
    {
        liqRenderer = liquid.GetComponent<Renderer>();
        liqRenderer.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "StaticLiquid")
        {
            Debug.Log(other.gameObject);
            GameObject obj;
            LiquidContainer container;
            obj = other.transform.parent.gameObject; // A static liquid's parent object should be its container
            container = obj.GetComponent<LiquidContainer>();

            if (container.totalVolume > 0)
            {
                if (!liqRenderer.enabled) liqRenderer.enabled = true;
                liqRenderer.material.color = container.liqRenderer.material.color;
            }
        }
    }
}
