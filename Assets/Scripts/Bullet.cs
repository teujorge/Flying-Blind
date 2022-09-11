using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    Vector3 initPosition;

    const int maxDistance = 50000;
    const float speed = 100;

    // Start is called before the first frame update
    void Start()
    {
        initPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, 0, speed, Space.Self);


        Debug.Log((transform.position - initPosition).magnitude);
        if ((transform.position - initPosition).magnitude > maxDistance)
        {
            Destroy(gameObject);
        }

    }
}
