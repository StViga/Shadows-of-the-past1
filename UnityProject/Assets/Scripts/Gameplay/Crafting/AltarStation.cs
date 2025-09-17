using Game.Core;
using Game.Gameplay.Crafting;
using Game.Gameplay.Interactions;
using UnityEngine;

namespace Game.Gameplay.CraftingStations
{
    public sealed class AltarStation : Interactable
    {
        [SerializeField] private CraftingSystem crafting = null!;
        [SerializeField] private Recipe candleRecipe; // optional asset

        private void EnsureRecipe()
        {
            if (candleRecipe != null) return;
            // Create an in-memory recipe for prototype
            candleRecipe = ScriptableObject.CreateInstance<Recipe>();
            candleRecipe.recipeId = "candle_fear";
            candleRecipe.station = StationType.Altar;
            candleRecipe.inputs.Add(new ItemAmount{ id = "fat", amount = 1 });
            candleRecipe.inputs.Add(new ItemAmount{ id = "thread", amount = 1 });
            candleRecipe.outputs.Add(new ItemAmount{ id = "candle", amount = 1 });
            candleRecipe.sanityDelta = -2;
        }

        public override string Prompt => "Craft Candle [E]";

        public override bool Interact()
        {
            EnsureRecipe();
            return crafting.Craft(candleRecipe);
        }
    }
}
