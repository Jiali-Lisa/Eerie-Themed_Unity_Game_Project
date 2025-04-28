using UnityEngine;

public class NPCFollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform player; 

    void Update()
    {
        if (player != null)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0; 
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
