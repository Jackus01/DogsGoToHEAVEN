using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Dog : MonoBehaviour
{

    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
        Vector3 movement = Vector3.zero;


        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.forward * Time.deltaTime * 10;
            transform.eulerAngles = new Vector3(0, 0, 0);

        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.back * Time.deltaTime * 10;
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * Time.deltaTime * 10;
            transform.eulerAngles = new Vector3(0, -90, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * Time.deltaTime * 10;
            transform.eulerAngles = new Vector3(0, 90, 0);
        }

        if (rb != null)
        {
            rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);
        }


    }


    
}
