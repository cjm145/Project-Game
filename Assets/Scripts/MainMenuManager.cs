using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    [SerializeField] private float _startAnimationDelay = 0.5f; // has to be >= 0.1f

    private void Start() {
        StartCoroutine(PlayAnimations());
    }

    private IEnumerator PlayAnimations() {
        yield return new WaitForSeconds(_startAnimationDelay);
        foreach(LerpUIPosition lup in FindObjectsOfType<LerpUIPosition>()) {
            lup.Toggle();
        }
    }

    public void OnStartPressed() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ControlsMenu() {
        SceneManager.LoadScene("ControlsMenu");
    }

    public void MainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame() {
        Application.Quit();
    }

}
