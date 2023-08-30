using UnityEngine;

public class DeathParticle : MonoBehaviour {
    [SerializeField] private GameObject _deathParticle;
    private ParticleSystemRenderer _deathRenderer;
    
    private void OnEnable() {
        _deathRenderer = _deathParticle.GetComponent<ParticleSystemRenderer>();
    }

    private void OnDestroy() {
        _deathRenderer.material = gameObject.GetComponent<AppearanceManager>().GetMaterialFromSnakePart();
        Instantiate(_deathParticle, transform.position, transform.rotation);
    }
}
