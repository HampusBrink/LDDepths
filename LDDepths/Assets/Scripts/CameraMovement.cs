using System;
using Player;
using UnityEngine;

public class CameraMovement : MonoBehaviour, IPlayerInput
{
    public PlayerMovement movement;
    public float sens;
    public float heightFromTopOfCol = 0.08f;

    private Vector2 _look;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void OnEnable()
    {
        this.Subscribe();
    }

    private void OnDisable()
    {
        this.UnSubscribe();
    }

    public void OnLook(Vector2 lookVector)
    {
        _look = lookVector;
    }
    
    // public void OnLook(Vector2 lookVector)
    // {
    //     _rotationX -= lookVector.y * lookSpeed;
    //     _rotationX = Mathf.Clamp(_rotationX, -90f, 90f);
    //
    //     followTarget.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + lookVector.x * lookSpeed, 0);
    //     transform.rotation = Quaternion.Euler(_rotationX, transform.rotation.eulerAngles.y + lookVector.x * lookSpeed, 0);
    // }

    void Update()
    {
        Look();
        RotateBodyY();
        AlignHead();
    }

    private void Look()
    {
        Vector3 currentRot = transform.rotation.eulerAngles;
        float desiredYRot = currentRot.y + _look.x * sens * Time.deltaTime * 52.55f;
        float desiredXRot = ClampAngle(currentRot.x - _look.y * sens * Time.deltaTime * 52.55f, -89.5f, 89.5f);
        Quaternion quaternion = Quaternion.Euler(desiredXRot, desiredYRot, 0);
        transform.rotation = quaternion;
    }

    private void AlignHead()
    {
        transform.localPosition = (movement.GetHeight() - heightFromTopOfCol) * Vector3.up;
    }

    private void RotateBodyY()
    {
        movement.body.rotation = Quaternion.Euler(movement.body.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, movement.body.rotation.eulerAngles.z);
    }
    
    float ClampAngle(float angle, float from, float to)
    {
        // accepts e.g. -80, 80
        if (angle < 0f) angle = 360 + angle;
        if (angle > 180f) return Mathf.Max(angle, 360 + from);
        return Mathf.Min(angle, to);
    }
}