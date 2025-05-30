using System;
using System.Collections.Generic;
using EntitiesRelated.Animation.Commands;
using EntitiesRelated.Animation.Events;
using EntitiesRelated.Core;
using GameplayFocused.TilemapManagment.Crops;
using GameplayFocused.TilemapManagment.Strategies;
using GameplayFocused.TimeManagment;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace GameplayFocused.TilemapManagment
{
    /// <summary>
    /// Manages the farming tilemap including crop growth, tile interactions, and player actions.
    /// Utilizes a chain-of-responsibility pattern via a ChainTileActionResolver and separates crop growth logic into a CropGrowthService.
    /// </summary>
    public class FarmingTilemapManager : TimeAgent
    {
        /// <summary>
        /// Gets the singleton instance of the FarmingTilemapManager.
        /// </summary>
        public static FarmingTilemapManager Instance;

        /// <summary>
        /// Represents the different states a farming tile can be in.
        /// </summary>
        public enum TileState { Unplowed, Plowed, Watered, Seeded, Growing, ReadyToHarvest }

        /// <summary>
        /// Contains data for an individual farming tile.
        /// </summary>
        [Serializable]
        public class FarmingTileData
        {
            /// <summary>
            /// The current state of the tile.
            /// </summary>
            public TileState state;

            /// <summary>
            /// The crop tile associated with this tile.
            /// </summary>
            public CropTile cropTile;

            /// <summary>
            /// The GameObject used to display the crop sprite.
            /// </summary>
            public GameObject cropSpriteObject;
        }

        [SerializeField] private Tilemap tilemap;
        [SerializeField] private TileBase unplowedTile;
        [SerializeField] private TileBase plowedTile;
        [SerializeField] private TileBase wateredTile;
        [SerializeField] private TileBase seededTile;
        [SerializeField] private GameObject cropSpritePrefab;
        [SerializeField] private float tickInterval = 1f;

        private readonly Dictionary<Vector3Int, FarmingTileData> _tileDataMap = new Dictionary<Vector3Int, FarmingTileData>();
        private ToolbarController _toolbar;
    
        // Chain-of-responsibility based tile action resolver.
        private ChainTileActionResolver _tileActionResolver;
    
        // Service handling crop growth logic.
        private CropGrowthService _cropGrowthService;

        /// <summary>
        /// Initializes the FarmingTilemapManager instance, sets up the tile data, and configures the tile action resolver and crop growth service.
        /// </summary>
        protected void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            if (tilemap == null)
                tilemap = GetComponent<Tilemap>();

            _toolbar = FindObjectOfType<ToolbarController>();

            InitializeTileData();
            OnTimeTick += Tick;
        
            _cropGrowthService = new CropGrowthService();
            _tileActionResolver = new ChainTileActionResolver(tilemap, plowedTile, wateredTile, seededTile, cropSpritePrefab, _toolbar);
        }

        /// <summary>
        /// Initializes the tile data map by iterating over all tiles in the tilemap bounds and setting them to the Unplowed state.
        /// </summary>
        private void InitializeTileData()
        {
            BoundsInt bounds = tilemap.cellBounds;
            for (var x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (var y = bounds.yMin; y < bounds.yMax; y++)
                {
                    var pos = new Vector3Int(x, y, 0);
                    if (!tilemap.HasTile(pos)) continue;
                    FarmingTileData data = new FarmingTileData { state = TileState.Unplowed };
                    _tileDataMap[pos] = data;
                    tilemap.SetTile(pos, unplowedTile);
                }
            }
        }

        /// <summary>
        /// Invoked on each tick to process crop growth across all tiles.
        /// </summary>
        private void Tick()
        {
            CropGrowthService.ProcessGrowth(_tileDataMap);
        }

        /// <summary>
        /// Processes the interaction for a character based on their current position in the tilemap.
        /// </summary>
        /// <param name="character">The character interacting with the tilemap.</param>
        public void Interact(Character character)
        {
            Vector3Int gridPos = tilemap.WorldToCell(character.transform.position);
            if (!_tileDataMap.TryGetValue(gridPos, out var data))
                return;

            ITileActionStrategy strategy = _tileActionResolver.Resolve(data);
            if (strategy != null)
            {
                strategy.Execute(gridPos, data, character);
            }
            else
            {
                Debug.Log("No action determined for tile at: " + gridPos);
            }
        }

        /// <summary>
        /// Retrieves an interactable wrapper for the tile located at the given world point.
        /// </summary>
        /// <param name="worldPoint">The world position to check for a tile.</param>
        /// <returns>
        /// An <see cref="Interactable"/> wrapper for the tile if found; otherwise, null.
        /// </returns>
        public Interactable GetTileWrapperAtPoint(Vector2 worldPoint)
        {
            Vector3Int gridPos = tilemap.WorldToCell(worldPoint);
            return _tileDataMap.TryGetValue(gridPos, out var data) ? new FarmingTileWrapper(gridPos, data) : null;
        }

        /// <summary>
        /// Processes an interaction at a specific tile position.
        /// </summary>
        /// <param name="gridPos">The grid position of the tile.</param>
        /// <param name="data">The tile data for the specified position.</param>
        /// <param name="character">The character interacting with the tile.</param>
        public void InteractAtTile(Vector3Int gridPos, FarmingTileData data, Character character)
        {
            ITileActionStrategy strategy = _tileActionResolver.Resolve(data);
            if (strategy != null)
            {
                strategy.Execute(gridPos, data, character);
            }
            else
            {
                Debug.Log("No action determined for tile at: " + gridPos);
            }
        }

        /// <summary>
        /// Retrieves the animation for the action associated with a given tile.
        /// </summary>
        /// <param name="data">The tile data for which to get the animation.</param>
        /// <returns>
        /// A tuple containing the <see cref="AnimationAction"/> and an array of <see cref="AnimationEventData"/>.
        /// </returns>
        public AnimationCommand GetAnimationForTile(FarmingTileData data)
        {
            ITileActionStrategy strategy = _tileActionResolver.Resolve(data);
            return strategy != null ? strategy.GetAnimation() : (new IdleAnimationCommand());
        }
    }
}
