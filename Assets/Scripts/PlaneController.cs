using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    [Range(0, 10000)]
    public float thrust;
    [Range(0, 100)]
    public float liftCoefficient;
    [Range(0, 100)]
    public float dragCoefficient;

    public TextMeshProUGUI forcesText;
    public TextMeshProUGUI velocitiesText;

    Rigidbody rigidbody;
    bool disabled;

    float lift;
    float drag;
    //float thrust;
    Vector3 localVelocity;

    // Start is called before the first frame update
    void Start()
    {
        disabled = false;
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = new(0, 1, 50);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            disabled = false;
            rigidbody.transform.SetPositionAndRotation(new(0, 500, 0), Quaternion.Euler(0, 90, 0));
        }

        forcesText.text = "lift: " + ((int)lift).ToString() + "\nthrust: " + ((int)thrust).ToString() + "\ndrag: " + ((int)drag).ToString();
        velocitiesText.text = localVelocity.ToString();

    }

    private void FixedUpdate()
    {

        localVelocity = transform.worldToLocalMatrix.MultiplyVector(rigidbody.velocity);

        if (!disabled)
        {
            /*
             * Get Forces
             */
            lift = Mathf.Clamp(liftCoefficient * localVelocity.x * localVelocity.x, 0, 15000);
            drag = dragCoefficient * localVelocity.x * localVelocity.x;

            rigidbody.AddRelativeForce(drag - thrust, lift, 0);
            rigidbody.AddForce(0, -9.81f * rigidbody.mass, 0);

            /*
             * User input
             */
            Vector3 input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), Input.GetAxis("Alt")) / (rigidbody.mass / 1000);
            rigidbody.transform.Rotate(input.x, input.z, input.y);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        disabled = true;

    }
}
