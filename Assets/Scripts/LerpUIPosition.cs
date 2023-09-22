using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LerpUIPosition : MonoBehaviour {
    [SerializeField] private AnimationCurve _moveCurve;

    [SerializeField] private Vector2 _pos1;
    [SerializeField] private Vector2 _pos2;

    private void OnDrawGizmos() {
        _rT = GetComponent<RectTransform>();
        _pos1 = _rT.anchoredPosition;
    }

    private float _animationTimePosition;

    private RectTransform _rT;

    private void Start() {
        _rT = GetComponent<RectTransform>();
        _target = _rT.anchoredPosition;
    }

    private void Update() {
        if(_target != _rT.anchoredPosition) {
            _animationTimePosition += Time.deltaTime;
            _rT.anchoredPosition = Vector2.Lerp(_start, _target, _moveCurve.Evaluate(_animationTimePosition));
        }
    }
    private Vector2 _target;
    private Vector2 _start;
    public void Toggle() {
        if(_target == _rT.anchoredPosition) {
            if(_target == _pos1) {
                _start = _pos1;
                _target = _pos2;
            } else {
                _start = _pos2;
                _target = _pos1;
            }
            _animationTimePosition = 0;
        }
    }
}
