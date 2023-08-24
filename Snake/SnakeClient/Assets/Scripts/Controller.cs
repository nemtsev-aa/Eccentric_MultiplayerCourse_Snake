using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Controller : MonoBehaviour {
    [SerializeField] private float _cameraOffsetY = 15;
    [SerializeField] private Transform _cursor;
    private Snake _snake;
    private Camera _camera;
    private Plane _plane;
    private MultiplayerManager _multiplayerManager;

    public void Init(Snake snake) {
        _multiplayerManager = MultiplayerManager.Instance;
        _snake = snake;
        _camera = Camera.main;
        _plane = new Plane(Vector3.up, Vector3.zero);

        _snake.AddComponent<CameraManager>().Init(_cameraOffsetY);

    }

    void Update() {
        if (Input.GetMouseButton(0)) {
            MoveCursor();
            _snake.LerpRotation(_cursor.position);
        }

        SendMove();
    }

    private void SendMove() {
        _snake.GetMoveInfo(out Vector3 position);
        Dictionary<string, object> data = new Dictionary<string, object>() {
            {"x", position.x },
            {"z", position.z }
        };

        _multiplayerManager.SendMessage("move", data);
    }

    private void MoveCursor() {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        _plane.Raycast(ray, out float distance);
        Vector3 point = ray.GetPoint(distance);
        _cursor.position = point;
    }
}
