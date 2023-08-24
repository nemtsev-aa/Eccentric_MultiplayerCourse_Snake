using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour {
    [SerializeField] private Transform _head;
    [SerializeField] private List<Transform> _details = new List<Transform>();
    [SerializeField] private float _detailDistance = 1f;
    [SerializeField] private float _snakeSpeed = 2f;
    private List<Vector3> _positionHistory = new List<Vector3>();

    private void Awake() {
        _positionHistory.Add(_head.position);

        foreach (Transform iDetail in _details) {
            _positionHistory.Add(iDetail.position);
        }
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
            Vector3 direction = (_positionHistory[i] - _positionHistory[i + 1]).normalized;
            _details[i].position += direction * Time.deltaTime * _snakeSpeed;
        }
    }
}
