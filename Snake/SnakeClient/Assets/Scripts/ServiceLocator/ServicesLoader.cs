using CustomEventBus;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServicesLoader : MonoBehaviour, IServicesLoader {
    protected EventBus _eventBus;                                                                           // ���� �������
    protected SavesManager _savesManager;
    protected List<CustomEventBus.IDisposable> _disposables = new List<CustomEventBus.IDisposable>();       // ��������� ��� ������� �� ���������� ����

    public virtual void Init() {
        
    }

    public virtual void RegisterServices() {
        
    }

    public virtual void AddDisposables() {

    }

    public virtual void OnDestroy() {

    }
}
