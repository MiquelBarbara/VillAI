using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Databases/Item Database")]
public class ItemDatabase : GenericDatabase<Item>
{
    /// <summary>
    /// Retorna el objeto T que coincida con 'name', o null si no existe.
    /// </summary>
    public virtual Item GetByName(string name)
    {
        return items.FirstOrDefault(item => item.name == name);
    }
    
}