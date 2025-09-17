using UnityEngine;

namespace Game.Gameplay.Interactions
{
    public abstract class Interactable : MonoBehaviour
    {
        [SerializeField] protected string prompt = "Interact [E]";
        public virtual string Prompt => prompt;
        public abstract bool Interact();
    }
}
