using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IPlayerInputMapperDependencies
{
    CameraFOVMovementEnhancer FOVMovementEnhancer { get; }
    CameraTiltMovementEnhancer TiltMovementEnhancer { get; }
    FPMovementController FPMovementController { get; }
    FPCameraRotationController FPCameraRotationController { get; }
    SpellContainer SpellContainer { get; }
    FPHandAnimator FPHandAnimator { get; }
}

[RequireComponent(typeof(IPlayerInputMapperDependencies))]
public class PlayerInputMapper : InputEventMapper
{
    private IPlayerInputMapperDependencies _dependencies;

    protected override void OnInitialize()
    {
        _dependencies = GetComponent<IPlayerInputMapperDependencies>();
        Subscribe(InputActions.Move, BindablePhase.All, PublishMovementInput);
        Subscribe(InputActions.Look, BindablePhase.All, PublishLookInput);
        Subscribe(InputActions.InputPrimary, BindablePhase.All, PublishInputPrimary);
        Subscribe(InputActions.InputSecondary, BindablePhase.All, PublishInputSecondary);
    }

    private void OnDestroy()
    {
        if (IsInitialized)
        {
            Unsubscribe(InputActions.Move, BindablePhase.All, PublishMovementInput);
            Unsubscribe(InputActions.Look, BindablePhase.All, PublishLookInput);
            Unsubscribe(InputActions.InputPrimary, BindablePhase.All, PublishInputPrimary);
            Unsubscribe(InputActions.InputSecondary, BindablePhase.All, PublishInputSecondary);
        }
    }

    private void PublishMovementInput(InputAction.CallbackContext context)
    {
        var inputValue = context.ReadValue<Vector2>();
        _dependencies.FPMovementController.SetInput(inputValue);
        _dependencies.TiltMovementEnhancer.SetInput(inputValue);
        _dependencies.FOVMovementEnhancer.SetInput(inputValue.y);
    }

    private void PublishLookInput(InputAction.CallbackContext context)
    {
        _dependencies.FPCameraRotationController.SetInput(context.ReadValue<Vector2>());
    }

    private void PublishInputPrimary(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                _dependencies.SpellContainer.StartInputPrimary();
                _dependencies.FPHandAnimator.OnInputStart();
                break;

            case InputActionPhase.Canceled:
                _dependencies.SpellContainer.ReleaseInputPrimary();
                _dependencies.FPHandAnimator.OnInputRelease();
                break;
        }
    }

    private void PublishInputSecondary(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                _dependencies.SpellContainer.StartInputSecondary();
                _dependencies.FPHandAnimator.OnInputStart();
                break;

            case InputActionPhase.Canceled:
                _dependencies.SpellContainer.ReleaseInputSecondary();
                _dependencies.FPHandAnimator.OnInputRelease();
                break;
        }
    }
}
