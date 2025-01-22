using Players;
using UnityEngine;
using UnityEngine.InputSystem;
using World;

public class CameraDrag : MonoBehaviour
{
    private Vector3 GetMousePosition => _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (!_isDragging) return;

        _difference = GetMousePosition - transform.position;
        transform.position = _origin - _difference;
        if (_mainCamera == null || PlayerManager.LocalPlayer == null) return;
        PlayerManager.LocalPlayer.Position = _mainCamera.transform.position;
    }

    public void OnDrag(InputAction.CallbackContext ctx)
    {
        if (ctx.started) _origin = GetMousePosition;
        _isDragging = ctx.started || ctx.performed;
    }

    #region Variables

    private Vector3 _origin;
    private Vector3 _difference;

    private Camera _mainCamera;

    private bool _isDragging;

    #endregion
}