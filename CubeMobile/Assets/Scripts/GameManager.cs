using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance {  get; private set; }

    [Header("Spawn Objects")]
    [SerializeField] private GameObject obstaclePrefab;
    public float spawnInterval = 2f;
    public bool isGameOver = false;
    public float spawnY = 12f;
    public float spawnX = 8.5f;

    [Header("Maping Controller")]
    [SerializeField] private InputActionReference cancelAction;

    [Header("To Pause")]
    public GameObject pauseMenu;

    [Header("Points")]
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score = 0;
    private float timeScore = 0f;

    [Header("End Run")]
    public GameObject gameOverScreen;

    private void OnEnable()
    {
        cancelAction.action.Enable();
        //Event Register
        cancelAction.action.performed += OnCancel;
    }
    private void OnDisable()
    {
        //Event Remove
        cancelAction.action.performed -= OnCancel;
        cancelAction.action.Disable();
    }
    private void OnCancel(InputAction.CallbackContext context)
    {
        if (isGameOver) { return; }//Cannot pause after game over   

        //Debug.Log("ESC pressed");
        if(Time.timeScale == 0f)
        {
            StartCoroutine(ScaleTime(0f, 1f, 0.5f));
            pauseMenu.SetActive(false);
        }
        else if(Time.timeScale == 1f)
        {
            StartCoroutine(ScaleTime(1f, 0f, 0.5f));
            pauseMenu.SetActive(true);
        }
    }

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
    private void Start()
    {
        StartCoroutine(SpawnObstacle());
    }

    private void Update()
    {
        if (isGameOver)
        {
            return;//Stop Score
        }
        scoreText.gameObject.SetActive(true);
        ToPoint();
    }

    private IEnumerator SpawnObstacle()
    {
        while (!isGameOver)
        {
            var obstacleSpawn = Random.Range(1, 4);

            for(int i = 0; i < obstacleSpawn; i++)
            {
                var xPosition = Random.Range(-spawnX, spawnX);

                var damping = Random.Range(0f, 2f); //Air resistence

                var objObstacle =
                    Instantiate(obstaclePrefab, new Vector3(xPosition, spawnY, 0), Quaternion.identity);

                objObstacle.GetComponent<Rigidbody>().linearDamping = damping;
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private IEnumerator ScaleTime(float start, float end, float duration)
    {
        float lastTime = Time.realtimeSinceStartup;
        float timer = 0.0f;

        while (timer < duration)
        {
            //Interpol and suavization of time
            Time.timeScale = Mathf.Lerp(start, end, timer / duration);
            //Physics time consistence adjustiment
            Time.fixedDeltaTime = 0.02f * Time.deltaTime;

            timer += Time.realtimeSinceStartup - lastTime;
            lastTime = Time.realtimeSinceStartup;

            yield return null;
        }

        Time.timeScale = end;
        Time.fixedDeltaTime = 0.02f * Time.deltaTime;
     }
    private void ToPoint()
    {
        timeScore += Time.deltaTime;
        if(timeScore > 1)
        {
            score++;
            scoreText.text = "Score: " + score;
            timeScore = 0f;
        }
    }

    //Can be acessed by other scripts
    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void GameOver()
    {
        if (isGameOver)
        {
            gameOverScreen.SetActive(true);
            return;
        }
    }
}
