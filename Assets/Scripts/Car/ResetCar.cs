using UnityEngine;

public class ResetCar : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        // When backspace is pressed set the cars position and rotation.
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Transform t = GetComponent<Transform>();
            t.SetPositionAndRotation(t.position + new Vector3(0, 0.2f, 0), new Quaternion(0, -1, 0, 1));
        }
    }
}
