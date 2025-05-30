using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericDatabaseBase : ScriptableObject
{
    /// <summary>
    /// Devuelve el Type concreto (T) que maneja esta DB.
    /// </summary>
    public abstract Type GetItemType();

    /// <summary>
    /// Recoge todos los items en forma de ScriptableObject.
    /// Podrás usar esto en el Editor genérico (por Reflection).
    /// </summary>
    public abstract List<ScriptableObject> GetAllItemsAsSO();

    /// <summary>
    /// Reemplaza la lista interna con la lista de items dada (sólo si son del tipo correcto).
    /// </summary>
    public abstract void SetItems(List<ScriptableObject> newItems);
}