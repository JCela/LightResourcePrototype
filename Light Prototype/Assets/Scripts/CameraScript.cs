using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject target;
    [Range(0,10)]
    public float speed;
    [Range(0,10)]
    public float viewDistance = 1;

    public GameObject visionOverlay;
    public GameObject detectionOverlay;

    void Start () 
    {
        visionOverlay.SetActive(true);
        detectionOverlay.SetActive(true);
    }

    void FixedUpdate () {
        Vector3 playerPos = target.transform.position;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = target.transform.position.z;

        Vector3 center = (playerPos + mousePos) / 2;

        Vector3 targetPos;
        if (Vector3.Distance (playerPos, center) > viewDistance) {
            targetPos = playerPos + ((center - playerPos).normalized * viewDistance);
        } else {
            targetPos = center;
        }

        Vector3 cameraPosNoHeight = transform.position;
        cameraPosNoHeight.z = 0;

        Vector3 newCameraPosNoHeight = Vector3.Lerp(cameraPosNoHeight, targetPos, speed * Time.deltaTime);

        transform.position = new Vector3 (newCameraPosNoHeight.x, newCameraPosNoHeight.y, -10);
    }
}
