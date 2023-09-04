using CustomEventBus;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServicesLoader : MonoBehaviour, IServicesLoader {
    protected EventBus _eventBus;                                                                           // Ўина событий
    protected SavesManager _savesManager;
    protected List<CustomEventBus.IDisposable> _disposables = new List<CustomEventBus.IDisposable>();       // »нтерфейс дл€ отписки от сигнальной шины

    public virtual void Init() {
        
    }

    public virtual void RegisterServices() {
        
    }

    public virtual void AddDisposables() {

    }

    public virtual void OnDestroy() {

    }
}
