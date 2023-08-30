using System;
using UnityEngine;

public class PlayerAim : MonoBehaviour {
    
    [SerializeField] private float _rotateSpeed = 90f;
    [SerializeField] private float _overlapRadius = 0.5f;
    private Vector3 _targetDirection = Vector3.zero;
    private float _speed;
    private Transform _snakeHead;

    public void Init(Transform snakeHead, float speed) {
        _snakeHead = snakeHead;
        _speed = speed;
    }

    private void Update() {
        Rotate();
        Move();
    }

    private void FixedUpdate() {
        CheckCollision();
    }

    private void CheckCollision() {
        Collider[] colliders = Physics.OverlapSphere(_snakeHead.position, _overlapRadius);
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].TryGetComponent(out Apple apple)) {
                apple.Collect();
            }
        }
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
