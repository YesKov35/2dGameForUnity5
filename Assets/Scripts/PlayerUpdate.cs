using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour
{
    public Camera cam;
    // Use this for initialization
    void Start()
    {
        cam = transform.GetComponentInChildren<Camera>().GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {

        }
        else
        {
            if (cam.enabled)
            {
                cam.enabled = false;
                cam.gameObject.GetComponent<AudioListener>().enabled = false;
            }
        }
    }
}