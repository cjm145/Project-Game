using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombController : MonoBehaviour {
    [Header("Bomb")]
    public KeyCode inputKey = KeyCode.LeftShift;
    public GameObject BombPrefab;
    public float BombExplodeAfter = 3f;
    public int BombAmount = 1;
    private int _bombsRemaining;

    [Header("Explosion")]
    public Explosion ExplosionPrefab;
    public LayerMask ExplosionLayerMask;
    public LayerMask ElayerLayerMask;
    public LayerMask EnemyLayerMask;
    public float ExplosionDur = 1f;
    public int ExplosionWidth = 1;

    [Header("Destructible")]
    public Tilemap Destructables;
    public Destructible DestructablePrefab;

    private void OnEnable() => _bombsRemaining = BombAmount;

    private void Update() {
        if(_bombsRemaining > 0 && Input.GetKeyDown(inputKey)) {
            StartCoroutine(PlantBomb());
        }
    }

    private IEnumerator PlantBomb() {
        Vector2 pos = transform.position;
        pos.x = Mathf.Round(pos.x);
        pos.y = Mathf.Round(pos.y);

        GameObject b = Instantiate(BombPrefab, pos, Quaternion.identity);
        _bombsRemaining--;

        SoundPlayer.Instance.PlaySound("BombPlaced");

        yield return new WaitForSeconds(BombExplodeAfter);

        pos = b.transform.position;
        pos.x = Mathf.Round(pos.x);
        pos.y = Mathf.Round(pos.y);

        Explosion expl = Instantiate(ExplosionPrefab, pos, Quaternion.identity);
        expl.SetAnimator(expl.startAnimation);
        expl.DestroyAfter(ExplosionDur);

        SoundPlayer.Instance.PlaySound("Explosion");
        CalculateExplosion(pos, Vector2.up, ExplosionWidth);
        CalculateExplosion(pos, Vector2.down, ExplosionWidth);
        CalculateExplosion(pos, Vector2.left, ExplosionWidth);
        CalculateExplosion(pos, Vector2.right, ExplosionWidth);

        Destroy(b);
        _bombsRemaining++;
    }

    private void CalculateExplosion(Vector2 pos, Vector2 dir, int length) {
        if(length <= 0) {
            return;
        }

        pos += dir;

        if(Physics2D.OverlapBox(pos, Vector2.one / 2f, 0f, ExplosionLayerMask)) {
            DestructableHit(pos);
            return;
        } 

        if(Physics2D.OverlapBox(pos, Vector2.one / 2f, 0f, EnemyLayerMask)) {
            SoundPlayer.Instance.PlaySound("EnemyDie");
            Destroy(Physics2D.OverlapBox(pos, Vector2.one / 2f, 0f, EnemyLayerMask).gameObject);
            return;
        }

        Explosion explosion = Instantiate(ExplosionPrefab, pos, Quaternion.identity);
        explosion.SetAnimator(length > 1 ? explosion.middleAnimation : explosion.endAnimation);
        explosion.Setdir(dir);
        explosion.DestroyAfter(ExplosionDur);

        if(Physics2D.OverlapBox(pos, Vector2.one / 2f, 0f, ElayerLayerMask)) {
            FindObjectOfType<MovementControl>().Die();
            return;
        }

        CalculateExplosion(pos, dir, length - 1);
    }

    private void DestructableHit(Vector2 pos) {
        Vector3Int cell = Destructables.WorldToCell(pos);
        TileBase tile = Destructables.GetTile(cell);

        if(tile != null) {
            Instantiate(DestructablePrefab, pos, Quaternion.identity);
            Destructables.SetTile(cell, null);
        }
    }

    public void IncreaseBlastRadius(int by) {
        ExplosionWidth += by;
    }
    
    public void IncreaseBombCount(int by) {
        BombAmount += by;
        _bombsRemaining += by;
    }
    
    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("Bomb")) {
            other.isTrigger = false;
        }
    }
}
