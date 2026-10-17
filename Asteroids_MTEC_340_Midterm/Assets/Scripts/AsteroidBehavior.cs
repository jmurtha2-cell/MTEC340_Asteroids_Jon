using UnityEngine;
using System.Collections;

public class AsteroidBehavior : MonoBehaviour
{
    //[SerializeField] private GameObject _gameObject;
    [SerializeField] float _lifespan = 50.0f;
    [SerializeField] float _speed = 5.0f;
    Vector3 currentPlayerLocation = ShipBehavior._shipLocation;
    Vector3 direction;
    private Rigidbody2D _rb;

    private void Start()
    {
        Vector3 asteroidStartPosition = new Vector3(transform.position.x, transform.position.y);
        _rb = GetComponent<Rigidbody2D>();
        StartCoroutine(lifespan());
        direction.x = currentPlayerLocation.x - asteroidStartPosition.x;
        direction.y = currentPlayerLocation.y - asteroidStartPosition.y;
        direction.Normalize();
        _rb.linearVelocity = direction * _speed;
    }

    private void Update()
    {
        _rb.simulated = GameBehavior.Instance.State == Utilities.GameState.Play;
    }

    private IEnumerator lifespan()
    {
        float timeRemaining = _lifespan;
        while (timeRemaining > 0)
        {
            if (GameBehavior.Instance.State == Utilities.GameState.Play)
            {
                timeRemaining -= Time.deltaTime;
            }
            yield return null;
        }
        Destroy(gameObject);
        yield break;
    }
}
