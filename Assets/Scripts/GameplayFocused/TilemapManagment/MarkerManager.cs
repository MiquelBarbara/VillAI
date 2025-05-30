using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameplayFocused.TilemapManagment
{
    /// <summary>
    /// Manages a marker on a tilemap by placing a marker tile at a specified grid position.
    /// The marker updates only when its position changes.
    /// </summary>
    public class MarkerManager : MonoBehaviour
    {
        /// <summary>
        /// The Tilemap on which the marker will be placed.
        /// </summary>
        [SerializeField] private Tilemap tilemap;

        /// <summary>
        /// The TileBase representing the marker.
        /// </summary>
        [SerializeField] private TileBase markerTile;

        /// <summary>
        /// The current grid position for the marker.
        /// </summary>
        [SerializeField] private Vector3Int markerPosition;

        private Vector3Int _previousMarkerPosition;

        /// <summary>
        /// Gets or sets the current marker grid position.
        /// </summary>
        public Vector3Int MarkerPosition
        {
            get => markerPosition;
            set => markerPosition = value;
        }

        /// <summary>
        /// Updates the marker tile on the tilemap when its position changes.
        /// </summary>
        private void Update()
        {
            if (tilemap == null || markerTile == null)
                return;

            // Only update if the marker position has changed.
            if (markerPosition == _previousMarkerPosition) return;
            tilemap.SetTile(_previousMarkerPosition, null);
            tilemap.SetTile(markerPosition, markerTile);
            _previousMarkerPosition = markerPosition;
        }
    }
}