using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
/*
public class SeriliazableList<T1, T2> : List<Tuple<T1,T2>> ,ISerializationCallbackReceiver
{
    
    public T1 Item1 { get;  set; }
    public T2 Item2 { get; set; }

    [SerializeField] List<T1> items1 = new List<T1>();
    [SerializeField] List<T2> items2 = new List<T2>();

   public SeriliazableList(T1 a,T2 b)
    {
        items1.Add(a);
        items2.Add(b);
    }



    public void OnBeforeSerialize()
    {
        
        items1.Clear();
        items2.Clear();
        for(int i = 0; i< this.Count; i++)
        {
            items1[i] = this[i].Item1;
            items2[i] = this[i].Item2;
        }
      
    }

    public void OnAfterDeserialize()
    {
        this.Clear();
        for(int i = 0; i< items1.Count; i++)
        {
            this.Add(new Tuple<T1, T2>(items1[i], items2[i]));
        }

    }

}*/

public class SeriliazableDictionary<TKey, TValue1,TValue2> : Dictionary<TKey, Tuple<TValue1,TValue2>>, ISerializationCallbackReceiver 
{


    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue1> values1= new List<TValue1>();
    [SerializeField] private List<TValue2> values2 = new List<TValue2>();
    public void OnAfterDeserialize()
    {
        this.Clear();
        for (int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], Tuple.Create(values1[i], values2[i]));
        }
        
    }

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values1.Clear();
        values2.Clear();
        foreach (KeyValuePair<TKey, Tuple<TValue1, TValue2>> pair in this)
        {
            keys.Add(pair.Key);
            values1.Add(pair.Value.Item1);
            values2.Add(pair.Value.Item2);
        }
    }



}
