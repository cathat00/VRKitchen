using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeManager : MonoBehaviour
{
    public Recipe recipe;

    [SerializeField] TextMeshProUGUI uiTitle;
    [SerializeField] TextMeshProUGUI uiTaskText;
    [SerializeField] Image uiBackground;

    [SerializeField] Color bgCompleteColor;
    [SerializeField] Color textCompleteColor;

    private void Update()
    {
        if (!recipe.isStarted) recipe.start();
        if (recipe.currentTask.isComplete) recipe.nextTask();

        UpdateUI();
    }

    private void UpdateUI()
    {
        // Check if UI already updated, to avoid having to update entire canvas each loop
        if (uiTaskText.text != GetTaskText()) uiTaskText.text = GetTaskText();
        if (uiTitle.text != recipe.title) uiTitle.text = recipe.title;

        if (recipe.isComplete)
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
