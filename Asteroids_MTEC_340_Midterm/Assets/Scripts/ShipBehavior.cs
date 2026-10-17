using UnityEngine;

public class ShipBehavior : MonoBehaviour
{
    public static ShipBehavior instance;
    private float[] _direction = new float[] {0.0f, 0.0f};
    public static Vector3 _shipLocation = new Vector3(0.0f, 0.0f, 0.0f);
    private SpriteRenderer _spriteRenderer;
    private Color32 _invincibleColor = new Color32(0, 255, 255, 255);

    [SerializeField] private float _speed = 3.0f;

    [SerializeField] private KeyCode _leftDirection = KeyCode.A;
    [SerializeField] private KeyCode _rightDirection = KeyCode.D;
    [SerializeField] private KeyCode _upDirection = KeyCode.W;
    [SerializeField] private KeyCode _downDirection = KeyCode.S;

    public static bool _isInvincible;

    private Rigidbody2D _rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        _rb.linearVelocityX = _direction[0] * _speed;
        _rb.linearVelocityY = _direction[1] * _speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isInvincible)
        {
            //Debug.Log("player is invincible");
            _spriteRenderer.color = _invincibleColor;
        } else {
            _spriteRenderer.color = Color.white;
        }
        _rb.simulated = GameBehavior.Instance.State == Utilities.GameState.Play;
        _shipLocation = gameObject.transform.position;
        _direction[0] = 0.0f;
        _direction[1] = 0.0f;
        if (GameBehavior.Instance.State == Utilities.GameState.Play)
        {
            if (Input.GetKey(_rightDirection))
            {
                _direction[0] += 1.0f;
            }
            if (Input.GetKey(_leftDirection))
            {
                _direction[0] -= 1.0f;
            }
            if (Input.GetKey(_upDirection))
            {
                _direction[1] += 1.0f;
            }
            if (Input.GetKey(_downDirection))
            {
                _direction[1] -= 1.0f;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_isInvincible && collision.gameObject.CompareTag("Asteroid"))
        {
            GameBehavior.Instance.GameRestart();
        }
    }
}
