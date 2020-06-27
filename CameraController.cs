using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Transform lookAt;
    public Camera theCMBrain;
    public Vector3 offset;

    public float rotateSpeed;

    public Transform pivot;

    public float maxViewAngle;
    public float minViewAngle;

    public GameObject pauseScreen;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        offset = lookAt.position - transform.position;
        pivot.transform.position = lookAt.transform.position;
        pivot.transform.parent = lookAt.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!pauseScreen.activeSelf)
        {
            float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
            float vertical = Input.GetAxis("Mouse Y") * rotateSpeed;

            lookAt.Rotate(0, horizontal, 0);
            pivot.Rotate(-vertical, 0, 0);

            if (pivot.rotation.eulerAngles.x > maxViewAngle && pivot.rotation.eulerAngles.x < 180f)
            {
                pivot.rotation = Quaternion.Euler(maxViewAngle, 0, 0);
            }
            if (pivot.rotation.eulerAngles.x > 180f && pivot.rotation.eulerAngles.x < 360f + minViewAngle)
            {
                pivot.rotation = Quaternion.Euler(360f + minViewAngle, 0, 0);
            }

            float desiredYAngle = lookAt.eulerAngles.y;
            float desiredXAngle = pivot.eulerAngles.x;
            Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0);
            transform.position = lookAt.position - (rotation * offset);;

            if (transform.position.y < lookAt.position.y + 0.5f)
            {
                transform.position = new Vector3(transform.position.x, lookAt.position.y + 0.5f, transform.position.z);
            }

            transform.LookAt(lookAt);

            float zoomInput = Input.GetAxis("Mouse ScrollWheel");
            if (zoomInput > 0f || Input.GetButtonDown("Camera1"))
            {
                if (offset.y > -2)
                {
                    //do nothing
                }
                else
                {
                    offset.z -= 1.5f;
                    offset.y += 1.5f;
                }
            }
            else if (zoomInput < 0f || Input.GetButtonDown("Camera2"))
            {
                if (offset.y < -14f)
                {
                    //do nothing
                }
                else
                {
                    offset.z += 1.5f;
                    offset.y -= 1.5f;
                }
            }
        }
    }
}
