using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator {
    private ServiceLocator() {
    }

    /// <summary>
    /// Зарегистрированные сервисы
    /// </summary>
    private readonly Dictionary<string, IService> _services = new Dictionary<string, IService>();

    public static ServiceLocator Current { get; private set; }

    public static void Initialize() {
        Current = new ServiceLocator();
    }

    // Возвращает сервис нужного нам типа
    public T Get<T>() where T : IService {
        string key = typeof(T).Name;
        if (!_services.ContainsKey(key)) {
            Debug.LogError($"{key} not registered with {GetType().Name}");
            return default(T);
            //throw new InvalidOperationException();
        }

        return (T)_services[key];
     }

    /// <summary>
    /// Регистрирует сервис в текущем сервис локаторе
    /// </summary>
    /// <typeparam name="T">Тип сервиса </typeparam>
    /// <param name="service">Экземпляр сервиса</param>
    public void Register<T>(T service) where T : IService {
        string key = typeof(T).Name;
        if (_services.ContainsKey(key)) {
            Debug.LogError($"Предпринята попытка зарегистрировать службу типа {key}, которая уже зарегистрирована в {GetType().Name}.");
            return;
        }

        _services.Add(key, service);
    }
    /// <summary>
    /// Регистрирует сервис в текущем сервис локаторе с заменой
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="service"></param>
    public void RegisterWithReplacement<T>(T service) where T : IService {
        string key = typeof(T).Name;
        if (_services.ContainsKey(key)) _services.Remove(key);
        _services.Add(key, service);
    }
    /// <summary>
    /// Убирает сервис из текущего сервис локатора
    /// </summary>
    /// <typeparam name="T">Тип сервиса.</typeparam>
    public void Unregister<T>() where T : IService {
        string key = typeof(T).Name;
        if (!_services.ContainsKey(key)) {
            Debug.LogError(
                $"Предпринята попытка отменить регистрацию службы типа {key}, которая не зарегистрирована в {GetType().Name}.");
            return;
        }

        _services.Remove(key);
    }
}
