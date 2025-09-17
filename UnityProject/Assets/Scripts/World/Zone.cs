using UnityEngine;

namespace Game.World
{
    public enum ZoneType { OldHouse, Hotel, CityNorth, CitySouth, CityWest, CityEast, Chapel, Market, Morgue, Library, School, Cafe, PhotoStudio, Neighbor }

    [DisallowMultipleComponent]
    public sealed class Zone : MonoBehaviour
    {
        public ZoneType type;
        [Tooltip("Optional minimap icon or debug color")] public Color debugColor = Color.white;
    }
}
