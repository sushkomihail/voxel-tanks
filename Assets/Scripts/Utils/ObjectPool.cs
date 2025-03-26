using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utils
{
    public class ObjectPool<T> where T : MonoBehaviour
    {
        private readonly T _prefab;
        private readonly List<T> _objects = new();
        private readonly Action<T> _initializeFunction;

        public ObjectPool(T prefab, Action<T> initializeFunction, int objectsCount)
        {
            _prefab = prefab;
            _initializeFunction = initializeFunction;
            
            for (int i = 0; i < objectsCount; i++)
            {
                var obj = Object.Instantiate(_prefab, Vector3.zero, Quaternion.identity);
                _initializeFunction(obj);
                obj.gameObject.SetActive(false);
                _objects.Add(obj);
            }
        }

        public T Get()
        {
            var obj = GetFirstNotActive();

            if (obj == null)
            {
                AddObject(out obj);
            }
            
            obj.gameObject.SetActive(true);
            return obj;
        }

        public void Release(T obj)
        {
            obj.gameObject.SetActive(false);
        }

        private void AddObject(out T obj)
        {
            obj = Object.Instantiate(_prefab, Vector3.zero, Quaternion.identity);
            _initializeFunction(obj);
            _objects.Add(obj);
        }

        private T GetFirstNotActive()
        {
            foreach (var obj in _objects)
            {
                if (!obj.gameObject.activeInHierarchy)
                {
                    return obj;
                }
            }

            return null;
        }
    }
}