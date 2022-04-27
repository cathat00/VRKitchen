using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class defines a single task within a recipe. This task is completed when the required amount of a certain ingredient has been added to the recipe.
 * 
 * The TaskType variable is really only used to make the UI output a bit cleaner (see RecipeManager class for more). There is no functional difference in the 
 * completion conditions of the different task types.
 */

[System.Serializable]
public class RecipeTask
{
    public TaskType type;

    public string reqIngredient; // Ingredient required
    [SerializeField] public float requiredAmt; // Amount of ingredient that needs to be added
    [HideInInspector] public float currentAmt; 
    public string unitOfMeas; // Units used to measure ingredient -> liters, grams, tsps, etc. 

    [HideInInspector] public bool isComplete = false;

    private bool checkIfComplete() // Check if the taks is complete
    {
        return Mathf.Approximately(requiredAmt, (float)Mathf.Round(currentAmt * 100f) / 100f); // Rounded to two decimal points to account for floating point error
    }

    public void addAmt(string ingType, float amt) // ADD ingredient to current amount IF it is of the required type
    {
        if (ingType == reqIngredient) currentAmt += amt;
        isComplete = checkIfComplete(); // Check for completion
    }

    public void removeAmt(string ingType, float amt) // SUBTRACT ingredient from current amount IF it is of the required type
    {
        if (ingType == reqIngredient) currentAmt -= amt;
        isComplete = checkIfComplete(); // Check for completion
    }

    public void setAmt(string ingType, float amt) { // SET current amount to given amount IF ingredient specified is of type required
        if (ingType == reqIngredient) currentAmt = amt;
        isComplete = checkIfComplete(); // Check for completion
    }
}

public enum TaskType
{
    Add,
    HeatOverTime
}
