using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<K, V> : Dictionary<K, V>, ISerializationCallbackReceiver
{
    [SerializeField] private List<K> _keys = new List<K>();
    [SerializeField] private List<V> _values = new List<V>();

    public void OnBeforeSerialize()
    {
        //_keys.Clear();
        //_values.Clear();

        //_keys.AddRange(this.Keys);
        //_values.AddRange(this.Values);
    }

    public void OnAfterDeserialize()
    {
        this.Clear();

        for(int i=0; i<_keys.Count && i<_values.Count; i++)
        {
            this.Add(_keys[i], _values[i]);
        }
    }

    
}
