using UnityEngine;
using Utilities.EnumExtensions;

namespace LLM.Templates
{
    [CreateAssetMenu(fileName = "ModelParameters", menuName = "LLM/ModelParameters")]
    public class ModelParameters: EnumScriptableObject
    {
        [SerializeField] private string parameterName;
    }
}