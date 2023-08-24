using UnityEngine;

public class Snake : MonoBehaviour {
    public float Speed { get { return _speed; } }
    [SerializeField] private Transform _head;
    [SerializeField] private Tail _tailPrefab;
    [SerializeField] private float _speed = 2f;

    private Vector3 _targetDirection = Vector3.zero;
    private Tail _newTail;

    public void Init(int detailCount) {
        _newTail = Instantiate(_tailPrefab, transform.position, Quaternion.identity);
        _newTail.Init(_head, _speed, detailCount);
    }

    public void SetDetailCount(int detailCount) {
        _newTail.SetDetailCount(detailCount);
    }

    void Update() {
        Move();
    }

    public void SetRotation(Vector3 pointToLook) {
        _head.LookAt(pointToLook);
    }

    private void Move() {
        transform.position += _head.forward * Time.deltaTime * _speed;
    }

    public void Destroy() {
        _newTail.Destroy();
        Destroy(gameObject);
    }

    /* //Реализация поворота от знатаков геометрии
     * public void LookAt(Vector3 cursorPosition) {
       _targetDirection = cursorPosition - _head.position; 
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
