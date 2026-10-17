using UnityEngine;
using TMPro;
using System.Collections;

public class GameBehavior : MonoBehaviour
{
    [SerializeField] public static GameBehavior Instance;
    [SerializeField] private AsteroidManager _asteroidManager;

    private Utilities.GameState _state;

    [Header("Ship")]
    [SerializeField] private GameObject _shipPrefab;
    [SerializeField] private Transform _shipParent;
    private GameObject _ship;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private Transform _laserParent;
    [SerializeField] private float _coolDown;
    bool _isCoolingDown = false;

    [Header("Audio")]
    private AudioSource _source;
    [SerializeField] private AudioClip _deathSound;
    [SerializeField] private AudioClip _pauseSound;
    [SerializeField] private AudioClip _unPauseSound;

    public Utilities.GameState State
    {
        get => _state;

        set
        {
            _state = value;
            _pauseUI.enabled = State == Utilities.GameState.Pause;
        }
    }
    
    [Header("Pickup")]
    [SerializeField] private GameObject _powerUpPrefab;
    [SerializeField] private Transform _powerUpParent;
    [SerializeField] private float _powerUpDuration;
    private GameObject _powerUp;

    [SerializeField] private Vector2 _powerUpSpawnBounds = new Vector2(2.0f, 2.0f);
    [SerializeField, Range(0.000001f, 0.01f)] private float _powerUpSpawnProbability = 0.0001f;
    private Vector2 _prefabSize;

    [Header("Scoring")]
    [SerializeField] private float _passiveRate = 1.0f;

    // ---- Player Script ----
    [Header("UI")]
    [SerializeField] private TMP_Text _scoreUI;
    [SerializeField] private TMP_Text _pauseUI;
    private int _score;

    // Public variable
    public int Score
    {
        // get
        // {
        //     return _score * 10;
        // }

        get => _score;

        set
        {
            _score = value;
            // _scoreUI.text = Score.ToString();
            _scoreUI.SetText(Score.ToString());
        }

        // set => _score = value;
    }
    // -----------------------

    void Awake()
    {
        // Singleton pattern

        if (Instance == null)
        {
            Instance = this;
            Debug.Log("New instance initialized...");
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            Debug.Log("Duplicate instance found and deleted...");
        }
    }

    void Start()
    {
        _source = GetComponent<AudioSource>();
        SpawnShip();
        State = Utilities.GameState.Play;
        Score = 0;
        StartCoroutine(ScorePointsTimeSurvived());
    }
    
    private void Update()
    {
        // State Machine Transition
        if (Input.GetKeyDown(KeyCode.P))
        {
            State = State == Utilities.GameState.Play ? Utilities.GameState.Pause : Utilities.GameState.Play;
            if (State == Utilities.GameState.Pause)
            {
                _source.PlayOneShot(_pauseSound);
            } else {
                _source.PlayOneShot(_unPauseSound);
            }
        } 
        if (Input.GetMouseButtonDown(0) && State == Utilities.GameState.Play && !_isCoolingDown)
        {
            Debug.Log("Left Mouse Clicked - Shot fired");
            Instantiate(_laserPrefab, _ship.transform.position, Quaternion.identity, _laserParent);
            StartCoroutine(coolDown());
        }
        if (State == Utilities.GameState.Play)
        {
            if (!_powerUp && Random.value < _powerUpSpawnProbability)
            {
                Vector2 position = Vector2.zero;
                int counter = 0;
                int maxNumberTries = 30;
                do
                {
                    if (counter > maxNumberTries)
                    {
                        break;
                    }
                    position = new Vector2(
                        Random.Range(-_powerUpSpawnBounds.x, _powerUpSpawnBounds.x),
                        Random.Range(-_powerUpSpawnBounds.y, _powerUpSpawnBounds.y)
                    );
                    counter++;
                } while (Physics2D.OverlapBox(position, _prefabSize, 0.0f) && Physics2D.OverlapBox(position, _prefabSize, 45.0f));
                _powerUp = Instantiate(_powerUpPrefab, position, Quaternion.identity, _powerUpParent);
            }
            _pauseUI.enabled = false;
        } else {
            _pauseUI.enabled = true;
        }
    }

    public void PowerUp()
    {
        Score += 500;
        StartCoroutine(PowerUpTimer());
    }

    public IEnumerator PowerUpTimer()
    {
        if (ShipBehavior._isInvincible)
        {
            //Break out of coroutine from within the coroutine function
            yield break;
        }

        ShipBehavior._isInvincible = true;
        Debug.Log("is invincible = true");

        yield return new WaitForSeconds(_powerUpDuration);

        ShipBehavior._isInvincible = false;
        Debug.Log("is invincible = false");
    }

    public IEnumerator coolDown()
    {
        _isCoolingDown = true;
        float timeRemaining = _coolDown;
        while (timeRemaining > 0)
        {
            if (GameBehavior.Instance.State == Utilities.GameState.Play)
            {
                timeRemaining -= Time.deltaTime;
            }
            yield return null;
        }
        _isCoolingDown = false;
        yield break;
    }
    public void ScorePointsAsteroidDestroyed()
    {
        Score += 100;
    }

    public IEnumerator ScorePointsTimeSurvived()
    {        
        while (State == Utilities.GameState.Play){
            Score += 10;
            float timeRemaining = _passiveRate;
            while (timeRemaining > 0)
            {
                if (GameBehavior.Instance.State == Utilities.GameState.Play)
                {
                    timeRemaining -= Time.deltaTime; 
                }
                yield return null;
            }
        }
        yield break;
    }

    public void GameRestart()
    {
        Score = 0;
        Destroy(_ship);
        SpawnShip();
        _asteroidManager.ResetAsteroids();
        clearPowerUps();
        _source.PlayOneShot(_deathSound);
    }

    private void SpawnShip()
    {
        _ship = Instantiate(_shipPrefab, Vector3.zero, Quaternion.identity);
    }

    private void clearPowerUps()
    {
        for (int i = _powerUpParent.childCount - 1; i >= 0; i--)
        {
            GameObject child = _powerUpParent.GetChild(i).gameObject;
            Destroy(child);
        }
    }
}
