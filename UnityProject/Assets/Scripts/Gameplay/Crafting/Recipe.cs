using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.Crafting
{
    [Serializable]
    public sealed class ItemAmount
    {
        public string id = string.Empty;
        public int amount = 1;
    }

    public enum StationType { Altar, Workbench, Kitchen }

    [CreateAssetMenu(menuName = "Game/Crafting/Recipe")]
    public sealed class Recipe : ScriptableObject
    {
        public string recipeId = string.Empty;
        public StationType station;
        public List<ItemAmount> inputs = new();
        public List<ItemAmount> outputs = new();
        public int sanityDelta;
    }
}
