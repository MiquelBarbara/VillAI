using Systems.SaveSystem.Databases;
using UnityEngine;

namespace Utilities.ScriptableObjectExtensions
{
    public class SaveSystemInitializer : MonoBehaviour
    {
        [SerializeField] private ScriptableSaveDataBase saveableDatabase;

        private void Start()
        {
            InitializeSaveSystems();
        }
        
        private void OnApplicationQuit()
        {
            SaveOnExit();
        }

        private void InitializeSaveSystems()
        {
            foreach (var saveable in saveableDatabase.Items)
            {
                saveable.InitializeSaveSystem();
            }
        }
        
        private void SaveOnExit()
        {
            foreach (var saveable in saveableDatabase.Items)
            {
                saveable.OnApplicationQuit();
            }
        }
    }

}