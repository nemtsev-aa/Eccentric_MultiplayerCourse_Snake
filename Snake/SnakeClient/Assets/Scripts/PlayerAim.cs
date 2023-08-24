using UnityEngine;

public class PlayerAim : MonoBehaviour {
    [SerializeField] private float _rotateSpeed = 90f;
    private Vector3 _targetDirection = Vector3.zero;
    private float _speed;

    public void Init(float speed) {
        _speed = speed;
    }
    private void Update() {
        Rotate();
        Move();
    }

    private void Move() {
        transform.position += transform.forward * Time.deltaTime * _speed;
    }

    private void Rotate() {
        Quaternion targetRotation = Quaternion.LookRotation(_targetDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * _rotateSpeed);
    }

    public void SetTargetDitection(Vector3 pointToLook) {
        _targetDirection = pointToLook - transform.position;
    }

    public void GetMoveInfo(out Vector3 position) {
        position = transform.position;
    }
}
