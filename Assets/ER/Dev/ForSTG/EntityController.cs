using UnityEngine;

namespace ER.STG
{
    public class EntityController : MonoBehaviour
    {
        private InputDefaultAction stg_actions;

        [SerializeField]
        private Entity role;

        private ActionGroup actions;

        private bool slowMode;
        private bool shooting;
        private Vector2 moveDir;

        private void Start()
        {
            actions = role.GetComponent<ActionGroup>();
        }

        private void OnEnable()
        {
            stg_actions = new InputDefaultAction();
            stg_actions.Enable();
            stg_actions.STG.Move.performed += (context) =>
            {
                moveDir = context.ReadValue<Vector2>();
                //Debug.Log($"触发移动: {moveDir}, slow: {slowMode}");
                role?.Move(moveDir, slowMode);
            };
            stg_actions.STG.Move.canceled += (context) =>
            {
                moveDir = Vector2.zero;
                role?.StopMove();
            };
            stg_actions.STG.Shoot.started += (context) =>
            {
                shooting = true;
            };
            stg_actions.STG.Shoot.canceled += (context) =>
            {
                shooting = false;
            };
            stg_actions.STG.SlowMode.started += (context) =>
            {
                if (role != null)
                {
                    slowMode = true;
                    role.Move(moveDir, slowMode);
                }
            };
            stg_actions.STG.SlowMode.canceled += (context) =>
            {
                if (role != null)
                {
                    slowMode = false;
                    role.Move(moveDir, slowMode);
                }
            };
            stg_actions.STG.Bomb.started += (context) =>
            {
                actions?.Bomb();
            };
            stg_actions.STG.Action_1.started += (context) =>
            {
                actions?.Action(1);
            };
            stg_actions.STG.Action_2.started += (context) =>
            {
                actions?.Action(2);
            };
            stg_actions.STG.Action_3.started += (context) =>
            {
                actions?.Action(3);
            };
        }

        private void Update()
        {
            if (role == null) return;
            if (shooting)
                actions.Shoot();
        }
    }
}