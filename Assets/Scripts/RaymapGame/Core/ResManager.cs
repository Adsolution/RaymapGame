//================================
//  By: Adsolution
//================================

using System.Collections.Generic;
using UnityEngine;

public static class ResManager
{
    static Dictionary<string, Object[]> resources = new Dictionary<string, Object[]>();
    static Dictionary<string, GameObject> findCache = new Dictionary<string, GameObject>();

    public static T Get<T>(string path) where T : Object {
        if (resources.ContainsKey(path))
            return (T)resources[path][0];

        var res = Resources.Load<T>($"{nameof(RaymapGame)}/{path}");
        resources.Add(path, new Object[] { res });
        return res;
    }
    public static T[] GetAll<T>(string path) where T : Object {
        if (resources.ContainsKey(path))
            return (T[])resources[path];

        var res = Resources.LoadAll<T>($"{nameof(RaymapGame)}/{path}");
        resources.Add(path, res);
        return res;
    }

    public static GameObject Inst(string path)
        => Object.Instantiate(Get<GameObject>(path));
    public static GameObject Inst(string path, Component parent)
        => Object.Instantiate(Get<GameObject>(path), parent.transform, false);

    public static T Inst<T>(string path)
        => Object.Instantiate(Get<GameObject>(path)).GetComponent<T>();
    public static T Inst<T>(string path, Component parent)
        => Object.Instantiate(Get<GameObject>(path), parent.transform, false).GetComponent<T>();


    public static GameObject[] InstAll(string path) {
        var all = GetAll<GameObject>(path);
        foreach (var o in all)
            Object.Instantiate(o);
        return all;
    }
    public static GameObject[] InstAll(string path, Component parent) {
        var all = GetAll<GameObject>(path);
        foreach (var o in all)
            Object.Instantiate(o, parent.transform, false);
        return all;
    }


    public static GameObject Find(string name) {
        if (findCache.ContainsKey(name))
            return findCache[name];

        var obj = GameObject.Find(name);
        findCache.Add(name, obj);
        return obj;
    }
}