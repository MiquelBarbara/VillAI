#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class TypeSearchWindow : EditorWindow
{
    private string _searchString = "";
    private Vector2 _scrollPos;

    private Action<string> _onTypeSelected;
    private bool _allowLists;

    // Para las pestañas
    private int _selectedTabIndex = 0;
    private readonly string[] _tabs = { "Primitives", "Lists", "Project Types" };

    // Listas internas de opciones
    private List<(string displayName, string fullName, string searchData)> _primitives = new();
    private List<(string displayName, string fullName, string searchData)> _lists = new();
    private List<(string displayName, string fullName, string searchData)> _projectTypes = new();

    /// <summary>
    /// Abre la ventana de búsqueda.
    /// </summary>
    public static void ShowWindow(Action<string> onTypeSelected, bool allowLists)
    {
        var window = CreateInstance<TypeSearchWindow>();
        window.titleContent = new GUIContent("Seleccionar Tipo");
        window._onTypeSelected = onTypeSelected;
        window._allowLists = allowLists;
        window.InitTypes();
        window.ShowUtility();
    }

    /// <summary>
    /// Inicializa la lista de tipos en 3 categorías:
    /// 1) Primitives
    /// 2) Lists
    /// 3) Project Types
    /// </summary>
    private void InitTypes()
    {
        // 1. Tipos primitivos que deseas exponer
        var basicTypes = new[]
        {
            typeof(int), typeof(float), typeof(bool),
            typeof(double), typeof(long), typeof(short),
            typeof(string)
        };

        // 2. Clases marcadas con [AllowAsParameterAttribute]
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var allowedClasses = assemblies.SelectMany(asm => asm.GetTypes())
            .Where(t => t.IsClass && t.IsPublic && !t.IsAbstract 
                        && t.GetCustomAttribute<AllowAsParameterAttribute>() != null)
            .OrderBy(t => t.Name) // Ordena por nombre corto
            .ToList();

        // Llenar la lista de primitivos
        foreach (var t in basicTypes)
        {
            string displayName = t.Name;            // p.ej. "Int32" => se podría mejorar a "int"
            string fullName = t.FullName;           // p.ej. "System.Int32"
            string searchData = t.FullName;         // lo usamos para filtrar
            _primitives.Add((displayName, fullName, searchData));
        }

        // Llenar la lista de project types
        foreach (var t in allowedClasses)
        {
            // p.ej. "TalkSession" y "MyNamespace.TalkSession"
            string displayName = t.Name;    
            string fullName = t.FullName;    
            string searchData = t.FullName;  
            _projectTypes.Add((displayName, fullName, searchData));
        }

        // Generar la lista de List<T> si se permite
        if (_allowLists)
        {
            // Listas de primitivos
            foreach (var p in _primitives)
            {
                string listDisplayName = $"List<{p.displayName}>";
                // Formato para List<T>: "System.Collections.Generic.List`1[[{p.fullName}]]"
                string listFullName = $"System.Collections.Generic.List`1[[{p.fullName}]]";
                string listSearchData = listFullName; 
                _lists.Add((listDisplayName, listFullName, listSearchData));
            }
            // Listas de project types
            foreach (var pt in _projectTypes)
            {
                string listDisplayName = $"List<{pt.displayName}>";
                string listFullName = $"System.Collections.Generic.List`1[[{pt.fullName}]]";
                string listSearchData = listFullName;
                _lists.Add((listDisplayName, listFullName, listSearchData));
            }
        }
    }

    private void OnGUI()
    {
        // Pestañas
        _selectedTabIndex = GUILayout.Toolbar(_selectedTabIndex, _tabs);

        // Campo de búsqueda
        EditorGUILayout.LabelField("Buscar Tipo", EditorStyles.boldLabel);
        _searchString = EditorGUILayout.TextField(_searchString);
        EditorGUILayout.Space();

        // Decidir qué lista mostrar según la pestaña
        var currentList = _selectedTabIndex switch
        {
            0 => _primitives,
            1 => _lists,
            2 => _projectTypes,
            _ => _primitives
        };

        // Filtrar según la cadena de búsqueda
        var filteredList = currentList
            .Where(o => string.IsNullOrEmpty(_searchString)
                     || o.searchData.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0
                     || o.displayName.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) >= 0)
            .OrderBy(o => o.displayName);

        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
        foreach (var opt in filteredList)
        {
            if (GUILayout.Button(opt.displayName, EditorStyles.miniButton))
            {
                _onTypeSelected?.Invoke(opt.fullName);
                Close();
            }
        }
        EditorGUILayout.EndScrollView();
    }
}
#endif
