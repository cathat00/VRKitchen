using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RecipeTask
{
    public TaskType type;

    public string reqIngredient; // Ingredient required
    [SerializeField] public float requiredAmt;
    public float currentAmt;
    public string unitOfMeas;

    [HideInInspector] public bool isComplete = false;

    private bool checkIfComplete()
    {
        return Mathf.Approximately(requiredAmt, (float)Mathf.Round(currentAmt * 100f) / 100f); // Rounded to two decimal points to account for floating point error
    }

    public void addAmt(string ingType, float amt)
    {
        if (ingType == reqIngredient) currentAmt += amt;
        isComplete = checkIfComplete();
    }

    public void removeAmt(string ingType, float amt)
    {
        if (ingType == reqIngredient) currentAmt -= amt;
        isComplete = checkIfComplete();
    }

    public void setAmt(string ingType, float amt) {
        if (ingType == reqIngredient) currentAmt = amt;
        isComplete = checkIfComplete();
    }
}

public enum TaskType
{
    Add,
    Cook
}
