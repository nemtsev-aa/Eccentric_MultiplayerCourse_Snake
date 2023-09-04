using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum SaveType { Binary, Json };

public class SavesManager : IService, CustomEventBus.IDisposable {
    public IStorageService CurrentService { get { return _saveServices[_currentSaveType]; } }
    private string _savePath;
    private SaveType _currentSaveType = SaveType.Binary;

    private Dictionary<SaveType, IStorageService> _saveServices = new Dictionary<SaveType, IStorageService>();

    public void Init() {
        if (_savePath == "") _savePath = Application.persistentDataPath;
        //_savePath = Application.dataPath;

        _saveServices.Add(SaveType.Binary, new BinaryToFileStorageService());
        _saveServices.Add(SaveType.Json, new JsonToFileStorageService());

        foreach (var iService in _saveServices) {
            iService.Value.Init(_savePath);
        }
    }

    public void SetType(SaveType saveType) {
        _currentSaveType = saveType;
    }

    public void Save(string key, object data, Action<bool> callback = null) {
        _saveServices[_currentSaveType].Save(key, data, callback);
    }

    public void Load<T>(string key, Action<T> callback) {
        _saveServices[_currentSaveType].Load(key, callback);
    }

    public void DeleteFile(string key) {
        string path = "";
        switch (_currentSaveType) {
            case SaveType.Binary:
                path = Path.Combine(_savePath, key, ".save");
                break;
            case SaveType.Json:
                path = Path.Combine(_savePath, key, ".json");
                break;
            default:
                break;
        }

        if (File.Exists(path)) {
            File.Delete(path);
            Debug.Log("SavesManager: DeleteFile - Deleted");
        } else {
            Debug.LogError("SavesManager: DeleteFile - No file");
        } 
    }

    public void Dispose() {
        
    }
}

