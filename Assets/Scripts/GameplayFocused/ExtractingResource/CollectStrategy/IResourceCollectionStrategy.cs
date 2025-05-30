using EntitiesRelated.Core;
using UnityEngine;

namespace GameplayFocused.ExtractingResource.CollectStrategy
{
    /// <summary>
    /// Defines a strategy for collecting resources.
    /// Implementations determine how a resource is delivered to the interactor.
    /// </summary>
    public interface IResourceCollectionStrategy
    {
        /// <summary>
        /// Collects the resource.
        /// </summary>
        /// <param name="interactor">The character that is interacting with the resource.</param>
        /// <param name="resourceItem">The item representing the resource to collect.</param>
        /// <param name="dropPosition">
        /// The position where the resource should be dropped, if applicable.
        /// </param>
        void Collect(Character interactor, Item resourceItem, Vector3 dropPosition);
    }
}