using EntitiesRelated.Core;
using UnityEngine;

namespace GameplayFocused.ExtractingResource.Replacement
{
    /// <summary>
    /// Defines a teleportation strategy for moving a character relative to a resource.
    /// </summary>
    public interface ITeleportationStrategy
    {
        /// <summary>
        /// Teleports the specified character based on the resource's position.
        /// </summary>
        /// <param name="interactor">The character to teleport.</param>
        /// <param name="resourcePosition">The position of the resource triggering the teleport.</param>
        void Teleport(Character interactor, Vector3 resourcePosition);
    }
}