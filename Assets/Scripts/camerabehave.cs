using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerabehave : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player; // La balle que la caméra va suivre
    public Vector3 offset; // Décalage entre la balle et la caméra
    public float rotationSpeed = 5f; // Vitesse de rotation
    public Rigidbody rb;
    private bool isRotating = false;
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
       if (!isRotating)
        {
            transform.position = player.transform.position + offset;
        }

        rb = player.GetComponent<Rigidbody>();
        if (Input.GetMouseButtonDown(1) && rb.velocity.magnitude == 0)
        {
            isRotating = true;
        }

        
        if (Input.GetMouseButtonUp(1))
        {
            isRotating = false;
            
            offset = transform.position - player.transform.position;
        }

       
        if (isRotating)
        {
            
            float horizontalInput = Input.GetAxis("Mouse X") * rotationSpeed;
            
            
            transform.RotateAround(player.transform.position, Vector3.up, horizontalInput);
            

            offset = transform.position - player.transform.position;
        }
    }
}
