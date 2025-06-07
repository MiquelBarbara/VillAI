using System.Collections.Generic;
using UnityEngine;
using Utilities.EnumExtensions;

namespace LLM.Templates
{
    /// <summary>
    /// Represents a configuration for a language model, including its definition, temperature setting, and parameters.
    /// </summary>
    [CreateAssetMenu(fileName = "ModelConfig", menuName = "LLM/ModelConfig")]
    public class ModelConfig: ScriptableObject
    {
        [SerializeField] public ModelDefinitionSO modelDefinition;
        [Range(0.5f, 1)] public float temperature = 0.8f;
        [SerializeField] public ModelParameters bParameters;
    }
}