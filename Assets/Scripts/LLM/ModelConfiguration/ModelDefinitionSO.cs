using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities.EnumExtensions;

namespace LLM.Templates
{
    /// <summary>
    /// Represents a definition for a language model, including its name, available parameters, and description.
    /// </summary>
    [CreateAssetMenu(fileName = "ModelDefinition", menuName = "LLM/ModelDefinition")]
    public class ModelDefinitionSO : EnumScriptableObject
    {
        [SerializeField] public string modelName;

        [SerializeField] private List<ModelParameters> availableParameters;

        [SerializeField] private string modelDescription;
    }
}