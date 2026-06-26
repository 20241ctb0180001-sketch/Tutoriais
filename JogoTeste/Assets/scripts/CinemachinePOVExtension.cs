using UnityEngine;
using Unity.Cinemachine;

public class CinemachinePOVExtension : CinemachineExtension
{
    [SerializeField] private float clampAngle = 85f;
    [SerializeField] private float horizontalSpeed = 10f;
    [SerializeField] private float verticalSpeed = 10f;

    private InputManager inputManager;
    private Vector3 startingRotation;

    protected override void Awake()
    {
        inputManager = InputManager.Instance;
        base.Awake();
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow && stage == CinemachineCore.Stage.Aim)
        {
            if (startingRotation == null) startingRotation = transform.localRotation.eulerAngles;

            Vector2 deltaInput = inputManager.GetMouseDelta();
            
            startingRotation.x += deltaInput.x * horizontalSpeed * Time.deltaTime;
            startingRotation.y += deltaInput.y * verticalSpeed * Time.deltaTime;

            // Limita a rotação vertical (olhar para cima e para baixo)
            startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngle, clampAngle);

            state.RawOrientation = Quaternion.Euler(-startingRotation.y, startingRotation.x, 0f);
        }
    }
}