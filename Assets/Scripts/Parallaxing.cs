using UnityEngine;
using System.Collections;

public class Parallaxing : MonoBehaviour {

	public Transform[] backgrounds;	
	private float[] parallaxScales;		
	public float smoothing = 1f;	

	private Transform cam;			
	private Vector3 previousCamPos;	

    public Camera player;
    public string clanName;

	void Start ()
    {
        cam = player.transform;
        if(clanName == "red")
        {
            previousCamPos = new Vector3(cam.position.x - 11, cam.position.y, cam.position.z);
        }
        else
        {
            previousCamPos = cam.position;
        }

        parallaxScales = new float[backgrounds.Length];
		for (int i = 0; i < backgrounds.Length; i++)
        {
			parallaxScales[i] = backgrounds[i].position.z*-1;
		}
	}
	
	void Update ()
    {
		for (int i = 0; i < backgrounds.Length; i++)
        {
			float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];
			float backgroundTargetPosX = backgrounds[i].position.x + parallax;
			Vector3 backgroundTargetPos = new Vector3 (backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

			backgrounds[i].position = Vector3.Lerp (backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
		}

		previousCamPos = cam.position;
	}
}
