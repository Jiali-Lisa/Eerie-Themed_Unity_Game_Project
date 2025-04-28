using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public enum TaskType {
    UNLOCK,
    MAIN,
}

public class ChasingTasksManager : MonoBehaviour
{
    public TMP_Text taskListText;
    private string currentTask;
    private Dictionary<TaskType, List<IInteractable>> tasks = new Dictionary<TaskType, List<IInteractable>>();

    void Start() {
        foreach (TaskType type in Enum.GetValues(typeof(TaskType))) {
            tasks.Add(type, new List<IInteractable>());
        }
    }

    public void AddTask(TaskType type, IInteractable obj)
    {
        switch (type) {
            case TaskType.UNLOCK:
                if (!tasks[type].Contains(obj)) tasks[type].Add(obj);
                break;
            case TaskType.MAIN:
                if (!tasks[type].Contains(obj)) tasks[type].Add(obj);
                break;
            default:
                break;
        }
        UpdateTaskList();
    }

    public void CompleteTask(TaskType type, IInteractable obj)
    {
        tasks[type].Remove(obj);
        UpdateTaskList();
    }

    private void UpdateTaskList()
    {
        if (tasks.Count > 0) {
            taskListText.text = string.Empty;
            foreach (KeyValuePair<TaskType, List<IInteractable>> task in tasks) {
                int count = tasks[task.Key].Count;
                switch (task.Key) {
                    case TaskType.UNLOCK:
                        if (count <= 0) break;
                        taskListText.text += "Unlock Door" + (count > 1 ? ("s " + "(" + count + ")") : "" ) + '\n';
                        break;
                    case TaskType.MAIN:
                        if (count <= 0) break;
                        taskListText.text += "Find Trigger" + (count > 1 ? ("s " + "(" + count + ")") : "" ) + '\n';
                        break;
                    default:
                        break;
                }
            }
            return;
        }
    }
}
