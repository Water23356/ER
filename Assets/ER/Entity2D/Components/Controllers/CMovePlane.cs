using ER.ForEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ER.Entity2D.Components
{
    public class CMovePlane : ActionControllerBase
    {
        [SerializeField]
        [ReadOnly]
        private bool[] states;

        private AMovePlane m_handleAim;
        public override ActionBase handleAim { get => m_handleAim; protected set => m_handleAim = (AMovePlane)value; }

        public override void Init()
        {
            UpdateHandleAim();
            states = new bool[4];
            var actions = GlobalInput.Action.Plane2D;

            actions.MoveUp.performed += OnMovePerformed;
            actions.MoveUp.canceled += OnMoveCanceled;
            actions.MoveLeft.performed += OnMovePerformed;
            actions.MoveLeft.canceled += OnMoveCanceled;
            actions.MoveDown.performed += OnMovePerformed;
            actions.MoveDown.canceled += OnMoveCanceled;
            actions.MoveRight.performed += OnMovePerformed;
            actions.MoveRight.canceled += OnMoveCanceled;
        }

        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            int index = GetIndexFromAction(context.action);
            if (index != -1)
            {
                states[index] = true;
            }
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            int index = GetIndexFromAction(context.action);
            if (index != -1)
            {
                states[index] = false;
            }
        }

        private int GetIndexFromAction(InputAction action)
        {
            var actions = GlobalInput.Action.Plane2D;
            if (action == actions.MoveUp) return 0;
            if (action == actions.MoveLeft) return 1;
            if (action == actions.MoveDown) return 2;
            if (action == actions.MoveRight) return 3;
            return -1;
        }

        public override void UpdateHandleAim()
        {
            handleAim = entity.actAgent.Get<AMovePlane>();
        }

        private void Update()
        {
            Vector2 dirValue = Vector2.zero;
            if (states[0])
            {
                dirValue += Vector2.up;
            }
            if (states[2])
            {
                dirValue += Vector2.down;
            }
            if (states[1])
            {
                dirValue += Vector2.left;
            }
            if (states[3])
            {
                dirValue += Vector2.right;
            }
            m_handleAim.movDir = dirValue;
            UpdateActionControlState(dirValue != Vector2.zero);
        }

        public override void OnDestroy()
        {
            var actions = GlobalInput.Action.Plane2D;
            actions.MoveUp.performed -= OnMovePerformed;
            actions.MoveUp.canceled -= OnMoveCanceled;
            actions.MoveLeft.performed -= OnMovePerformed;
            actions.MoveLeft.canceled -= OnMoveCanceled;
            actions.MoveDown.performed -= OnMovePerformed;
            actions.MoveDown.canceled -= OnMoveCanceled;
            actions.MoveRight.performed -= OnMovePerformed;
            actions.MoveRight.canceled -= OnMoveCanceled;
        }
    }
}