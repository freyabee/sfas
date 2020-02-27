using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    private Transform pointingAtBoard;
    public Transform player;
    bool following = true;
    Vector3 offset;
    public float cameraSmooth;
    // Start is called before the first frame update
    void Start()
    {
        pointingAtBoard = transform;
        offset = transform.localPosition - player.localPosition;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        if (following)
        {
            Vector3 targetCameraPosition = player.position;
            transform.localPosition = Vector3.Lerp(transform.position, targetCameraPosition, cameraSmooth * Time.deltaTime);
        }
        else if(transform.localPosition != pointingAtBoard.localPosition)
        {
            transform.localPosition = Vector3.Lerp(transform.position, pointingAtBoard.localPosition, cameraSmooth * Time.deltaTime);
        }
    }
    public void StartFollowingPlayer()
    {
        offset = transform.localPosition - player.localPosition;
        following = true;
    }
    public void StopFollowingPlayer()
    {
        following = false;
    }

}
