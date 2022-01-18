using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameObject _devNotes;

    public void Start() {
        _devNotes = GameObject.Find("DevNotes");
        _devNotes.SetActive(false);
    }

    public void StartGame() {
        SceneManager.LoadScene(1);
    }

    public void DevNotesShow() {
        _devNotes.SetActive(true);
    }

    public void DevNotesHide() {
        _devNotes.SetActive(false);
    }

    public void UdemyCourseLink() {
        Application.OpenURL("https://www.udemy.com/course/the-ultimate-guide-to-game-development-with-unity/");
    }

    public void ProjectSourceCode() {
        Application.OpenURL("https://github.com/hedgerow512/unity-untitled-space-shooter/");
    }

    public void MyGithub() {
        Application.OpenURL("https://github.com/hedgerow512");
    }

    public void QuitGame() {
        Application.Quit();
    }
}
