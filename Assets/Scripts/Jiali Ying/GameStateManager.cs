using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    public Vector3 playerPosition;
    public Quaternion playerRotation;
    public bool hasSavedState = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SavePlayerState(Transform playerTransform)
    {
        playerPosition = playerTransform.position;
        playerRotation = playerTransform.rotation;
        hasSavedState = true;
    }

    public void RestorePlayerState(Transform playerTransform)
    {
        if (hasSavedState)
        {
            playerTransform.position = playerPosition;
            playerTransform.rotation = playerRotation;
        }
    }
}
