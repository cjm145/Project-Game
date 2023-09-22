using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedSpriteRenderer : MonoBehaviour {
    private SpriteRenderer _spriteRenderer;

    public Sprite Idle;
    public Sprite[] AnimationSprites;

    public float AnimationTime = 0.25f;
    private int _currentFrame;

    public bool IsLooping = true;
    public bool IsIdle = true;

    private void Awake() => _spriteRenderer = GetComponent<SpriteRenderer>();

    private void OnEnable() => _spriteRenderer.enabled = true;

    private void OnDisable() => _spriteRenderer.enabled = false;

    private void Start() => InvokeRepeating(nameof(SetNextFrame), AnimationTime, AnimationTime);

    private void SetNextFrame() {
        _currentFrame++;

        if(IsLooping && _currentFrame >= AnimationSprites.Length) {
            _currentFrame = 0;
        }

        if(IsIdle) {
            _spriteRenderer.sprite = Idle;
        } else if(_currentFrame >= 0 && _currentFrame < AnimationSprites.Length) {
            _spriteRenderer.sprite = AnimationSprites[_currentFrame];
        }
    }

}
