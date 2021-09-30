using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;

    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float freeLookRotationSpeed = 200f;

    private Vector3 pivotOffset = new Vector3(0, 2, 0);

    public bool isInFreeLook = false;
    private bool isHome = false;

    void Start()
    {
        transform.position = targetTransform.position + pivotOffset;
    }

    private void Update()
    {
        FreeLook();
        RotateHome();
    }

    void FixedUpdate()
    {
        transform.position = targetTransform.position + pivotOffset;
    }

    void FreeLook()
    {
        // When RMB is held rotate the camera pivot object with mouse X axis delta.
        if (Input.GetMouseButton(1))
        {
            isInFreeLook = true;
            transform.RotateAround(targetTransform.position, targetTransform.up, Input.GetAxis("Mouse X") * freeLookRotationSpeed * Time.deltaTime);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isInFreeLook = false;
        }
    }

    void RotateHome()
    {
        float angle = Quaternion.Angle(transform.localRotation, Quaternion.Euler(Vector3.zero));

        // Check if the camera is back to its original position.
        if (angle <= 0.01f)
        {
            isHome = true;
        }
        else
        {
            isHome = false;
        }

        // If the camera is not back in its original position Lerp to it.
        if (!isHome && !isInFreeLook)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(Vector3.zero), rotationSpeed * Time.deltaTime);
        }
    }
}
