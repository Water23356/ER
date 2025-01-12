using ER.ForEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ER.Entity2D.Components
{
    public class CShootMouse : ActionControllerBase
    {
        private AShoot m_handleAim;

        [SerializeField]
        [DisplayLabel("事件相机")]
        private Camera camera;

        public override ActionBase handleAim { get => m_handleAim; protected set => m_handleAim = (AShoot)value; }

        public override void Init()
        {
            if (camera == null)
                camera = Camera.main;
            UpdateHandleAim();
            var actions = GlobalInput.Action.Plane2D;
            actions.Fire.performed += OnFirePerformed;
            actions.Fire.canceled += OnFireCanceled;
        }

        private void OnFirePerformed(InputAction.CallbackContext context)
        {
            UpdateActionControlState(true);
        }

        private void OnFireCanceled(InputAction.CallbackContext context)
        {
            UpdateActionControlState(false);
        }

        public override void UpdateHandleAim()
        {
            m_handleAim = entity.actAgent.Get<AShoot>();
        }

        private void Update()
        {
            var mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
            m_handleAim.shootDir = mousePos - entity.transform.position;
        }
    }
}