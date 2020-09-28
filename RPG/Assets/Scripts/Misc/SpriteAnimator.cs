using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimator : MonoBehaviour
{
    public Sprite[] frames;
    private int currentFrame;
    private float timer;
    private float frameRate;
    private bool loop;
    private bool useRealTime;
    public bool isPlaying{
        get;
        private set;
    } = false;
    private SpriteRenderer spriteRenderer;
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update() {
        if (!isPlaying) return;
        if (useRealTime) timer += Time.unscaledDeltaTime;
        else timer += Time.deltaTime;
        if (timer >= frameRate){
            timer -= frameRate;
            currentFrame = (currentFrame+1)%frames.Length;
            if (!loop && currentFrame == 0) StopPlaying();
            else spriteRenderer.sprite = frames[currentFrame];
        }
    }
    public void PlayAnimation(IList<Sprite> frameArray, float _frameRate = 1, bool looping = false, bool realTime = false){
        frames = (Sprite[])frameArray;
        frameRate = _frameRate;
        loop = looping;
        useRealTime = realTime;
        isPlaying = true;
    }
    public void PauseAnimation(bool resume = false){
        isPlaying = resume;
    }
    public void StopPlaying(){
        isPlaying = false;
        timer = 0;
    }
}