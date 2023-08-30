using System;
using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour {
    
    [SerializeField] private Transform _detailPrefab;
    [SerializeField] private AppearanceManager _appearanceManager;
    [SerializeField] private float _detailDistance = 1f;

    private Transform _head;
    private float _snakeSpeed;

    private List<Transform> _details = new List<Transform>();
    private List<Vector3> _positionHistory = new List<Vector3>();
    private List<Quaternion> _rotationHistory = new List<Quaternion>();

    private int _playerLayer;
    private bool _isPlayer;

    public void Init(Transform head, float speed, int detailCount, int playerLayer, bool isPlayer) {
        _playerLayer = playerLayer;
        _isPlayer = isPlayer;

        if (_isPlayer) SetPlayerLayer(gameObject);

        _head = head;
        _snakeSpeed = speed;

        _details.Add(transform);
        _positionHistory.Add(_head.position);
        _rotationHistory.Add(_head.rotation);

        _positionHistory.Add(transform.position);
        _rotationHistory.Add(transform.rotation);

        SetDetailCount(detailCount);
    }

    private void SetPlayerLayer(GameObject gameObject) {
        gameObject.layer = _playerLayer;
        Transform[] childrens = GetComponentsInChildren<Transform>();
        foreach (var iChildren in childrens) {
            iChildren.gameObject.layer = _playerLayer;
        }
    }

    public void SetDetailCount(int detailCount) {
        if (_details.Count - 1 == detailCount) return;

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
        Quaternion rotation = _details[_details.Count - 1].rotation;
        Transform newDetail = Instantiate(_detailPrefab, position, rotation);
        if (_isPlayer) SetPlayerLayer(newDetail.gameObject);
        
        _details.Insert(0, newDetail);
        _positionHistory.Add(position);
        _rotationHistory.Add(rotation);
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
        _rotationHistory.RemoveAt(_rotationHistory.Count - 1);
    }

    public void SetSkinFromDetails(SkinData skin) {
        for (int i = 0; i < _details.Count; i++) {
            _details[i].GetComponent<AppearanceManager>().SetSkin(skin);
        }
    }

    public void SetSkinFromTail(SkinData skin) {
        for (int i = 0; i < _details.Count; i++) {
            _details[i].GetComponent<AppearanceManager>().SetSkin(skin);
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

            _rotationHistory.Insert(0, _head.rotation);
            _rotationHistory.RemoveAt(_rotationHistory.Count - 1);

            distance -= _detailDistance;
        }

        for (int i = 0; i < _details.Count; i++) {
            float percent =  distance / _detailDistance;
            _details[i].position = Vector3.Lerp(_positionHistory[i + 1], _positionHistory[i], percent);
            _details[i].rotation = Quaternion.Lerp(_rotationHistory[i + 1], _rotationHistory[i], percent);
        }
    }

    public DetailPositions GetDetailPositions() {
        int detailsCount = _details.Count;
        DetailPosition[] ds = new DetailPosition[detailsCount];
        for (int i = 0; i < detailsCount; i++) {
            ds[i] = new DetailPosition() {
                X = _details[i].position.x,
                Z = _details[i].position.z
            };
        }

        DetailPositions detailPosition = new DetailPositions() {
            DS = ds
        };
        
        return detailPosition;
    }

    public void Destroy() {
        for (int i = 0; i < _details.Count; i++) {
            Destroy(_details[i].gameObject);
        }
    }
}

[Serializable]
public struct DetailPosition {
    public float X;
    public float Z;
}

[Serializable]
public struct DetailPositions {
    public string ID;
    public DetailPosition[] DS;
}

