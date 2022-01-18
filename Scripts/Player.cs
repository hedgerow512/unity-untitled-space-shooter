using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Play Variables
    private int _score = 0;
    private bool _isDead = false;

    // External Script Managers
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private GameManager _gameManager;

    // Ship Variables
    [SerializeField] private float _speed = 8f;
    [SerializeField] private int _lives = 3;

    // Ship Skeleton Components
    private SpriteRenderer _shipSpriteRenderer;
    private SpriteRenderer _trailSpriteRenderer;
    private Collider2D _shipCollider;
    [SerializeField] private GameObject _explosionPrefab;

    // Laser Variables
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;

    // Powerup Variables
    private bool tripleShotIsActive = false;
    private bool speedIsActive = false;
    private bool shieldIsActive = false;
    private float _powerupTimerDuration = 8f;
    private float _speedBoost = 2;
    [SerializeField] private GameObject _shieldVisualizer;

    // Firing rate
    [SerializeField] private float _fireRate = 0.2f;
    private float _canFire = -1f;

    // Audio
    [SerializeField] private AudioClip _laserSFX;
    private AudioSource _audioSource;

    void Start() {
        _shipCollider = this.gameObject.GetComponent<Collider2D>();
        _shipSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        _trailSpriteRenderer = GameObject.Find("Thruster").GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();

        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        //Error Checks
        if (_spawnManager == null) {
            Debug.LogError("The Spawn Manager is NULL.");
        }
        if (_uiManager == null) {
            Debug.LogError("The UI Manager is NULL.");
        }
        if (_audioSource == null) {
            Debug.LogError("AudioSource on the player is NULL.");
        } else {
            _audioSource.clip = _laserSFX;
        }
    }

    void Update() {
        PlayerController();
    }

    void PlayerController() {
        // Input
        float horizontalInput = Input.GetAxis("Horizontal");
	    float verticalInput = Input.GetAxis("Vertical");

        // Movement
	    transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
	    transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);

        // Position manager
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 4.5f), 0);
        if (transform.position.x > 11.25f) {
            transform.position = new Vector3(-11.25f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.25f) {
            transform.position = new Vector3 (11.25f, transform.position.y, 0);
        }

        // Fire
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && !_isDead) {
            Fire();
        }

        // Pause
        if (Input.GetKeyDown(KeyCode.Escape)) {
            _gameManager.PauseGame();
            _uiManager.PauseScreen();
        }
    }

    void Fire() {
        // Cooldown calculation
        _canFire = Time.time + _fireRate;
        // Instantiates 1.2 meters above player
        if (!tripleShotIsActive) {
            Instantiate(_laserPrefab, transform.position + new Vector3 (0, 1.2f, 0), Quaternion.identity);
        }
        else {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        _audioSource.Play();
    }

    IEnumerator NextLife() {
        yield return new WaitForSeconds(2);
        int i;
        transform.position = new Vector3(0, 0, 0);
        _isDead = false;
        for (i = 0 ; i < 15 ; i++) {
            yield return new WaitForSeconds(0.2f);
            if (i % 2 == 0) {
                _shipSpriteRenderer.enabled = true;
                _trailSpriteRenderer.enabled = true;
            } else {
                _shipSpriteRenderer.enabled = false;
                _trailSpriteRenderer.enabled = false;
            }
        }
        _shipCollider.enabled = true;
    }

    public void Damage() {
        if (shieldIsActive) {
            shieldIsActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }
        // THE CODE HALTS HERE IF THE PLAYER HAS A SHIELD //
            _shipCollider.enabled = false;
            _isDead = true;
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _lives--;
            _uiManager.UpdateLives(_lives);
            _spawnManager.OnPlayerDeath(_lives);

        if (_lives < 1) {
            Destroy(this.gameObject);
        } else {
            _shipSpriteRenderer.enabled = false;
            _trailSpriteRenderer.enabled = false;
            StartCoroutine(NextLife());
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("EnemyLaser")) {
            Damage();
            Destroy(other.gameObject);
        }
    }

    public void TripleShotActive() {
        tripleShotIsActive = true;
        StartCoroutine(TripleShotPowerdownRoutine());
    }

    public void SpeedActive() {
        speedIsActive = true;
        _speed *= _speedBoost;
        StartCoroutine(SpeedPowerdownRoutine());
    }

    public void ShieldActive() {
        shieldIsActive = true;
        _shieldVisualizer.SetActive(true);
    }

    IEnumerator TripleShotPowerdownRoutine() {
        yield return new WaitForSeconds(_powerupTimerDuration);
        tripleShotIsActive = false;
    }

    IEnumerator SpeedPowerdownRoutine() {
        yield return new WaitForSeconds(_powerupTimerDuration);
        speedIsActive = false;
        _speed /= _speedBoost;
    }

    public void AddScore(int points) {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
