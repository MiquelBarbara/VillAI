using System.Collections.Generic;
using UnityEngine;

namespace Game.ALPHA
{
    /// <summary>
    /// Command interface for selecting a target from a list of detected objects.
    /// </summary>
    public interface ITargetSelectionCommand
    {
        T Execute<T>(List<T> detectables, Vector3 agentPosition) where T : Component;
    }
}