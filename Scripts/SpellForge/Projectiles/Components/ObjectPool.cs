using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject _sourceObject;
    [SerializeField] private Transform _poolParent;

    private List<GameObject> _pool = new List<GameObject>();

    //private void Awake()
    //{
    //    _poolParent = new GameObject($"{gameObject.GetInstanceID()}_{gameObject.name}_{_sourceObject.name}").transform;
    //}

    //private void OnDisable()
    //{
    //    _poolParent?.gameObject.SetActive(false);
    //}

    //private void OnEnable()
    //{
    //    _poolParent?.gameObject.SetActive(true);
    //}

    //private void OnDestroy()
    //{
    //    Destroy(_poolParent.gameObject, 1f);
    //}

    public GameObject GetPooledObject()
    {
        var obj = _pool.FirstOrDefault(x => x.activeInHierarchy == false);
        if (obj == null)
        {
            obj = Instantiate(_sourceObject, _poolParent);
            _pool.Add(obj);
        }
        obj.SetActive(true);
        return obj;
    }

    public Transform GetPooledParent()
    {
        return _poolParent;
    }

    public T TryExtractComponent<T>(GameObject obj)
    {
        var component = obj.GetComponentInChildren<T>();
        if (component == null)
            throw new MissingComponentException($"Pooled Object did not contain required component {nameof(T)}");
        return component;
    }

    public T GetPooledObjectComponent<T>(out GameObject obj)
    {
        obj = GetPooledObject();
        return TryExtractComponent<T>(obj);
    }

    public T GetPooledObjectComponent<T>()
    {
        return TryExtractComponent<T>(GetPooledObject());
    }
}
