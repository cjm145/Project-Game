using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementControl : MonoBehaviour {
    private Rigidbody2D _rb;
    private Vector2 _dir = Vector2.down;
    public float MovementSpeed = 5f;

    public KeyCode iUp = KeyCode.UpArrow;
    public KeyCode iDown = KeyCode.DownArrow;
    public KeyCode iLeft = KeyCode.LeftArrow;
    public KeyCode iRight = KeyCode.RightArrow;

    [Header("Sprites")]
    public AnimatedSpriteRenderer sRUp;
    public AnimatedSpriteRenderer sRDown;
    public AnimatedSpriteRenderer sRLeft;
    public AnimatedSpriteRenderer sRRight;
    private AnimatedSpriteRenderer defaultSR;

    [Header("UI")]
    [SerializeField] private LerpUIPosition _youDiedPanel;
    [SerializeField] private LerpUIPosition _youWinPanel;


    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        defaultSR = sRDown;
    }

    private void Update() {
        if(Input.GetKey(iUp)) {
            SetDir(Vector2.up, sRUp);
        } else if(Input.GetKey(iDown)) {
            SetDir(Vector2.down, sRDown);
        } else if(Input.GetKey(iLeft)) {
            SetDir(Vector2.left, sRLeft);
        } else if(Input.GetKey(iRight)) {
            SetDir(Vector2.right, sRRight);
        } else {
            SetDir(Vector2.zero, defaultSR);
        }
    }

    private void FixedUpdate() {
        Vector2 position = _rb.position;
        Vector2 translation = MovementSpeed * Time.fixedDeltaTime * _dir;

        _rb.MovePosition(position + translation);
    }

    private void SetDir(Vector2 nDir, AnimatedSpriteRenderer animation) {
        _dir = nDir;

        sRUp.enabled = animation == sRUp;
        sRDown.enabled = animation == sRDown;
        sRLeft.enabled = animation == sRLeft;
        sRRight.enabled = animation == sRRight;

        defaultSR = animation;
        defaultSR.IsIdle = _dir == Vector2.zero;
    }

    public void Win() {
        SoundPlayer.Instance.PlaySound("StageClear");
        SoundPlayer.Instance.StopBackgroundMusic();

        int curIndex = SceneManager.GetActiveScene().buildIndex;
        if(SceneManager.sceneCountInBuildSettings > (curIndex + 1)) {
            StartCoroutine(LoadNewScene());
        } else {
            GetComponent<BombController>().enabled = false;
            GetComponent<MovementControl>().enabled = false;
            _youWinPanel.Toggle();
        }

       
    }

    private IEnumerator LoadNewScene() {
        yield return new WaitForSeconds(4.1f); // win sound time.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Die() {
        if(!GetComponent<MovementControl>().enabled) {
            return;
        }

        SoundPlayer.Instance.PlaySound("PlayerDie");
        SoundPlayer.Instance.StopBackgroundMusic();
        GetComponent<BombController>().enabled = false;
        GetComponent<MovementControl>().enabled = false;
        _youDiedPanel.Toggle();
        StartCoroutine(DieAnimation());
    }

    private Vector3 _dieSpeed = new(1f, 1f, 0);
    private IEnumerator DieAnimation() {
        Vector3 scale = transform.localScale;
        while(scale.x > 0) {
            scale = transform.localScale;
            scale -= _dieSpeed * Time.deltaTime;
            transform.localScale = scale;
            yield return null;
        }
    }

    public void OnRetryPressed() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // reload current scene
    }

    public void OnPickup(Pickup p) {
        switch(p.Type) {
            case PickupUpgrade.BlastRadius:
                GetComponent<BombController>().IncreaseBlastRadius(p.Strength);
                break;
            case PickupUpgrade.ExtraBomb:
                GetComponent<BombController>().IncreaseBombCount(p.Strength);
                break;
            default:
                throw new System.Exception();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Enemy")) {
            Die();
        }
    }
}
