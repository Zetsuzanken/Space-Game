using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolling : MonoBehaviour
{
    public float ScrollSpeed;

    void Update()
    {
        transform.position += new Vector3(0, ScrollSpeed * Time.deltaTime, 0);
    }
}
