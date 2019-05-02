using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 offset;
    private GameObject player;
    private GameObject rotationCenter;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        transform.position = player.transform.position + offset;
        rotationCenter = GameObject.FindWithTag("RotationCenter");
        rotationCenter.transform.position = transform.position;
    }

    void LateUpdate()
    {
        //transform.LookAt(player.transform);
        transform.position = player.transform.position + offset;
        //transform.eulerAngles = new Vector3(0, rotationCenter.transform.eulerAngles.y, 0);
        transform.position = rotationCenter.transform.position;
        transform.LookAt(player.transform);
    }
}
