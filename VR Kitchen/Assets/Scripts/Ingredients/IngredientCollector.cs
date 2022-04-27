using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This script keeps track of all objects currently "contained" (colliding with) the object it is attached to and sends this info the recipe manager. */ 

public class IngredientCollector : RecipeBroadcaster
{
    private List<GameObject> containing = new List<GameObject>(); // List of all objects contained

    private void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject; // Object that has entered collison box
        addIngredient(obj);
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject obj = other.gameObject; // Object that has left collision box
        removeIngredient(obj);
    }

    private void addIngredient(GameObject obj) // Add object to list of those contained
    {
        if (obj.TryGetComponent<Ingredient>(out Ingredient ing))
        {
            containing.Add(obj);
            BroadcastAdd(ing.type, ing.amount); // Let the recipe system know about this change
        }
    }
    
    private void removeIngredient(GameObject obj) // Remove object from list of those contained
    {
        if (containing.Contains(obj))
        {
            Ingredient ing = obj.GetComponent<Ingredient>(); // Only an object of type Ingredient would be in the container, so this is safe
            containing.Remove(obj);
            BroadcastRemove(ing.type, ing.amount); // Let the recipe system know about this change
        }
    }
}
