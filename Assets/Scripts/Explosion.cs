using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
    public AnimatedSpriteRenderer startAnimation;
    public AnimatedSpriteRenderer middleAnimation;
    public AnimatedSpriteRenderer endAnimation;

    public void SetAnimator(AnimatedSpriteRenderer renderer) {
        startAnimation.enabled = renderer == startAnimation;
        middleAnimation.enabled = renderer == middleAnimation;
        endAnimation.enabled = renderer == endAnimation;
    }

    public void Setdir(Vector2 dir) {
        float angle = Mathf.Atan2(dir.y, dir.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    public void DestroyAfter(float sec) {
        Destroy(gameObject, sec);
    }

}
