using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool paused = false;
    public static GameManager instance;
    private void Awake() {
        if (instance){
            Destroy(gameObject);
            return;
        }
        instance = this;

        UnPause();
    }
    #region Pause Manager
    public void Pause(){
        paused = true;
        Time.timeScale = 0;
    }
    public void UnPause(){
        paused = false;
        Time.timeScale = 1;
    }
    public void SetPause(bool pause){
        if (pause){
            Pause();
        } else {
            UnPause();
        }
    }
    public static void SetPause_Static(bool pause){
        instance.SetPause(pause);
    }
    public void TogglePause(){
        SetPause(!paused);
    }
    public static void TogglePause_Static(){
        instance.TogglePause();
    }
    #endregion

}