using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Responsible for loading sprite animations from the Resources folder.
/// </summary>
public class AnimationLoader
{
    /// <summary>
    /// Loads animations for each <see cref="AnimationAction"/> using a given prefix.
    /// </summary>
    /// <param name="basePath">The base path where animations are stored.</param>
    /// <param name="prefix">
    /// The prefix to use when loading animations (e.g., "base", hair type, or "tools").
    /// </param>
    /// <returns>
    /// A dictionary mapping each <see cref="AnimationAction"/> to its corresponding array of sprites.
    /// </returns>
    public Dictionary<AnimationAction, Sprite[]> LoadAnimations(string basePath, string prefix)
    {
        Dictionary<AnimationAction, Sprite[]> animations = new Dictionary<AnimationAction, Sprite[]>();
        foreach (AnimationAction action in Enum.GetValues(typeof(AnimationAction)))
        {
            string path = $"{basePath}/{action}/{prefix}_{action}";
            Sprite[] sprites = LoadSpriteSheetAutoDetect(path);
            animations[action] = sprites;
        }
        return animations;
    }

    /// <summary>
    /// Loads sprites from the specified path, with a fallback to the parent directory if no sprites are found.
    /// </summary>
    /// <param name="path">The path from which to load sprites.</param>
    /// <returns>
    /// An array of loaded sprites. If no matching sprites are found, returns an empty array.
    /// </returns>
    public Sprite[] LoadSpriteSheetAutoDetect(string path)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(path);
        if (sprites.Length > 0)
            return sprites;

        string parentPath = Path.GetDirectoryName(path);
        Sprite[] allSprites = Resources.LoadAll<Sprite>(parentPath);
        Sprite[] matchingSprites = Array.FindAll(allSprites, s => s.name.StartsWith(Path.GetFileName(path)));
        return matchingSprites.Length > 0 ? matchingSprites : new Sprite[0];
    }
}
