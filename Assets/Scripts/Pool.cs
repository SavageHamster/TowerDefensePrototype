using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class Pool : SingletonMonoBehaviour<Pool>
{
    [Serializable]
    private class PrototypeCount
    {
        public MonoBehaviour prototype;
        public int count;
    }

    [SerializeField]
    private List<PrototypeCount> _prototypes;

    private readonly Dictionary<Type, List<GameObject>> _activeDict = new Dictionary<Type, List<GameObject>>();
    private readonly Dictionary<Type, List<GameObject>> _inactiveDict = new Dictionary<Type, List<GameObject>>();
    private readonly Dictionary<Type, GameObject> _prototypesByType = new Dictionary<Type, GameObject>();

    protected override void OnAwake()
    {
        GroupPrototypesByType();

        foreach (var prototypeCountPair in _prototypes)
        {
            for (int i = 0; i < prototypeCountPair.count; i++)
            {
                var monoBehaviour = Instantiate(prototypeCountPair.prototype, transform);
                monoBehaviour.gameObject.SetActive(false);
                var type = monoBehaviour.GetType();

                if (!_inactiveDict.ContainsKey(type))
                {
                    _inactiveDict.Add(type, new List<GameObject>());
                }

                _inactiveDict[type].Add(monoBehaviour.gameObject);
            }
        }

        foreach (var key in _inactiveDict.Keys)
        {
            _activeDict.Add(key, new List<GameObject>());
        }

        base.OnAwake();
    }

    public GameObject Get<T>() where T : MonoBehaviour
    {
        GameObject result = null;

        var type = typeof(T);

        if (_inactiveDict.ContainsKey(type) && _inactiveDict[type] != null)
        {
            if (_inactiveDict[type].Count > 0)
            {
                result = _inactiveDict[type][0];
                result.SetActive(true);
                result.transform.SetParent(null);
                _activeDict[type].Add(result);
                _inactiveDict[type].RemoveAt(0);
            }
            else
            {
                result = Instantiate(_prototypesByType[type].gameObject, transform);
                result.transform.SetParent(null);
                _activeDict[type].Add(result);
            }
        }

        return result;
    }

    public void Release<T>(GameObject go) where T : MonoBehaviour
    {
        var type = typeof(T);

        if (_activeDict.ContainsKey(type))
        {
            for (int i = 0; i < _activeDict[type].Count; i++)
            {
                var activeElement = _activeDict[type][i];

                if (activeElement == go)
                {
                    activeElement.SetActive(false);
                    activeElement.transform.SetParent(transform);
                    activeElement.transform.position = Vector3.zero;
                    activeElement.transform.rotation = Quaternion.identity;
                    _inactiveDict[type].Add(activeElement);
                    _activeDict[type].RemoveAt(i);

                    return;
                }
            }
        }
    }

    public void GroupPrototypesByType()
    {
        if (_prototypesByType.Count == 0)
        {
            foreach (var prototypeCount in _prototypes)
            {
                if (prototypeCount != null && prototypeCount.prototype != null)
                {
                    var type = prototypeCount.prototype.GetType();

                    if (!_prototypesByType.ContainsKey(type))
                    {
                        _prototypesByType.Add(type, prototypeCount.prototype.gameObject);
                    }
                }
            }
        }
    }
}
