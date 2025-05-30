using System.Collections.Generic;
using UnityEngine;
using Utilities.EnumExtensions;

namespace LLM.Templates
{
    
    [CreateAssetMenu(fileName = "ModelConfig", menuName = "LLM/ModelConfig")]
    public class ModelConfig: ScriptableObject
    {
        [SerializeField] public ModelDefinitionSO modelDefinition;
        [Range(0.5f, 1)] public float temperature = 0.8f;
        [SerializeField] public ModelParameters bParameters;
    }
}