using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class represents an entire recipe, composed of individual tasks. At the outset, a recipe loads the first task. A recipe is completed when each of its tasks is completed.
 */

[System.Serializable]
public class Recipe
{ 
    public string title; // Title of the recipe -> "Tomato Soup," etc. 
    public RecipeTask[] tasks; // Array of all tasks

    [HideInInspector] public bool isStarted = false;
    [HideInInspector] public bool isComplete = false;

    public RecipeTask currentTask; // Current task being completed
    private int taskIdx = 0; // Index of the current task

    public void nextTask() // A task has been completed -> Move to the next one, or mark the recipe as complete 
    {
        if (taskIdx < (tasks.Length - 1)) // Are all tasks completed? 
        {
            currentTask = tasks[++taskIdx]; // Load the next task
        }
        else if (taskIdx >= (tasks.Length - 1) && !isComplete) // Check if all tasks have just been completed
        {
            isComplete = true;
            Debug.Log("Recipe Complete!!");
        }
    }

    public void start() // Start the recipe
    {
        isStarted = true;
        currentTask = tasks[0]; // Load current task with the first task
    }
}
