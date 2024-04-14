using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenCamera : MonoBehaviour
{
    [SerializeField] int sensitivity;
    [SerializeField] int lockVertMin, lockVertMax;
    [SerializeField] bool invertY;
    float rotX;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //input
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;
        //invert?
        if (invertY)
        {
            rotX += mouseY;
        }
        else
        {
            rotX -= mouseY;
        }
        //clamp
        rotX = Mathf.Clamp(rotX, lockVertMin, lockVertMax);
        //rotate cam on x-axis
        transform.localRotation = Quaternion.Euler(rotX, 0, 0);
        //rotate player on y-axis
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
