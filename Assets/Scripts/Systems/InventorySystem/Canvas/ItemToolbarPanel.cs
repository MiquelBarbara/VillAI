using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
/// <summary>
/// Specialized <see cref="ItemPanel"/> that integrates with <see cref="ToolbarController"/> 
/// to highlight the selected toolbar slot.
/// </summary>
public class ItemToolbarPanel : ItemPanel
{
    [SerializeField]
    private ToolbarController toolbarController;

    private int currentSelectedTool;

    private void Start()
    {
        // Initialize slots in the base class
        Initialize();
        // When the toolbar selection changes, highlight the corresponding button
        toolbarController.onChange += Highlight;
        Highlight(0);
    }

    /// <summary>
    /// Invoked when a slot button is clicked; sets the toolbar's selected index and updates highlight.
    /// </summary>
    /// <param name="id">Index of the clicked slot.</param>
    public override void OnClick(int id)
    {
        toolbarController.Set(id);
        Highlight(id);
    }

    /// <summary>
    /// Highlights the chosen slot button and un-highlights the previously selected one.
    /// </summary>
    /// <param name="id">Index of the newly selected slot.</param>
    private void Highlight(int id)
    {
        buttons[currentSelectedTool].Highlight(false);
        currentSelectedTool = id;
        buttons[currentSelectedTool].Highlight(true);
    }

    /// <summary>
    /// Extends base Show method to also update the toolbar icon highlight.
    /// </summary>
    public override void Show()
    {
        base.Show();
        toolbarController.UpdateHightlightIcon();
    }
}