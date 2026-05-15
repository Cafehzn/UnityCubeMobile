using UnityEngine;

public class GameRestart : MonoBehaviour
{
    [Header("Game Manager")]
    public GameManager _gameManager;

    public void Restart()
    {
        _gameManager.Enable();
        gameObject.SetActive(false);
    }
}
