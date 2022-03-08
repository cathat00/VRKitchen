using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This script keeps track of all objects currently "contained" by the object it is attached to and sends this info the recipe manager. */ 

public class IngredientCollector : RecipeBroadcaster
{
    private List<GameObject> containing = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;
        addIngredient(obj);
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject obj = other.gameObject;
        removeIngredient(obj);
    }

    private void addIngredient(GameObject obj)
    {
        if (obj.TryGetComponent<Ingredient>(out Ingredient ing))
        {
            containing.Add(obj);
            BroadcastAdd(ing.type, ing.amount);
        }
    }
    
    private void removeIngredient(GameObject obj)
    {
        if (containing.Contains(obj))
        {
            Ingredient ing = obj.GetComponent<Ingredient>(); // Only an object of type Ingredient would be in the container, so this is safe
            containing.Remove(obj);
            BroadcastRemove(ing.type, ing.amount);
        }
    }
}
