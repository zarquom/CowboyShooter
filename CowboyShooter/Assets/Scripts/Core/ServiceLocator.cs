using System;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator
{
    private static readonly Dictionary<Type, object> _services = new();

    public static void RegisterService<T>(T instance) => _services[typeof(T)] = instance;

    public static T GetService<T>()
    {
        if (_services.TryGetValue(typeof(T), out var service))
            return (T)service;
        throw new Exception($"Service {typeof(T).Name} not registered.");
    }

    public static void Clear() => _services.Clear();

}