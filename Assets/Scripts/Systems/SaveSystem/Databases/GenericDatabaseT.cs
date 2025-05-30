using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class GenericDatabase<T> : GenericDatabaseBase where T : ScriptableObject
{
    [SerializeField]
    protected List<T> items = new List<T>();

    // Para accesos de solo lectura
    public List<T> Items => items;

    // Devuelve el Type real (T)
    public override Type GetItemType()
    {
        return typeof(T);
    }

    // Retorna la lista como ScriptableObject
    public override List<ScriptableObject> GetAllItemsAsSO()
    {
        var list = new List<ScriptableObject>();
        foreach (var item in items)
        {
            list.Add(item);
        }
        return list;
    }
    
    
    // Asigna la lista recibiendo ScriptableObjects, filtrando sólo los de tipo T
    public override void SetItems(List<ScriptableObject> newItems)
    {
        items.Clear();
        foreach (var obj in newItems)
        {
            if (obj is T tObj)
            {
                items.Add(tObj);
            }
        }
    }
}