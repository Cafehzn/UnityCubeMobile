using UnityEngine;

public class Menu : MonoBehaviour
{
    [Header("GameManager")]
    public GameManager _gameManager;//Put Game Manager on inspector manually

    public void Play()
    {
        GetComponent<CanvasGroup>().LeanAlpha(0, 0.2f).setOnComplete(StartGame);
    }

    private void Start()
    {
        GetComponentInChildren<TMPro.TextMeshProUGUI>().
            gameObject.LeanScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f).setLoopPingPong();
    }

    private void StartGame()
    {
        _gameManager.Enable();
        Destroy(gameObject);
    }
}
