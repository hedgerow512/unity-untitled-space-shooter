using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int _countdown;
    private int _level;
    private bool _isGameOver;
    public bool isGamePaused;

    private void Start() {
        NewGameStats();
        StartCoroutine(RunTime());
    }

    private void NewGameStats() {
        _level = 1;
        _countdown = 60;
    }

    private void Update() {
        if (_isGameOver) {
            if (Input.GetKeyDown(KeyCode.R)) {
                NewGameStats();
                SceneManager.LoadScene(1); // Current game scene
            }
            else if (Input.GetKeyDown(KeyCode.E)) {
                NewGameStats();
                SceneManager.LoadScene(0); // Main Menu
            }
        }
    }

    public void GameOver() {
        _isGameOver = true;
    }

    public void PauseGame() {
        if (isGamePaused) {
            Time.timeScale = 1;
            isGamePaused = false;
        } else {
            Time.timeScale = 0;
            isGamePaused = true;
        }
    }
    
    IEnumerator RunTime() {
        while (!_isGameOver) {
            yield return new WaitForSeconds(1);
            _countdown--;
            if (_countdown <= 0) {
                _level++;
                _countdown = 60;
            }
        }
    }
}
