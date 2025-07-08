using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameplayFocused.TilemapManagment
{
    /// <summary>
    /// Provides functionality to process the growth of crops on the farming tilemap.
    /// </summary>
    public class CropGrowthService
    {
        /// <summary>
        /// Processes crop growth for each tile in the provided tile data map.
        /// Increases the grow timer, updates the crop's growth stage and sprite, and marks crops ready for harvest when complete.
        /// </summary>
        /// <param name="tileDataMap">
        /// A dictionary mapping grid positions to their corresponding farming tile data.
        /// </param>
        /// <param name="tilemap">The Tilemap containing the farming tiles.</param>
        public static void ProcessGrowth(Dictionary<Vector3Int, FarmingTilemapManager.FarmingTileData> tileDataMap)
        {
            foreach (var (gridPos, data) in tileDataMap)
            {
                if (data.state != FarmingTilemapManager.TileState.Seeded &&
                    data.state != FarmingTilemapManager.TileState.Growing) continue;
                if (data.cropTile == null)
                    continue;

                data.cropTile.growTimer += 1;

                if (data.cropTile.growStage < data.cropTile.crop.cropGrowData.Length){
                {
                    int stageTime = data.cropTile.crop.cropGrowData[data.cropTile.growStage].growthStageTime;
                    if (data.cropTile.growTimer >= stageTime)
                    {
                        data.cropTile.growStage++;
                        if (data.cropSpriteObject != null)
                        {
                            SpriteRenderer sr = data.cropSpriteObject.GetComponent<SpriteRenderer>();
                            int stageIndex = Mathf.Clamp(data.cropTile.growStage - 1, 0, data.cropTile.crop.cropGrowData.Length - 1);
                            sr.sprite = data.cropTile.crop.cropGrowData[stageIndex].sprite;
                        }
                    }
                }}

                if (data.cropTile.Complete)
                {
                    data.state = FarmingTilemapManager.TileState.ReadyToHarvest;
                    Debug.Log("Crop ready to harvest at: " + gridPos);
                }
                else if (data.state == FarmingTilemapManager.TileState.Seeded)
                {
                    data.state = FarmingTilemapManager.TileState.Growing;
                }
            }
        }
    }
}
