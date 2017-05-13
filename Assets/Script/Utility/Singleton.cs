﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTon<T> where T : new()
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
                _instance = new T();

            return _instance;
        }

        private set { }
    }
}
