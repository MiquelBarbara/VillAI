using UnityEngine;
using UnityEngine.InputSystem;

//
/// <summary>
/// Toggles between showing the full inventory panel and the shorter toolbar UI.
/// </summary>
public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;       // Full inventory panel
    
    [SerializeField]
    private GameObject toolbarPanel; // Toolbar/hotbar panel

    /// <summary>
    /// Toggles the inventory panel in response to an input action.
    /// </summary>
    /// <param name="context">Unity InputSystem context.</param>
    public void ToggleInventory(InputAction.CallbackContext context)
    {
        if (panel.activeInHierarchy == false)
        {
            OpenInventory();
            CloseToolbar();
        }
        else
        {
            CloseInventory();
            OpenToolbar();
        }
    }
    
    public void OpenInventory()
    {
        panel.SetActive(true);
    }
    
    public void CloseInventory()
    {
        panel.SetActive(false);
    }
    
    public void OpenToolbar()
    {
        toolbarPanel.SetActive(true);
    }
    
    public void CloseToolbar()
    {
        toolbarPanel.SetActive(false);
    }
}