using UnityEngine;
using Game.Gameplay.Crafting;

namespace Game.Gameplay.CraftingStations
{
    public sealed class WorkbenchStation : Game.Gameplay.Interactions.Interactable
    {
        [SerializeField] private CraftingSystem crafting = null!;
        [SerializeField] private Recipe lockpickRecipe;

        private void EnsureRecipe()
        {
            if (lockpickRecipe != null) return;
            lockpickRecipe = ScriptableObject.CreateInstance<Recipe>();
            lockpickRecipe.recipeId = "lockpick";
            lockpickRecipe.station = StationType.Workbench;
            lockpickRecipe.inputs.Add(new ItemAmount{ id = ResourceIds.Thread, amount = 1 });
            lockpickRecipe.inputs.Add(new ItemAmount{ id = "scrap", amount = 1 });
            lockpickRecipe.outputs.Add(new ItemAmount{ id = ResourceIds.Lockpick, amount = 1 });
            lockpickRecipe.sanityDelta = 0;
        }

        public override string Prompt => "Craft Lockpick [E]";

        public override bool Interact()
        {
            EnsureRecipe();
            return crafting.Craft(lockpickRecipe);
        }
    }
}
