using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour {
    [SerializeField] private Transform _detailPrefab;
    [SerializeField] private float _detailDistance = 1f;

    private Transform _head;
    private float _snakeSpeed;

    private List<Transform> _details = new List<Transform>();
    private List<Vector3> _positionHistory = new List<Vector3>();

    public void Init(Transform head, float speed, int detailCount) {
        _head = head;
        _snakeSpeed = speed;

        _details.Add(transform);
        _positionHistory.Add(_head.position);
        _positionHistory.Add(transform.position);

        SetDetailCount(detailCount);
    }

    private void SetDetailCount(int detailCount) {
        if (_details.Count + 1 == detailCount) return;

        int diff = (_details.Count - 1) - detailCount;
        if (diff < 1) {
            for (int i = 0; i < -diff; i++) {
                AddDetail();
            }
        } else {
            for (int i = 0; i < diff; i++) {
                RemoveDetail();
            }
        }
    }

    private void AddDetail() {
        Vector3 position = _details[_details.Count - 1].position;
        Transform newDetail = Instantiate(_detailPrefab, position, Quaternion.identity);
        _details.Insert(0, newDetail);
        _positionHistory.Add(position);
    }

    private void RemoveDetail() {
        if (_details.Count <= 1) {
            Debug.LogError("Попытка удалить несуществующую деталь");
            return;
        }

        Transform detail = _details[0];
        _details.Remove(detail);
        Destroy(detail.gameObject);
        _positionHistory.RemoveAt(_positionHistory.Count - 1);
    }

    void Update() {
        Vector3 headoffset = _head.position - _positionHistory[0];
        float distance = headoffset.magnitude;

        while (distance > _detailDistance) {
            Vector3 direction = headoffset.normalized;
            Vector3 newPointPosition = _positionHistory[0] + direction * _detailDistance;
            _positionHistory.Insert(0, newPointPosition);
            _positionHistory.RemoveAt(_positionHistory.Count-1);

            distance -= _detailDistance;
        }

        for (int i = 0; i < _details.Count; i++) {
            _details[i].position = Vector3.Lerp(_positionHistory[i + 1], _positionHistory[i], distance / _detailDistance);
            //Vector3 direction = (_positionHistory[i] - _positionHistory[i + 1]).normalized;
            //_details[i].position += direction * Time.deltaTime * _snakeSpeed;
        }
    }

    public void Destroy() {
        for (int i = 0; i < _details.Count; i++) {
            Destroy(_details[i].gameObject);
        }
    }
}
