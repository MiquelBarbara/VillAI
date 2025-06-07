using UnityEngine;
using Utilities.EnumExtensions;

namespace LLM.Templates
{
    /// <summary>
    /// Represents a set of parameters for a language model, which can be used to configure its behavior.
    /// </summary>
    [CreateAssetMenu(fileName = "ModelParameters", menuName = "LLM/ModelParameters")]
    public class ModelParameters: EnumScriptableObject
    {
        [SerializeField] private string parameterName;
    }
}