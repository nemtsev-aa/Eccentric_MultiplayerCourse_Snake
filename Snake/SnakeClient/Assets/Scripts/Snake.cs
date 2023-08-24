using UnityEngine;

public class Snake : MonoBehaviour {
    [SerializeField] private Transform _head;
    [SerializeField] private Transform _directionPoint;
    [SerializeField] private Tail _tailPrefab;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _rotateSpeed = 90f;

    private Vector3 _targetDirection = Vector3.zero;
    private Tail _newTail;

    public void Init(int detailCount) {
        _newTail = Instantiate(_tailPrefab, transform.position, Quaternion.identity);
        _newTail.Init(_head, _speed, detailCount);
    }

    void Update() {
        Rotate();
        Move();
    }

    public void LookAt(Vector3 cursorPosition) {
        _targetDirection = cursorPosition - _head.position;
    }

    private void Rotate() {
        Quaternion targetRotation = Quaternion.LookRotation(_targetDirection);
        _head.rotation = Quaternion.RotateTowards(_head.rotation, targetRotation, Time.deltaTime * _rotateSpeed);
    }

    private void Move() {
        transform.position += _head.forward * Time.deltaTime * _speed;
    }

    public void GetMoveInfo(out Vector3 position) {
        position = transform.position;
    }

    public void Destroy() {
        _newTail.Destroy();
        Destroy(gameObject);
    }

    /* //Реализация поворота от знатаков геометрии
     * public void LookAt(Vector3 cursorPosition) {
        _directionPoint.LookAt(cursorPosition);
    }

    private void Rotate() {
        float diffY = _directionPoint.eulerAngles.y - _head.eulerAngles.y;
        if (diffY > 180) diffY = (diffY - 180) * -1;
        else if (diffY < -180) diffY = (diffY + 180) * -1;

        float maxAngle = Time.deltaTime * _rotateSpeed;
        float rotateY = Mathf.Clamp(diffY, -maxAngle, maxAngle);

        _head.Rotate(0, rotateY, 0);
    }*/
}
