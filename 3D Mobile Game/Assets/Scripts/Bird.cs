using UnityEngine;

public class Bird : MonoBehaviour
{

    private float speed = 5.0f;

    [Header("Settings")]

    public float minXSpawnCoord = -95.0f;
    public float maxXSpawnCoord = -55.0f;
    public float endCoord = 70.0f;

    public float minZCoord = -30.0f;
    public float maxZCoord = 30.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetBird();
    }

    // Update is called once per frame
    void Update()
    {
        // Move birds
        transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);

        // If the birds go too far off-scene, reset them
        if (transform.position.x > endCoord)
        {
            ResetBird();
        }
    }

    private void ResetBird()
    {
        // Get a random spawn pos so that they don'tr repeat the same path
        float randomZ = Random.Range(minZCoord, maxZCoord);
        float randomX = Random.Range(minXSpawnCoord, maxXSpawnCoord);

        transform.position = new Vector3(randomX, transform.position.y, randomZ);
    }
}
