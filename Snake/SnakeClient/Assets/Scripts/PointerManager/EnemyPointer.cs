using UnityEngine;

public class EnemyPointer : MonoBehaviour {
    private EnemyHealth _enemyHealth;

    public void Init(EnemyHealth enemyHealth) {
        _enemyHealth = enemyHealth;
        PointerManager.Instance.AddToList(this);
        _enemyHealth.OnDie += Destroy;
    }

    private void Destroy() {
        PointerManager.Instance.RemoveFromList(this);
    }
}
