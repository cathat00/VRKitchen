using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RecipeBroadcaster is inherited by a class that needs to broadcast information to the recipe manager, such as an IngredientCollector or LiquidContainer

public class RecipeBroadcaster : MonoBehaviour
{
    [SerializeField] private bool broadcasting = true;
    [SerializeField] private RecipeManager recipeManager;

    protected void BroadcastAdd(string ing, float amt) {
        if (broadcasting) recipeManager.recipe.currentTask.addAmt(ing, amt);
    }

    protected void BroadcastSet(string ing, float amt) {
        if (broadcasting) recipeManager.recipe.currentTask.setAmt(ing, amt);
    }


    protected virtual void BroadcastRemove(string ing, float amt) {
        if (broadcasting) recipeManager.recipe.currentTask.removeAmt(ing, amt);
    }
}
