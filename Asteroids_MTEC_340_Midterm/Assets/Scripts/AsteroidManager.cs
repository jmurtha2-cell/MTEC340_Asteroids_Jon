using UnityEngine;
using System.Collections;

public class AsteroidManager : MonoBehaviour
{
    [SerializeField] public static AsteroidManager Instance;
    [Header("Asteroid Spawn Settings")] 
    [SerializeField] private GameObject _asteroidPrefab;
    [SerializeField] private Transform _asteroidParent;
    [SerializeField] private Transform[] _edgeLocations;

    [Header("Asteroid Spawn Rate Manipulation Range")] 
    [SerializeField] private float _minSpawnDelay;
    [SerializeField] private float _maxSpawnDelay;

    private GameObject _asteroid;
    private float checkRadius = 0.01f;
    [SerializeField] private int objectLayer;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float currentDelay = Random.Range(_minSpawnDelay, _maxSpawnDelay);

            yield return new WaitForSeconds(currentDelay);

            if(GameBehavior.Instance.State == Utilities.GameState.Play)
            {
                SpawnObject();
            }
        }
    }

    private void SpawnObject()
    {
        Transform randomEdge = _edgeLocations[Random.Range(0, _edgeLocations.Length)];
        float randomEdgeXLocation = Random.Range(randomEdge.position.x - randomEdge.localScale.x / 2, randomEdge.position.x + randomEdge.localScale.x / 2);
        float randomEdgeYLocation = Random.Range(randomEdge.position.y - randomEdge.localScale.y / 2, randomEdge.position.y + randomEdge.localScale.y / 2);
        Transform currentSpawnPoint = new GameObject("NewPoint").transform;
        currentSpawnPoint.position = new Vector3(randomEdgeXLocation, randomEdgeYLocation, 0);

        if (!ObjectIsThere(currentSpawnPoint))
        {
            _asteroid = Instantiate(_asteroidPrefab, currentSpawnPoint.position, Quaternion.identity, _asteroidParent);
        }
        Destroy(currentSpawnPoint.gameObject);
    }

    private bool ObjectIsThere(Transform location)
    {
        Collider2D hit = Physics2D.OverlapCircle(location.position, checkRadius, objectLayer);
        bool objectThere = (hit != null);
        //Debug.Log(objectThere);
        return objectThere;
    }

    public void ResetAsteroids()
    {
        for (int i = _asteroidParent.childCount - 1; i >= 0; i--)
        {
            GameObject child = _asteroidParent.GetChild(i).gameObject;
            Destroy(child);
        }
    }
}
