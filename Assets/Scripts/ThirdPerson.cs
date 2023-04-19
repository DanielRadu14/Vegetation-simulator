using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPerson : MonoBehaviour
{
    float yaw, pitch;
    private Player player;
    public Transform target;
    public Vector3 cameraOffset;
    public Vector3 aimingOffset;
    public float followSpeed = 3f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        pitch -= Input.GetAxis("Mouse Y");
        yaw += Input.GetAxis("Mouse X");

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 newCameraPosition = target.position + transform.TransformDirection(cameraOffset);
        transform.position = Vector3.Lerp(transform.position, 
                                        newCameraPosition, 
                                        Mathf.Clamp01(Time.deltaTime * followSpeed));
        
        if(player.aiming)
        {
            transform.position = player.transform.position + transform.TransformDirection(aimingOffset);
        }
    }
}
