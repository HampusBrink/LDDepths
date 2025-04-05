using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    [CreateAssetMenu(menuName = "InputManager")]
    public class InputManager : ScriptableObject, InputActions.IPlayerActions, InputActions.IUIActions
    {
        private static InputActions _inputSystemActions;
        private static InputManager _instance;

        private static readonly List<IPlayerInput> PlayerActions = new();
        private static readonly List<IUIInput> UIActions = new();

        public static void Subscribe(IPlayerInput input)
        {
            _inputSystemActions ??= new InputActions();
            _inputSystemActions.Player.SetCallbacks(_instance);
            _inputSystemActions.Player.Enable();

            PlayerActions.Add(input);
        }

        public static void UnSubscribe(IPlayerInput input)
        {
            _inputSystemActions.Player.RemoveCallbacks(_instance);
            _inputSystemActions.Player.Enable();

            PlayerActions.Remove(input);
        }

        public static void Subscribe(IUIInput input)
        {
            _inputSystemActions ??= new InputActions();
            _inputSystemActions.UI.SetCallbacks(_instance);
            _inputSystemActions.UI.Enable();

            UIActions.Add(input);
        }

        public static void UnSubscribe(IUIInput input)
        {
            _inputSystemActions.UI.RemoveCallbacks(_instance);
            _inputSystemActions.UI.Enable();

            UIActions.Remove(input);
        }

        private void OnEnable()
        {
            _instance = this;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            foreach (var playerAction in PlayerActions)
            {
                playerAction.OnMove(context.ReadValue<Vector2>());
            }
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            foreach (var playerAction in PlayerActions)
            {
                playerAction.OnLook(context.ReadValue<Vector2>());
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            foreach (var playerAction in PlayerActions)
            {
                playerAction.OnAttack(context);
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            foreach (var playerAction in PlayerActions)
            {
                playerAction.OnInteract(context);
            }
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            foreach (var playerAction in PlayerActions)
            {
                playerAction.OnCrouch(context);
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            foreach (var playerAction in PlayerActions)
            {
                playerAction.OnJump(context);
            }
        }

        public void OnPrevious(InputAction.CallbackContext context)
        {
            foreach (var playerAction in PlayerActions)
            {
                playerAction.OnPrevious(context);
            }
        }

        public void OnNext(InputAction.CallbackContext context)
        {
            foreach (var playerAction in PlayerActions)
            {
                playerAction.OnNext(context);
            }
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            foreach (var playerAction in PlayerActions)
            {
                playerAction.OnSprint(context);
            }
        }

        public void OnNavigate(InputAction.CallbackContext context)
        {
            foreach (var uiAction in UIActions)
            {
                uiAction.OnNavigate(context);
            }
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            foreach (var uiAction in UIActions)
            {
                uiAction.OnSubmit(context);
            }
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            foreach (var uiAction in UIActions)
            {
                uiAction.OnCancel(context);
            }
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
            foreach (var uiAction in UIActions)
            {
                uiAction.OnPoint(context);
            }
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            foreach (var uiAction in UIActions)
            {
                uiAction.OnClick(context);
            }
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
            foreach (var uiAction in UIActions)
            {
                uiAction.OnRightClick(context);
            }
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
            foreach (var uiAction in UIActions)
            {
                uiAction.OnMiddleClick(context);
            }
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
            foreach (var uiAction in UIActions)
            {
                uiAction.OnScrollWheel(context);
            }
        }

        public void OnTrackedDevicePosition(InputAction.CallbackContext context)
        {
            foreach (var uiAction in UIActions)
            {
                uiAction.OnTrackedDevicePosition(context);
            }
        }

        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
        {
            foreach (var uiAction in UIActions)
            {
                uiAction.OnTrackedDeviceOrientation(context);
            }
        }
    }
}