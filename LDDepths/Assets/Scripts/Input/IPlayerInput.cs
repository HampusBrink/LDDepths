using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public interface IPlayerInput
    {
        void OnMove(Vector2 value)
        {
        }

        public void OnLook(Vector2 context)
        {
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
        }

        public void OnJump(InputAction.CallbackContext context)
        {
        }

        public void OnPrevious(InputAction.CallbackContext context)
        {
        }

        public void OnNext(InputAction.CallbackContext context)
        {
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
        }
    }

    public static class PlayerInputExtensions
    {
        public static void Subscribe(this IPlayerInput input) => InputManager.Subscribe(input);
        public static void UnSubscribe(this IPlayerInput input) => InputManager.UnSubscribe(input);
    }
}