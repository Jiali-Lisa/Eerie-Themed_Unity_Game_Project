using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string SceneName;

    public void Interact(GameObject player)
    {
        if (SceneName != null) {
            Debug.Log("Scene changed!");
            SceneManager.LoadScene(SceneName);
        } else {
            Debug.Log("Not connected to a scene!");
        }
    }

    public void InteractionOff() {}

    public string GetInteractText()
    {
        return "Press E to change scene";
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
