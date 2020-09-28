using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform playerTransform = null;
    [SerializeField] private Vector2 minPosition = new Vector2(-10,2);
    [SerializeField] private Vector2 maxPosition = new Vector2(10,10);
    public float followSpeed = 10;
    public Vector2 offset = new Vector2(0,0);
    public float lookOffset = 4;
    private Vector2 targetPos;
    void FixedUpdate()
    {
        if (playerTransform){
            float xPos = playerTransform.position.x + offset.x;
            float yPos = playerTransform.position.y + (Input.GetAxis("CameraVertOffset")*lookOffset) + offset.y;
            targetPos = new Vector2(Mathf.Clamp(xPos,minPosition.x,maxPosition.x),
                Mathf.Clamp(yPos,minPosition.y,maxPosition.y));
        }
    }
    private void LateUpdate() {
        transform.position = new Vector3(Mathf.Lerp(transform.position.x,targetPos.x,followSpeed*Time.deltaTime),
            Mathf.Lerp(transform.position.y,targetPos.y,followSpeed*Time.deltaTime),transform.position.z);
    }
}
