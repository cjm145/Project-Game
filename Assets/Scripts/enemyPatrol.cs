using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class enemyPatrol : MonoBehaviour {
    [SerializeField] private float _differenceFromPointMax = 0.2f;
    [SerializeField] private float _movementSpeed = 3.0f;
    [SerializeField] private Transform _moveToPoint;

    private Tilemap[] _tilemaps;

    private Animator _animator;

    private void Start() {
        _animator = GetComponent<Animator>();
        _tilemaps = 
            FindObjectsOfType<Tilemap>().Where(x => x.gameObject.GetComponent<Collider2D>() != null).ToArray();
    }

    void Update() {
        float step = _movementSpeed * Time.deltaTime;

        MoveTowardsPoint(step);
    }

    private readonly Vector3Int[] v = {
        Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right,
    };

    private void NewMoveTo() {
    start:
        Vector3Int random = v[Random.Range(0, 4)];
        Vector3 worldPos = transform.position;
        bool t = true;
        while(t) {
            worldPos += random;
            foreach(Tilemap tm in _tilemaps) {
                var cp = tm.WorldToCell(worldPos);
                if(tm.HasTile(cp)) {
                    t = false;
                    worldPos -= random;
                    goto end;
                }
            }
            List<Vector3Int> otherDir = new();
            #region Caluculate other possible routes
            if(!_tilemaps[0].HasTile(_tilemaps[0].WorldToCell(worldPos + Vector3Int.up)) &&
                !_tilemaps[1].HasTile(_tilemaps[1].WorldToCell(worldPos + Vector3Int.up)) &&
                random != Vector3Int.up) {
                otherDir.Add(Vector3Int.up);
            }
            if(!_tilemaps[0].HasTile(_tilemaps[0].WorldToCell(worldPos + Vector3Int.right)) &&
                !_tilemaps[1].HasTile(_tilemaps[1].WorldToCell(worldPos + Vector3Int.right)) &&
                random != Vector3Int.right) {
                otherDir.Add(Vector3Int.right);
            }
            if(!_tilemaps[0].HasTile(_tilemaps[0].WorldToCell(worldPos + Vector3Int.down)) &&
                !_tilemaps[1].HasTile(_tilemaps[1].WorldToCell(worldPos + Vector3Int.down)) &&
                random != Vector3Int.down) {
                otherDir.Add(Vector3Int.down);
            }
            if(!_tilemaps[0].HasTile(_tilemaps[0].WorldToCell(worldPos + Vector3Int.left)) &&
                !_tilemaps[1].HasTile(_tilemaps[1].WorldToCell(worldPos + Vector3Int.left)) &&
                random != Vector3Int.left) {
                otherDir.Add(Vector3Int.left);
            }
            #endregion
            if(Random.Range(0, otherDir.Count) != 0) {
                goto end;
            }
        }
    end:
        if(worldPos == transform.position) {
            goto start;
        }
        _moveToPoint.position = worldPos;

        if(random == Vector3Int.up) {
            _animator.Play("WalkUp");
        } else if(random == Vector3Int.down) {
            _animator.Play("WalkDown");
        } else if(random == Vector3Int.left) {
            _animator.Play("WalkLeft");
        } else if(random == Vector3Int.right) {
            _animator.Play("WalkRight");
        }
    }

    private void MoveTowardsPoint(float step) {
        transform.position = Vector2.MoveTowards(transform.position, _moveToPoint.position, step);
        if(HasReachedPoint(_moveToPoint)) {
            NewMoveTo();
        }
    }

    private bool HasReachedPoint(Transform point) {
        return Vector2.Distance(transform.position, point.position) <= _differenceFromPointMax;
    }
}
