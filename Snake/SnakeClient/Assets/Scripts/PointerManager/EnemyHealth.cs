using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {
    public Action OnDie;

    private void OnCollisionEnter(Collision collision) {
        Die();
    }

    public void Die() {
        Destroy(gameObject);
        OnDie.Invoke();
    }
}
