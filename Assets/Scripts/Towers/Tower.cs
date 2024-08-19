using System;
using UnityEngine;

// create a simple struct/class for Tower objects to more easily create more types of towers later down the line in development
[Serializable]
public class Tower 
{

    public string name;
    public int cost;
    public GameObject prefab;

    public Tower(string _name, int _cost,  GameObject _prefab)
    {
        name = _name;
        cost = _cost;
        prefab = _prefab;

    }

}
