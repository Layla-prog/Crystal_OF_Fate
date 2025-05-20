using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2, -4);
    public float rotationSpeed = 5f;
    public float minY = -30f;
    public float maxY = 60f;

    private float currentX = 0f;
    private float currentY = 20f;

    public float sensitivityX = 1f;
    public float sensitivityY = 1f;
    public float smoothing = 1f;

    private Vector2 smoothedInput;
    private Vector2 mouseInput;


    void LateUpdate()
{
    if (target == null) return;

    // Raw input
    float rawMouseX = Input.GetAxis("Mouse X");
    float rawMouseY = Input.GetAxis("Mouse Y");

    // Apply smoothing
    mouseInput.x = Mathf.Lerp(mouseInput.x, rawMouseX * sensitivityX, 1f / smoothing);
    mouseInput.y = Mathf.Lerp(mouseInput.y, rawMouseY * sensitivityY, 1f / smoothing);

    currentX += mouseInput.x;
    currentY -= mouseInput.y;
    currentY = Mathf.Clamp(currentY, minY, maxY);

    Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
    Vector3 desiredPosition = target.position + rotation * offset;

    transform.position = desiredPosition;
    transform.LookAt(target.position + Vector3.up * 1.5f);
}


    // Start is called before the first frame update
    void Start()
{
    sensitivityX = PlayerPrefs.GetFloat("XSensitivity", 1f);
    sensitivityY = PlayerPrefs.GetFloat("YSensitivity", 1f);
    smoothing = PlayerPrefs.GetFloat("MouseSmoothing", 1f);
}


    // Update is called once per frame
    void Update()
    {
        
    }
}
