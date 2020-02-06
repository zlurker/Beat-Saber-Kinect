using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iterator {
    public string n;
    public Type t;

    public Iterator() {
        if(string.IsNullOrEmpty(n))
            if(t != null)
                n = t.Name;
    }


    public static int ReturnKey(Iterator[] tA, string k) {
        for(int i = 0; i < tA.Length; i++)
            if(string.Equals(tA[i].n, k))
                return i;

        return -1;
    }

    public static int ReturnKey<T>(T[] tA, string k, Func<T, string> iI) {
        for(int i = 0; i < tA.Length; i++)
            if(string.Equals(iI(tA[i]), k))
                return i;

        return -1;
    }

    public static T ReturnObject<T>(T[] tA, Type k, Func<T, Type> iI) {

        for(int i = 0; i < tA.Length; i++)
            if(k == iI(tA[i]))
                return tA[i];

        return (T)(null as object);
    }

    public static T ReturnObject<T>(T[] tA, string k, Func<T, string> iI) {
        for(int i = 0; i < tA.Length; i++)
            if(string.Equals(iI(tA[i]), k))
                return (T)(tA[i] as object);

        return (T)(null as object);
    }

    public static T ReturnObject<T>(T[] tA, Func<T, bool> iI) {
        for(int i = 0; i < tA.Length; i++)
            if(iI(tA[i]))
                return (T)(tA[i] as object);

        return (T)(null as object);
    }

    public static T ReturnObject<T>(Iterator[] tA, string k) {
        for(int i = 0; i < tA.Length; i++)
            if(string.Equals(tA[i].n, k))
                return (T)(tA[i] as object);

        //Debug.LogErrorFormat("The key: {0} does not exist.", k);
        return (T)(null as object);
    }

    public static Iterator ReturnObject<T>(Iterator[] tA) {
        for(int i = 0; i < tA.Length; i++)
            if(tA[i].t == typeof(T))
                return (Iterator)(tA[i] as object);
        return (Iterator)(null as object);
    }

    public static Iterator ReturnObject(Iterator[] tA, Type t) {
        for(int i = 0; i < tA.Length; i++)
            if(tA[i].t == t)
                return (Iterator)(tA[i] as object);
        return (Iterator)(null as object);
    }
}