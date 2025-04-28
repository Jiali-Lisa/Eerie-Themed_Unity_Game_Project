using UnityEngine;
using UnityEngine.SceneManagement;

public class MazeTrigger : MonoBehaviour
{

    [SerializeField] private GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            SceneManager.LoadScene("Maze");

        }
    }
}
