using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UITween[] menus;
    [SerializeField] private UITween equipmentPanel = null;
    public static UITween currentlyOpenUI = null;
    public static bool paused = false;
    public static GameManager instance;
    private Controls controls;
    private void Awake() {
        if (instance){
            Destroy(gameObject);
            return;
        }
        instance = this;
        controls = new Controls();
        controls.Player.OpenInventory.performed += ctx => ToggleInventory();
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
    public static void OpenMenu(int menuIndex){
        SetPause_Static(true);
        UITween menu = instance.menus[menuIndex];
        menu.Enable();
        currentlyOpenUI = menu;
    }
    public static void CloseMenu(){
        if (currentlyOpenUI){
            currentlyOpenUI.Disable();
            currentlyOpenUI = null;
            SetPause_Static(false);
        }
    }
    public static void ToggleInventory(){
        if (InvManager.currentOpenInventory == null){
            OpenMenu(0);
            instance.equipmentPanel.Enable();
        } else {
            CloseMenu();
            instance.equipmentPanel.Disable();
        }
    }
    private void OnEnable() {
        controls.Enable();
    }
    private void OnDisable() {
        controls.Disable();
    }
}