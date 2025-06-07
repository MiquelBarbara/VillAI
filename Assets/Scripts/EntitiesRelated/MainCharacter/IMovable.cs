using UnityEngine;

namespace EntitiesRelated.MainCharacter
{
    /// <summary>
    /// Interface for objects that can be moved in a 2D space.
    /// </summary>
    public interface IMovable
    {
        public Vector2 GetVector2D();
    }
}