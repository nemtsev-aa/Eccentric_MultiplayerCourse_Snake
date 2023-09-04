using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator {
    private ServiceLocator() {
    }

    /// <summary>
    /// ������������������ �������
    /// </summary>
    private readonly Dictionary<string, IService> _services = new Dictionary<string, IService>();

    public static ServiceLocator Current { get; private set; }

    public static void Initialize() {
        Current = new ServiceLocator();
    }

    // ���������� ������ ������� ��� ����
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
    /// ������������ ������ � ������� ������ ��������
    /// </summary>
    /// <typeparam name="T">��� ������� </typeparam>
    /// <param name="service">��������� �������</param>
    public void Register<T>(T service) where T : IService {
        string key = typeof(T).Name;
        if (_services.ContainsKey(key)) {
            Debug.LogError($"����������� ������� ���������������� ������ ���� {key}, ������� ��� ���������������� � {GetType().Name}.");
            return;
        }

        _services.Add(key, service);
    }
    /// <summary>
    /// ������������ ������ � ������� ������ �������� � �������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="service"></param>
    public void RegisterWithReplacement<T>(T service) where T : IService {
        string key = typeof(T).Name;
        if (_services.ContainsKey(key)) _services.Remove(key);
        _services.Add(key, service);
    }
    /// <summary>
    /// ������� ������ �� �������� ������ ��������
    /// </summary>
    /// <typeparam name="T">��� �������.</typeparam>
    public void Unregister<T>() where T : IService {
        string key = typeof(T).Name;
        if (!_services.ContainsKey(key)) {
            Debug.LogError(
                $"����������� ������� �������� ����������� ������ ���� {key}, ������� �� ���������������� � {GetType().Name}.");
            return;
        }

        _services.Remove(key);
    }
}
