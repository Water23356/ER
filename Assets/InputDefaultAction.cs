//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/InputSystem/Default.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace ER
{
    public partial class @InputDefaultAction: IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @InputDefaultAction()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""Default"",
    ""maps"": [
        {
            ""name"": ""STG"",
            ""id"": ""eb23145a-fb9c-4d22-b680-b927275d0910"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""dbf73270-cb33-4d1d-a468-1f60c5c4112b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""SlowMode"",
                    ""type"": ""Button"",
                    ""id"": ""9167397f-a4c7-4151-a655-36fa103e29a3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""c80b0675-ca7f-48a7-934b-14893365f40e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Bomb"",
                    ""type"": ""Button"",
                    ""id"": ""136a7c52-9bec-4140-bad6-b8a7f2b23d6a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Action_1"",
                    ""type"": ""Button"",
                    ""id"": ""0285db47-1a17-454b-8edc-18ac3bc23b6d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Action_2"",
                    ""type"": ""Button"",
                    ""id"": ""d9394c17-dc6a-4a33-850d-609552c96afe"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Action_3"",
                    ""type"": ""Button"",
                    ""id"": ""0a9d47e4-a8d9-4aea-98b1-7f5e1bc3c61f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""db852120-6069-4cfa-b383-def51d901d61"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SlowMode"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""afce8f9b-de83-4c25-b6ca-47e74694b97b"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4a086000-6d62-4ec9-a8fb-35adcca4ec58"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Bomb"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""769934fe-5541-4255-9bfa-00ff793fd854"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Action_1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""677fc1d9-0d3d-450f-a7b5-1a7550e04c7b"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Action_2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3d3337e1-1758-472d-b407-54402e4c08dd"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Action_3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6ee624ce-23a2-4919-8bde-0d8e36483a6d"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""d573905c-8ae3-4fd3-861e-7c92675e7a9c"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""3ee2c527-43e0-48a2-bb99-09215e269322"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""a7f476e1-5404-41c5-8deb-ac6a444ee72e"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d564f3c5-d063-492e-9819-f51d0766ff52"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""3c465cfb-cdd3-460c-96e5-431cda047408"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector(2)"",
                    ""id"": ""72b0c6ca-086f-4206-82fc-a96f9fccadcf"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""1b52cb75-959a-4ee8-bae2-c7b890371722"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""42d36c5e-1a26-4563-8126-2b049939b991"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""9f2ffc9b-d8c4-413f-b03c-033f427fb76f"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""567fbbec-0c31-4a11-9010-ee3004b93a7d"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // STG
            m_STG = asset.FindActionMap("STG", throwIfNotFound: true);
            m_STG_Move = m_STG.FindAction("Move", throwIfNotFound: true);
            m_STG_SlowMode = m_STG.FindAction("SlowMode", throwIfNotFound: true);
            m_STG_Shoot = m_STG.FindAction("Shoot", throwIfNotFound: true);
            m_STG_Bomb = m_STG.FindAction("Bomb", throwIfNotFound: true);
            m_STG_Action_1 = m_STG.FindAction("Action_1", throwIfNotFound: true);
            m_STG_Action_2 = m_STG.FindAction("Action_2", throwIfNotFound: true);
            m_STG_Action_3 = m_STG.FindAction("Action_3", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        public IEnumerable<InputBinding> bindings => asset.bindings;

        public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
        {
            return asset.FindAction(actionNameOrId, throwIfNotFound);
        }

        public int FindBinding(InputBinding bindingMask, out InputAction action)
        {
            return asset.FindBinding(bindingMask, out action);
        }

        // STG
        private readonly InputActionMap m_STG;
        private List<ISTGActions> m_STGActionsCallbackInterfaces = new List<ISTGActions>();
        private readonly InputAction m_STG_Move;
        private readonly InputAction m_STG_SlowMode;
        private readonly InputAction m_STG_Shoot;
        private readonly InputAction m_STG_Bomb;
        private readonly InputAction m_STG_Action_1;
        private readonly InputAction m_STG_Action_2;
        private readonly InputAction m_STG_Action_3;
        public struct STGActions
        {
            private @InputDefaultAction m_Wrapper;
            public STGActions(@InputDefaultAction wrapper) { m_Wrapper = wrapper; }
            public InputAction @Move => m_Wrapper.m_STG_Move;
            public InputAction @SlowMode => m_Wrapper.m_STG_SlowMode;
            public InputAction @Shoot => m_Wrapper.m_STG_Shoot;
            public InputAction @Bomb => m_Wrapper.m_STG_Bomb;
            public InputAction @Action_1 => m_Wrapper.m_STG_Action_1;
            public InputAction @Action_2 => m_Wrapper.m_STG_Action_2;
            public InputAction @Action_3 => m_Wrapper.m_STG_Action_3;
            public InputActionMap Get() { return m_Wrapper.m_STG; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(STGActions set) { return set.Get(); }
            public void AddCallbacks(ISTGActions instance)
            {
                if (instance == null || m_Wrapper.m_STGActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_STGActionsCallbackInterfaces.Add(instance);
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @SlowMode.started += instance.OnSlowMode;
                @SlowMode.performed += instance.OnSlowMode;
                @SlowMode.canceled += instance.OnSlowMode;
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
                @Bomb.started += instance.OnBomb;
                @Bomb.performed += instance.OnBomb;
                @Bomb.canceled += instance.OnBomb;
                @Action_1.started += instance.OnAction_1;
                @Action_1.performed += instance.OnAction_1;
                @Action_1.canceled += instance.OnAction_1;
                @Action_2.started += instance.OnAction_2;
                @Action_2.performed += instance.OnAction_2;
                @Action_2.canceled += instance.OnAction_2;
                @Action_3.started += instance.OnAction_3;
                @Action_3.performed += instance.OnAction_3;
                @Action_3.canceled += instance.OnAction_3;
            }

            private void UnregisterCallbacks(ISTGActions instance)
            {
                @Move.started -= instance.OnMove;
                @Move.performed -= instance.OnMove;
                @Move.canceled -= instance.OnMove;
                @SlowMode.started -= instance.OnSlowMode;
                @SlowMode.performed -= instance.OnSlowMode;
                @SlowMode.canceled -= instance.OnSlowMode;
                @Shoot.started -= instance.OnShoot;
                @Shoot.performed -= instance.OnShoot;
                @Shoot.canceled -= instance.OnShoot;
                @Bomb.started -= instance.OnBomb;
                @Bomb.performed -= instance.OnBomb;
                @Bomb.canceled -= instance.OnBomb;
                @Action_1.started -= instance.OnAction_1;
                @Action_1.performed -= instance.OnAction_1;
                @Action_1.canceled -= instance.OnAction_1;
                @Action_2.started -= instance.OnAction_2;
                @Action_2.performed -= instance.OnAction_2;
                @Action_2.canceled -= instance.OnAction_2;
                @Action_3.started -= instance.OnAction_3;
                @Action_3.performed -= instance.OnAction_3;
                @Action_3.canceled -= instance.OnAction_3;
            }

            public void RemoveCallbacks(ISTGActions instance)
            {
                if (m_Wrapper.m_STGActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(ISTGActions instance)
            {
                foreach (var item in m_Wrapper.m_STGActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_STGActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public STGActions @STG => new STGActions(this);
        public interface ISTGActions
        {
            void OnMove(InputAction.CallbackContext context);
            void OnSlowMode(InputAction.CallbackContext context);
            void OnShoot(InputAction.CallbackContext context);
            void OnBomb(InputAction.CallbackContext context);
            void OnAction_1(InputAction.CallbackContext context);
            void OnAction_2(InputAction.CallbackContext context);
            void OnAction_3(InputAction.CallbackContext context);
        }
    }
}
