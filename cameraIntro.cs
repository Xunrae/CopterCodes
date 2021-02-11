using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraIntro : MonoBehaviour
{
    public GameObject but;

    // Update is called once per frame
    void Update()
    {
        //regarde le but
        transform.LookAt(but.transform);

        //va a droite par rapport à sa position actuelle, ce qui fait tourner la caméra autour du but
        transform.RotateAround(but.transform.position, Vector3.up, 50* Time.deltaTime);
    }
    
}
