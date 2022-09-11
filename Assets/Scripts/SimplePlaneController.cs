using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.UI.Image;

public class SimplePlaneController : MonoBehaviour
{
    [Range(0, 1)]
    public float thrust;
    [Range(1, 2000)]
    public float mass;
    public float speed;
    public GameObject bullet;
    public TextMeshProUGUI avionicsData;

    const float timeToUpdateAvionics = 0.5f;
    float timeElapsedToUpdate;

    bool disabled;

    float actualThrust;
    float altitude;
    float prevAltitude;

    Vector3 angles;

    Ray ray;
    RaycastHit hitData;

    // Start is called before the first frame update
    void Start()
    {
        disabled = false;
        speed = 50;
        ray = new Ray(transform.position, transform.forward);
    }


    // Update is called once per frame
    void Update()
    {

        // reset
        if (Input.GetKeyDown(KeyCode.Space))
        {
            disabled = false;
            transform.SetPositionAndRotation(new(0, 500, 0), Quaternion.Euler(0, 0, 0));
        }

        // fire gun
        if (Input.GetKey(KeyCode.X))
        {
            GameObject bulletInstance = Instantiate(bullet, transform.position, Quaternion.Euler(transform.forward));
            //bulletInstance.transform.parent = transform;
        }

        // avionics
        ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red);
        if (Physics.Raycast(ray, out hitData, 1000))
        {
            Debug.Log("RAY HIT");
        }
        altitude = transform.position.y;
        if (timeElapsedToUpdate > timeToUpdateAvionics)
        {
            float verticalSpeed = (altitude - prevAltitude) / timeElapsedToUpdate;
            prevAltitude = altitude;

            avionicsData.text = "altitude: " + ((int)altitude).ToString();
            avionicsData.text += "\nV-Speed: " + ((int)verticalSpeed).ToString();
            avionicsData.text += "\nAngles: " + angles.ToString();

            timeElapsedToUpdate = 0;
        }
        else
        {
            timeElapsedToUpdate += Time.deltaTime;
        }

        // camera
        float camBias = 0.99f;
        Vector3 moveCamTo = transform.position - transform.forward * 10.0f + Vector3.up * 5.0f;
        Camera.main.transform.position = Camera.main.transform.position * camBias + moveCamTo * (1.0f - camBias);
        Camera.main.transform.LookAt(transform.position + transform.forward * 20.0f);

    }

    private void FixedUpdate()
    {

        // gravity affects speed
        speed = Mathf.Clamp(speed - (transform.forward.y * Time.deltaTime) * 20.0f, thrust * 20.0f, 100);

        float thrustBias = 0.99f;
        actualThrust = Mathf.Clamp(actualThrust * thrustBias + thrust * (1 - thrustBias), 0.05f, 1.0f);


        // if controls online
        if (!disabled)
        {
            Vector3 input = new Vector3(Input.GetAxis("Vertical"), Input.GetAxis("Alt"), -Input.GetAxis("Horizontal")) / (mass / 1000);
            angles += input;
            transform.Rotate(input.x, input.y, input.z, Space.Self);

            float forwardMovement = actualThrust * speed + actualThrust;
            transform.Translate(0, 0, forwardMovement, Space.Self);

            float gravity = Mathf.Clamp(1.0f / actualThrust, 0, 5);
            transform.Translate(0, -gravity, 0, Space.World);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        disabled = true;
        Debug.Log("disabled...");
    }

}
