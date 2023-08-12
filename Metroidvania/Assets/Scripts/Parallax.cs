using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    public Camera cam;
    public Transform subject; // subject is the player in this situation


    Vector2 startPosition;
    float startZ;
    

    //property
    //distance camera has moved from original position.
    Vector2 travel => (Vector2)cam.transform.position - startPosition;

    //the distance that the background or forground is from the player.
    float distanceFromSubject => transform.position.z - subject.position.z;

    // the cam position + either the far or near clipping plane. since objects that are on the near clipping plane will move the oposite direction.
    // if distance from subject is greater than 0 then use cam.farclippingplane else use nearclippingplane
    float clippingPlane => (cam.transform.position.z + (distanceFromSubject > 0? cam.farClipPlane : cam.nearClipPlane));

    // the paralax factor is the distance from the player divided by the clipping plane.
    float parallaxFactor => Mathf.Abs(distanceFromSubject) / clippingPlane;

    
    public void Start()
    {
        startPosition = transform.position;
        startZ = transform.position.z;
        subject = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // needs to be fixed update to not have the background stutter.
    public void FixedUpdate()
    {
        Vector2 newPos = startPosition + travel * parallaxFactor;
        transform.position = new Vector3(newPos.x, newPos.y, startZ);
    }

}
