using Game.Core;
using UnityEngine;

namespace Game.Gameplay.Crafting
{
    public sealed class CraftingSystem : MonoBehaviour
    {
        [SerializeField] private InventoryManager inventory = null;
        [SerializeField] private StatsManager stats = null;

        public bool CanCraft(Recipe recipe)
        {
            foreach (var i in recipe.inputs)
            {
                if (!inventory.Has(i.id, i.amount)) return false;
            }
            return true;
        }

        public bool Craft(Recipe recipe)
        {
            if (!CanCraft(recipe)) return false;

            foreach (var i in recipe.inputs)
            {
                inventory.Remove(i.id, i.amount);
            }

            foreach (var o in recipe.outputs)
            {
                inventory.Add(o.id, o.amount);
            }

            if (recipe.sanityDelta != 0)
            {
                stats.AddSanity(recipe.sanityDelta);
            }

            Debug.Log($"Crafted {recipe.recipeId}");
            return true;
        }
    }
}
