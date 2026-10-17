using UnityEngine;
using System.Collections;

public class LaserBehavior : MonoBehaviour
{
    [SerializeField] float _lifespan = 50.0f;
    [SerializeField] float _speed = 5.0f;
    Vector3 currentPlayerLocation = ShipBehavior._shipLocation;
    Vector3 direction = Vector3.zero;
    Vector3 mouseScreenPos;
    private Rigidbody2D _rb;
    private Transform _transform;

    void Start()
    {
        _transform = GetComponent<Transform>();
        mouseScreenPos = Input.mousePosition;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = _transform.position.z;
        direction = mouseWorldPos - _transform.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        _transform.rotation = Quaternion.Euler(0.0f, 0.0f, -angle);
        direction.Normalize();
        Vector3 laserStartPosition = new Vector3(currentPlayerLocation.x, currentPlayerLocation.y);
        _rb = GetComponent<Rigidbody2D>();
        StartCoroutine(lifeSpan());
        _rb.linearVelocity = direction * _speed;
        
    }

    void Update()
    {
        _rb.simulated = GameBehavior.Instance.State == Utilities.GameState.Play;
    }

    private IEnumerator lifeSpan()
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
            GameBehavior.Instance.Score += 50;
        }
    }
}
