using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Pickup : MonoBehaviour {
    public PickupUpgrade Type;
    public int Strength;

    private void Start() {
        if(!gameObject.CompareTag("Pickup")) {
            Debug.LogWarning("Make sure to select \"Pickup\" tag.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.TryGetComponent<MovementControl>(out var v)) {
            v.OnPickup(this);
            Destroy(gameObject);
        }
    }
}

public enum PickupUpgrade {
    BlastRadius,
    ExtraBomb
}