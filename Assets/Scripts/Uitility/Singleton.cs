using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> where T : class, new()
{
    private static T _instance;
    public static T Instance
    {
        get { return _instance ??= new T(); }
        protected set { _instance = value; }
    }

    protected Singleton() { }
}