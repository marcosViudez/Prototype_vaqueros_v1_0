using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour {

    private Transform lookAt;
    private bool smooth;
    private float smoothSpeed;
    private Vector3 offSet;

    private void Start()
    {
        lookAt = GameObject.FindGameObjectWithTag("Player").transform;
        smooth = true;
        smoothSpeed = 2f;
        offSet = new Vector3(0, 4.5f, lookAt.transform.position.z - this.transform.position.z - 5f); // Constante eje z
    }

    private void LateUpdate()
    {
        Vector3 desirePosition = lookAt.transform.position + offSet;
        if (smooth) {
            transform.position = Vector3.Lerp(transform.position, desirePosition, smoothSpeed * Time.deltaTime);
        }
        else {
            transform.position = desirePosition;
        }

    }

}
