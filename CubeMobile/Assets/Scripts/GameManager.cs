using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab;
    public float spawnInterval = 2f;
    public bool isGameOver = false;
    public float spawnY = 12f;
    public float spawnX = 8.5f;

    [SerializeField] private InputActionReference cancelAction;

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
        //Debug.Log("ESC pressed");
        if(Time.timeScale == 0f)
        {
            StartCoroutine(ScaleTime(0f, 1f, 0.5f));
        }
        else if(Time.timeScale == 1f)
        {
            StartCoroutine(ScaleTime(1f, 0f, 0.5f));
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnObstacle());
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
}
