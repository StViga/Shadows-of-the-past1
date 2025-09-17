using UnityEngine;
using Game.Gameplay.Crafting;

namespace Game.Gameplay.CraftingStations
{
    public sealed class KitchenStation : Game.Gameplay.Interactions.Interactable
    {
        [SerializeField] private CraftingSystem crafting = null!;
        [SerializeField] private Recipe holyWaterRecipe;

        private void EnsureRecipe()
        {
            if (holyWaterRecipe != null) return;
            holyWaterRecipe = ScriptableObject.CreateInstance<Recipe>();
            holyWaterRecipe.recipeId = "holy_water";
            holyWaterRecipe.station = StationType.Kitchen;
            holyWaterRecipe.inputs.Add(new ItemAmount{ id = "water", amount = 1 });
            holyWaterRecipe.inputs.Add(new ItemAmount{ id = ResourceIds.Candle, amount = 1 });
            holyWaterRecipe.outputs.Add(new ItemAmount{ id = ResourceIds.HolyWater, amount = 1 });
            holyWaterRecipe.sanityDelta = +1;
        }

        public override string Prompt => "Brew Holy Water [E]";

        public override bool Interact()
        {
            EnsureRecipe();
            return crafting.Craft(holyWaterRecipe);
        }
    }
}
