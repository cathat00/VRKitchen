using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeManager : MonoBehaviour
{
    public Recipe recipe;

    [SerializeField] TextMeshProUGUI uiTitle;
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
        if (uiTitle.text != recipe.title) uiTitle.text = recipe.title;

        if (recipe.isComplete)
        {
            uiBackground.color = bgCompleteColor;
        }
    }


}
