using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    private float randomX = 0;
    private Player _player;
    private Animator _anim;
    private AudioSource _audioSource;
    private Collider2D _collider;
    private float _fireRate;
    [SerializeField] private GameObject _enemyLaserPrefab;
    private bool _stopShooting = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FireLaser());
        _collider = GetComponent<Collider2D>();
        _audioSource = GetComponent<AudioSource>();
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null) {
            Debug.LogError("The Player is NULL.");
        }

        _anim = GetComponent<Animator>();
        if (_anim == null) {
            Debug.LogError("The Animator is NULL.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if ((transform.position.y < -5.75f) && (_collider != null)) {
            randomX = Random.Range(-10f, 10f);
            transform.position = new Vector3(randomX, 7.75f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            Player player = other.transform.GetComponent<Player>();
            if (player != null) {
                player.Damage();
            }
            _stopShooting = true;
            DestroyProcess();
        }
        else if (other.gameObject.CompareTag("Laser")) {
            Destroy(other.gameObject);
            if (_player != null) {
                _player.AddScore(10);
            }
            _stopShooting = true;
            DestroyProcess();
        }
    }

    private void DestroyProcess() {
        _anim.SetTrigger("OnEnemyDeath");
        _audioSource.Play();
        Destroy(_collider);
        Destroy(this.gameObject, 2.5f);
    }

    IEnumerator FireLaser() {
        while (!_stopShooting) {
            _fireRate = Random.Range(0.5f, 2.0f);
            yield return new WaitForSeconds(_fireRate);
            Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, -1.2f, 0), Quaternion.identity);
        }
    }
}
