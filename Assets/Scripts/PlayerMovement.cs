using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float forwardBackwardMovement;
    public float sideMovement;
    public float maxSpeed = 2f;

    public float mouseSensitivity;
    Rigidbody _rb;
    [SerializeField] GameObject cam;

    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out _rb);
    }

    // Update is called once per frame
    void Update()
    {
        bool movingMax = _rb.velocity.magnitude > maxSpeed;

        if (Input.GetKeyDown(KeyCode.LeftShift))
		{
            maxSpeed *= 2;
		}
        if (Input.GetKeyUp(KeyCode.LeftShift))
		{
            maxSpeed /= 2;
		}

        if (Input.GetKey(KeyCode.W))
		{
            if (!movingMax)
                _rb.AddForce(transform.forward.normalized * forwardBackwardMovement * Time.deltaTime);
		}
        if (Input.GetKey(KeyCode.S))
		{
            if (!movingMax)
                _rb.AddForce(transform.forward.normalized * -1 * forwardBackwardMovement * Time.deltaTime);
		}
        if (Input.GetKey(KeyCode.A))
        {
            if (!movingMax)
                _rb.AddForce(transform.right.normalized * -1 * sideMovement * Time.deltaTime);
		}
        if (Input.GetKey(KeyCode.D))
        {
            if (!movingMax)
                _rb.AddForce(transform.right.normalized * sideMovement * Time.deltaTime);
		}

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + (mouseSensitivity * Input.GetAxis("Mouse X")), transform.rotation.eulerAngles.z);
        cam.transform.localRotation = Quaternion.Euler(cam.transform.localRotation.eulerAngles.x + (mouseSensitivity * -Input.GetAxis("Mouse Y")), cam.transform.localRotation.eulerAngles.y, cam.transform.localRotation.eulerAngles.z);
    }
}
