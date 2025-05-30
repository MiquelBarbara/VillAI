using System;
using EntitiesRelated.Animation.Events;
using EntitiesRelated.Core;
using GameplayFocused.TilemapManagment.Crops;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;
using EntitiesRelated.Animation.Commands;


namespace GameplayFocused.TilemapManagment.Strategies
{
    /// <summary>
    /// Implements a tile action strategy for planting a seed on a farming tile.
    /// </summary>
    public class PlantTileStrategy : ITileActionStrategy
    {
        private readonly Tilemap _tilemap;
        private readonly TileBase _seededTile;
        private readonly GameObject _cropSpritePrefab;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlantTileStrategy"/> class.
        /// </summary>
        /// <param name="tilemap">The tilemap containing the farming tiles.</param>
        /// <param name="seededTile">The tile to set when a seed is planted.</param>
        /// <param name="cropSpritePrefab">The prefab used for the crop's visual representation.</param>
        public PlantTileStrategy(Tilemap tilemap, TileBase seededTile, GameObject cropSpritePrefab)
        {
            this._tilemap = tilemap;
            this._seededTile = seededTile;
            this._cropSpritePrefab = cropSpritePrefab;
        }

        /// <summary>
        /// Executes the planting action on the specified tile.
        /// Validates that the selected item is a seed, removes it from the character's inventory, and plants the crop.
        /// </summary>
        /// <param name="gridPos">The grid position of the tile to plant on.</param>
        /// <param name="data">The farming tile data for the tile.</param>
        /// <param name="character">The character performing the planting action.</param>
        public void Execute(Vector3Int gridPos, FarmingTilemapManager.FarmingTileData data, Character character)
        {
            // Retrieve the selected seed item from the character's toolbar.
            Item item = character.GetComponent<ToolbarController>().GetItem;

            if (item is not SeedItem seed) return;
            character.RemoveItem(seed, 1);

            data.state = FarmingTilemapManager.TileState.Seeded;
            CropTile cropTile = new CropTile
            {
                crop = seed.cropToPlant,
                growTimer = 0,
                growStage = 0,
                damage = 0,
                position = gridPos
            };
            data.cropTile = cropTile;

            // Instantiate the crop sprite object.
            Vector3 worldPos = _tilemap.CellToWorld(gridPos) + _tilemap.cellSize / 2;
            GameObject cropGO =
                Object.Instantiate(_cropSpritePrefab, worldPos, Quaternion.identity, _tilemap.transform);
            data.cropSpriteObject = cropGO;
            if (cropTile.crop.cropGrowData.Length > 0)
            {
                SpriteRenderer sr = cropGO.GetComponent<SpriteRenderer>();
                sr.sprite = cropTile.crop.cropGrowData[0].sprite;
            }

            _tilemap.SetTile(gridPos, _seededTile);
        }

        /// <summary>
        /// Returns the animation action and associated event data for planting.
        /// </summary>
        /// <returns>
        /// A tuple containing the animation action (<see cref="AnimationAction.doing"/>) and an empty array of <see cref="AnimationEventData"/>.
        /// </returns>
        public AnimationCommand GetAnimation()
        {
            return new DoingAnimationCommand();
        }
    }
}
