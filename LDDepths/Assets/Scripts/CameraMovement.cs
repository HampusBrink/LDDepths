using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour, IPlayerInput
{
    [SerializeField] private Transform followTarget;
    [SerializeField] private float lookSpeed = 2f;
    [SerializeField] private float cameraSpeed = 5.0f;
    [SerializeField] private Vector3 offset;

    private float _rotationX;

    private void OnEnable()
    {
        this.Subscribe();
    }

    private void OnDisable()
    {
        this.UnSubscribe();
    }

    private void Update()
    {
        transform.position = followTarget.position + offset;
    }

    public void OnLook(Vector2 lookVector)
    {
        _rotationX -= lookVector.y * lookSpeed;
        _rotationX = Mathf.Clamp(_rotationX, -90f, 90f);
        
        followTarget.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + lookVector.x * lookSpeed, 0);
        transform.rotation = Quaternion.Euler(_rotationX, transform.rotation.eulerAngles.y + lookVector.x * lookSpeed, 0);
    }
}