using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public Recipe recipe;

    private void Update()
    {
        if (!recipe.isStarted) recipe.start();
        if (recipe.currentTask.isComplete) recipe.nextTask();
    }
}
