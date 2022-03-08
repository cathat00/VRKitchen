using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RecipeTask
{
    public TaskType type;

    public string reqIngredient; // Ingredient required
    [SerializeField] private float requiredAmt;
    [SerializeField] private float currentAmt;

    [HideInInspector] public bool isComplete = false;

    private bool checkIfComplete() => (requiredAmt == currentAmt);

    public void addIngredient(string ingType, float amt)
    {
        if (ingType == reqIngredient) currentAmt += amt;
        isComplete = checkIfComplete();
    }

    public void removeIngredient(string ingType, float amt)
    {
        if (ingType == reqIngredient) currentAmt -= amt;
        isComplete = checkIfComplete();
    }
}

public enum TaskType
{
    Add,
    Cook
}
