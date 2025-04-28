using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskManager : MonoBehaviour
{
    public TMP_Text taskListText;
    private string currentTask;

    void Start()
    {
        SetTask("Go to the classroom");
    }

    void Update()
    {
    }

    public void SetTask(string task)
    {
        currentTask = task;
        UpdateTaskList();
    }

    public void CompleteTask(string task)
    {
        if (currentTask == task)
        {
            currentTask = string.Empty;
            UpdateTaskList();
        }
    }

    private void UpdateTaskList()
    {
        taskListText.text = currentTask;
    }
}
