using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

[SerializeField] private TMP_Text _timerText;

    private float _time = 180;
    private bool _finished = false;

    private void Update() {
        if(_finished) {
            return;
        }

        _time -= Time.deltaTime;

        _timerText.text = TimeSpan.FromSeconds(_time).ToString(@"mm\:ss");

        // 180 seconds = 3 minutes
        if(_time <= 0) {
            FindObjectOfType<MovementControl>().Die();
            _timerText.text = "0:00";
            _finished = true;
        }

        var v = GameObject.FindWithTag("Enemy");
        if(v == null) {
            FindObjectOfType<MovementControl>().Win();
            _finished = true;
        }
    }

    public void OnReturnMainMenuClicked()
    {
        SceneManager.LoadScene(0);
    }

    public GameObject pauseMenu;

    public void Pause() {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void Continue() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
}
