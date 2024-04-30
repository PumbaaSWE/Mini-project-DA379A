//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/Controll Systems/Player_Actions.inputactions
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

public partial class @Player_Actions : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @Player_Actions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Player_Actions"",
    ""maps"": [
        {
            ""name"": ""Player1"",
            ""id"": ""96f19bab-f769-4d5a-a825-d45c7f162985"",
            ""actions"": [
                {
                    ""name"": ""Test_Action"",
                    ""type"": ""Button"",
                    ""id"": ""7f322001-fbe3-45b8-ae09-fd051588b1ed"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Anti_Test_Action"",
                    ""type"": ""Button"",
                    ""id"": ""10fa3f8a-07d7-4028-9822-fedaa99e0e3e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""cc7fe957-e48a-4a86-a89f-718198c428e2"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Test_Action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3ec4ca95-caae-44b8-acec-e08908d534ac"",
                    ""path"": ""<Keyboard>/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Anti_Test_Action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player1
        m_Player1 = asset.FindActionMap("Player1", throwIfNotFound: true);
        m_Player1_Test_Action = m_Player1.FindAction("Test_Action", throwIfNotFound: true);
        m_Player1_Anti_Test_Action = m_Player1.FindAction("Anti_Test_Action", throwIfNotFound: true);
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

    // Player1
    private readonly InputActionMap m_Player1;
    private IPlayer1Actions m_Player1ActionsCallbackInterface;
    private readonly InputAction m_Player1_Test_Action;
    private readonly InputAction m_Player1_Anti_Test_Action;
    public struct Player1Actions
    {
        private @Player_Actions m_Wrapper;
        public Player1Actions(@Player_Actions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Test_Action => m_Wrapper.m_Player1_Test_Action;
        public InputAction @Anti_Test_Action => m_Wrapper.m_Player1_Anti_Test_Action;
        public InputActionMap Get() { return m_Wrapper.m_Player1; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(Player1Actions set) { return set.Get(); }
        public void SetCallbacks(IPlayer1Actions instance)
        {
            if (m_Wrapper.m_Player1ActionsCallbackInterface != null)
            {
                @Test_Action.started -= m_Wrapper.m_Player1ActionsCallbackInterface.OnTest_Action;
                @Test_Action.performed -= m_Wrapper.m_Player1ActionsCallbackInterface.OnTest_Action;
                @Test_Action.canceled -= m_Wrapper.m_Player1ActionsCallbackInterface.OnTest_Action;
                @Anti_Test_Action.started -= m_Wrapper.m_Player1ActionsCallbackInterface.OnAnti_Test_Action;
                @Anti_Test_Action.performed -= m_Wrapper.m_Player1ActionsCallbackInterface.OnAnti_Test_Action;
                @Anti_Test_Action.canceled -= m_Wrapper.m_Player1ActionsCallbackInterface.OnAnti_Test_Action;
            }
            m_Wrapper.m_Player1ActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Test_Action.started += instance.OnTest_Action;
                @Test_Action.performed += instance.OnTest_Action;
                @Test_Action.canceled += instance.OnTest_Action;
                @Anti_Test_Action.started += instance.OnAnti_Test_Action;
                @Anti_Test_Action.performed += instance.OnAnti_Test_Action;
                @Anti_Test_Action.canceled += instance.OnAnti_Test_Action;
            }
        }
    }
    public Player1Actions @Player1 => new Player1Actions(this);
    public interface IPlayer1Actions
    {
        void OnTest_Action(InputAction.CallbackContext context);
        void OnAnti_Test_Action(InputAction.CallbackContext context);
    }
}