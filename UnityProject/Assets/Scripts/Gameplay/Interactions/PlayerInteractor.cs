using UnityEngine;

namespace Game.Gameplay.Interactions
{
    [RequireComponent(typeof(Collider2D))]
    public sealed class PlayerInteractor : MonoBehaviour
    {
        [SerializeField] private KeyCode key = KeyCode.E;
        private Interactable _current;

        private void OnTriggerEnter2D(Collider2D other)
        {
            _current = other.GetComponent<Interactable>();
            if (_current != null)
            {
                Debug.Log($"{_current.Prompt}");
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (_current != null && other.GetComponent<Interactable>() == _current)
            {
                _current = null;
            }
        }

        private void Update()
        {
            if (_current != null && Input.GetKeyDown(key))
            {
                var ok = _current.Interact();
                Debug.Log(ok ? "Interaction success" : "Interaction failed");
            }
        }
    }
}
