using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParrallaxEffect : MonoBehaviour
{
    //liên kết đến camera
    public Camera cam;
    // liên kết đến mục tiêu gốc
    public Transform followTarget;
    //vị trí bắt đầu của vật thể được áp dụng hiệu ứng parallax
    Vector2 startingPos;
    float startingZ;
    Vector2 canMoveSinceStart => (Vector2) cam.transform.position - startingPos;
    float zDistanceFromTarget => transform.position.z - followTarget.transform.position.z;
    float clippingPlane => (cam.transform.position.z + (zDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));
    float parrallaxFactor => Mathf.Abs(zDistanceFromTarget) / clippingPlane;
    // Start is called before the first frame update
    void Start()
    {
        // lấy vị trí ban đầu của
        startingPos = transform.position;
        startingZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPos = startingPos + canMoveSinceStart * parrallaxFactor;
        transform.position = new Vector3(newPos.x, newPos.y, startingZ);
    }
}
