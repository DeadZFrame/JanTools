using Jan.Core;
using Jan.Events;
using UnityEngine;

public class FPSControls : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveSpeed = 5f;

    private Vector2 _moveInput;
    private FPSCamera _fpsCamera;

    void OnEnable()
    {
        EventManager.Register<Vector2>(EventNames.OnMoveInput, OnMoveInput);
    }

    void OnDisable()
    {
        EventManager.UnRegister<Vector2>(EventNames.OnMoveInput, OnMoveInput);
    }

    void Start()
    {
        _fpsCamera = CameraManager.GetCamera() as FPSCamera;
    }

    void FixedUpdate()
    {
        if(GameStateManager.Instance.CurrentGameState != GameState.FPS) return;
        
        Vector3 move = new Vector3(_moveInput.x, 0f, _moveInput.y);
        move = Vector3.ClampMagnitude(move, 1f);

        Vector3 worldMove = _fpsCamera.CameraComponent.transform.rotation * move;
        rb.MovePosition(rb.position + worldMove * (moveSpeed * Time.fixedDeltaTime));
    }

    private void OnMoveInput(Vector2 moveInput)
    {
        _moveInput = moveInput;
    }
}
