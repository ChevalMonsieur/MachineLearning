using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // components and gameobjects
    [SerializeField] GameObject agentToFollow;

    void Update()
    {
        transform.position = agentToFollow.transform.position + Vector3.up * 4 + Vector3.forward * 6;
    }
}
