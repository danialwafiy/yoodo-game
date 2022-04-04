using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    public float clamp;

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, -7 * Time.deltaTime);
        if(transform.position.y < -clamp)
        {
            transform.position = new Vector3(transform.position.x, clamp);
        }
    }
}
