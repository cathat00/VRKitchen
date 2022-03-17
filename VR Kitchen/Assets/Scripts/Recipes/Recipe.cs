using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Recipe
{ 
    public string title;
    public RecipeTask[] tasks;

    [HideInInspector] public bool isStarted = false;
    [HideInInspector] public bool isComplete = false;

    public RecipeTask currentTask;
    private int taskIdx = 0;

    public void nextTask()
    {
        if (taskIdx < (tasks.Length - 1))
        {
            currentTask = tasks[++taskIdx];
        }
        else if (taskIdx >= (tasks.Length - 1) && !isComplete)
        {
            isComplete = true;
            Debug.Log("Recipe Complete!!");
        }
    }

    public void start()
    {
        isStarted = true;
        currentTask = tasks[0]; // Load current task with the first ask
    }
}
