using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.CullingGroup;

public class KitchenGameManager : MonoBehaviour
{


    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
    public static KitchenGameManager instance { get; private set; }


    
    [Serializable]
    public enum State {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    private float waitingToStartTimer = 1f;
    private float waitingToStartTimerMax = 1f;
    private float countdownToStartTimer = 3f;
    private float countdownToStartTimerMax = 3f;
    private float gamePlayingTimer = 240f;
    private float gamePlayingTimerMax = 240f;

    bool isGamePaused = false;
    private State state;
    private void Awake() {
        instance = this;
        state=State.WaitingToStart;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
        waitingToStartTimer = 1f;
        
    }

    private void Start() {
        GameInput.instance.OnPauseAction += GameInput_OnPauseAction;
        isGamePaused = false; 
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e) {
        if (IsGamePlaying()) { 
            TogglePauseGame();
        }
    }

    private void TogglePauseGame() {
        
        if (!isGamePaused) {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else { 
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);

        }
        isGamePaused = !isGamePaused;
    }

    private void Update() {
        switch (state) {
            case State.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime;
                if (waitingToStartTimer <= 0f) { 
                    waitingToStartTimer = waitingToStartTimerMax; state = State.CountdownToStart;  
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer <= 0f) { 
                    countdownToStartTimer = countdownToStartTimerMax; state = State.GamePlaying; 
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer <= 0f) { 
                    gamePlayingTimer = gamePlayingTimerMax; 
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                /*
                waitingToStartTimer -= Time.deltaTime;
                if (waitingToStartTimer <= 0f) { waitingToStartTimer = 1f; state = State.CountdownToStart; }
                */
                break;
        }
    }
    public bool IsGamePlaying() {return state == State.GamePlaying;}
    public bool IsGameOnCountdown() {return state == State.CountdownToStart; }
    public bool IsGameOver() {return state == State.GameOver; }

    public int GetCountdownToStartTimer() { return (int)countdownToStartTimer; }
    public void TriggerGameOver() { state=State.GameOver; }

    public float GetGamePlayingTimerNormalized() {
        return 1 - gamePlayingTimer * 1.0f / gamePlayingTimerMax;
    }
}

