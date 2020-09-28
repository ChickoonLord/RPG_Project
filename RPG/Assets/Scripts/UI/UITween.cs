using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent(typeof(RectTransform))]
public class UITween : MonoBehaviour
{
    [SerializeField] public bool useManualEnable = false;
    protected RectTransform rectTransform;
    protected Vector2 defaultScale = Vector2.one;
    [SerializeField] protected float tweenDuration = 0.2f;
    [SerializeField] protected float enableTweenDelay = 0;
    [SerializeField] protected Vector2 disabledScale = Vector2.zero;
    [SerializeField] protected Vector2 hoverScale = Vector2.one;
    protected bool Enabled = false;
    protected virtual void Awake() {
        rectTransform = GetComponent<RectTransform>();
        defaultScale = rectTransform.localScale;
    }
    public void Enable() {
        if (disabledScale == defaultScale || Enabled || enableTweenDelay < 0)
            return;
        if (useManualEnable) gameObject.SetActive(true);
        rectTransform.localScale = disabledScale;
        Enabled = true;
        LeanTween.cancel(gameObject);
        LeanTween.scale(rectTransform, defaultScale, tweenDuration).setIgnoreTimeScale(true).setDelay(enableTweenDelay);
    }
    public void Disable() {
        if (disabledScale == defaultScale || !Enabled || enableTweenDelay < 0)
            return;
        Enabled = false;
        LeanTween.cancel(gameObject);
        LeanTween.scale(rectTransform, disabledScale, tweenDuration).setIgnoreTimeScale(true).setOnComplete(ActuallyDisable);
    }
    protected void ActuallyDisable(){
        LeanTween.cancel(gameObject);
        gameObject.SetActive(false);
    }
    public void ToggleEnabled(){
        if (Enabled){
            Disable();
        } else {
            Enable();
        }
    }
    private void OnEnable() {
        if (!useManualEnable){
            Enable();
        }
    }
    private void OnDisable() {
        Enabled = false;
    }
}
