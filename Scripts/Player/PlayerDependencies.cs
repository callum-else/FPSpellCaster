using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CameraFOVMovementEnhancer))]
[RequireComponent(typeof(CameraTiltMovementEnhancer))]
[RequireComponent(typeof(FPMovementController))]
[RequireComponent(typeof(FPCameraRotationController))]
[RequireComponent(typeof(SpellContainer))]
[RequireComponent(typeof(FPHandAnimator))]
public class PlayerDependencies : MonoBehaviour, IPlayerInputMapperDependencies, ICameraFOVMovementEnhancerDependencies, ICameraTileMovementEnhancerDependencies, 
    IFPMovementControllerDependencies, IFPCameraRotationControllerDependencies, IFPHandAnimatorDependencies
{
    [Header("Child Components")]
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Transform _cameraLookAtTransform;
    [SerializeField] private Transform _xAxisTransform;
    [SerializeField] private Transform _yAxisTransform;
    [SerializeField] private Transform _zAxisTransform;
    [SerializeField] private Transform _handTransform;
    [SerializeField] private Transform _handContainerTransform;

    private Rigidbody _rigidbody;
    private CameraFOVMovementEnhancer _fovMovementEnhancer;
    private CameraTiltMovementEnhancer _tiltMovementEnhancer;
    private FPMovementController _movementController;
    private FPCameraRotationController _cameraRotationController;
    private SpellContainer _spellContainer;
    private FPHandAnimator _handAnimator;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _fovMovementEnhancer = GetComponent<CameraFOVMovementEnhancer>();
        _tiltMovementEnhancer = GetComponent<CameraTiltMovementEnhancer>();
        _movementController = GetComponent<FPMovementController>();
        _cameraRotationController = GetComponent<FPCameraRotationController>();
        _spellContainer = GetComponent<SpellContainer>();
        _handAnimator = GetComponent<FPHandAnimator>();
    }

    public Camera Camera => _camera;
    public Transform CameraTransform => _cameraTransform;
    public Transform CameraLookAtTransform => _cameraLookAtTransform;
    public Rigidbody Rigidbody => _rigidbody;
    public Transform XAxisTransform => _xAxisTransform;
    public Transform YAxisTransform => _yAxisTransform;
    public Transform ZAxisTransform => _zAxisTransform;
    public CameraFOVMovementEnhancer FOVMovementEnhancer => _fovMovementEnhancer;
    public CameraTiltMovementEnhancer TiltMovementEnhancer => _tiltMovementEnhancer;
    public FPMovementController FPMovementController => _movementController;
    public FPCameraRotationController FPCameraRotationController => _cameraRotationController;
    public SpellContainer SpellContainer => _spellContainer;
    public FPHandAnimator FPHandAnimator => _handAnimator;
    public Transform HandTransform => _handTransform;
    public Transform HandContainer => _handContainerTransform;
}
