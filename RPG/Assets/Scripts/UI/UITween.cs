using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent(typeof(RectTransform))]
public class UITween : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected RectTransform rectTransform;
    protected Vector2 defaultScale;
    [SerializeField] protected float tweenDuration = 0.2f;
    [SerializeField] protected float enableTweenDelay = 0;
    [SerializeField] protected Vector2 disabledScale = Vector2.zero;
    [SerializeField] protected Vector2 hoverScale = Vector2.one;
    protected bool Enabled = false;
    protected virtual void Awake() {
        rectTransform = GetComponent<RectTransform>();
        defaultScale = rectTransform.localScale;
        Enabled = enabled;
    }
    public void Enable() {
        if (disabledScale == defaultScale || Enabled || enableTweenDelay < 0)
            return;
        rectTransform.localScale = disabledScale;
        gameObject.SetActive(true);
        Enabled = true;
        //LeanTween.cancel(currentTweenID);
        LeanTween.scale(rectTransform, defaultScale, tweenDuration).setIgnoreTimeScale(true).setDelay(enableTweenDelay);
    }
    public void Disable() {
        if (disabledScale == defaultScale || !Enabled || enableTweenDelay < 0)
            return;
        LeanTween.scale(rectTransform, disabledScale, tweenDuration).setIgnoreTimeScale(true).setOnComplete(ActuallyDisable);
    }
    protected void ActuallyDisable(){
        gameObject.SetActive(false);
    }
    public void ToggleEnabled(){
        if (Enabled){
            Disable();
        } else {
            Enable();
        }
    }
    public virtual void OnPointerEnter(PointerEventData pointer){

    }
    public virtual void OnPointerExit(PointerEventData pointer){

    }
}
