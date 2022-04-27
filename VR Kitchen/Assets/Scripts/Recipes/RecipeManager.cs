using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * The RecipeManager loads each recipe, loads each new recipe task, and dynamically updates the recipe UI as progress is made toward recipe completion
 */

public class RecipeManager : MonoBehaviour
{
    public Recipe recipe; // Current recipe being completed

    [SerializeField] TextMeshProUGUI uiTitle; // UI title text
    [SerializeField] TextMeshProUGUI uiTaskText; // UI task text
    [SerializeField] Image uiBackground;

    [SerializeField] Color bgCompleteColor; // Color background changes to when the recipe is complete
    [SerializeField] Color textCompleteColor; // Color text changes to when the recipe is complete

    private void Update()
    {
        if (!recipe.isStarted) recipe.start(); // Start the recipe
        if (recipe.currentTask.isComplete) recipe.nextTask(); // If the recipe's current task is finished, try to move to the next task

        UpdateUI(); // Update the UI with each frame
    }

    private void UpdateUI()
    { 
        if (uiTaskText.text != GetTaskText()) uiTaskText.text = GetTaskText(); // Check if UI already updated, to avoid having to update entire canvas each loop
        if (uiTitle.text != recipe.title) uiTitle.text = recipe.title;

        if (recipe.isComplete) // Check if recipe is completed, IF SO then update the UI accordingly
        {
            uiBackground.color = bgCompleteColor;
        }
    }

    private string GetTaskText()
    {
        string ing = recipe.currentTask.reqIngredient;
        float reqAmt = (float)Mathf.Round(recipe.currentTask.requiredAmt * 100f) / 100f; // Round to two decimal places
        float curAmt = (float)Mathf.Round(recipe.currentTask.currentAmt * 100f) / 100f; // Round to two decimal places
        string units = recipe.currentTask.unitOfMeas;

        return recipe.currentTask.type + " " + ing + " (" + curAmt + " " + units + " / " + reqAmt + " " + units + ")";
    }


}
