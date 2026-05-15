using UnityEngine;

public class Menu : MonoBehaviour
{
    [Header("GameManager")]
    public GameManager _gameManager;//Put Game Manager on inspector manually

    public void Play()
    {
        _gameManager.Enable();

        Destroy(gameObject);
    }
}
