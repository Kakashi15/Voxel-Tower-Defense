using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dirMod = 10f;
    void Update()
    {
        // Get input from the keyboard
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float rotationPtichInput = Input.GetAxis("Yaw");
        float rotationYawInput = Input.GetAxis("Pitch");

        // Calculate the movement vector
        Vector3 movementForward = new Vector3(0f, 0f, verticalInput) * moveSpeed * Time.deltaTime;
        Vector3 movementSide = new Vector3(horizontalInput, 0f, 0f) * moveSpeed * Time.deltaTime;
        Vector3 Rotation = new Vector3(rotationYawInput, rotationPtichInput, 0f) * moveSpeed * Time.deltaTime;
        // Apply the movement to the camera's position
        Vector3 dir = (transform.up + transform.forward).normalized;
        Debug.DrawLine(transform.position, dir * dirMod, Color.black);
        transform.eulerAngles += Rotation;

        Vector3 stickDirection = new Vector3(horizontalInput, 0, verticalInput);            //Get Movement Input Vector
        Vector3 combinedVector = Camera.main.transform.forward + Camera.main.transform.up;    //Stores a new Vector which is an intermediate between Z and Y axis of the camera
        Vector3 camForward = combinedVector / combinedVector.magnitude;

        transform.position += camForward * verticalInput * moveSpeed * Time.deltaTime;
        transform.position += transform.right * horizontalInput * moveSpeed * Time.deltaTime;
    }
}
