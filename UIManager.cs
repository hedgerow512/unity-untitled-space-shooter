using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _gameOverText;
    [SerializeField] private TMP_Text _restartText;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _nextLevelText;
    [SerializeField] private Image _livesImage;
    [SerializeField] private Sprite[] _liveSprites;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private GameObject _pauseUI;

    // Start is called before the first frame update
    void Start()
    {
        _pauseUI.gameObject.SetActive(false);
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        _levelText.text = "Level 1";
        

        // Error checking
        if (_gameManager == null) {
            Debug.LogError("Game Manager is NULL.");
        }
    }

    public void PauseScreen() {
        if (_gameManager.isGamePaused) {
            _pauseUI.gameObject.SetActive(true);
        } else {
            _pauseUI.gameObject.SetActive(false);
        }
    }

    public void UpdateScore(int playerScore) {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives) {
        if (currentLives < 1){
            GameOverSequence();
        }
        _livesImage.sprite = _liveSprites[currentLives];
    }

    void GameOverSequence() {
        _gameManager.GameOver();
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine() {
        while (true) {
        _gameOverText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        _gameOverText.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        }
    }
}
