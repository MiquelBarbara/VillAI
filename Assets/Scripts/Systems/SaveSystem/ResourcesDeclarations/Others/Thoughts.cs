using System.Collections.Generic;
using UnityEngine;

namespace Systems.SaveSystem.Memory
{
    [CreateAssetMenu(fileName = "ThoughtsContainer", menuName = "Resources/ThoughtsContainer")]
    public class Thoughts: Summarizable<string> { }
}