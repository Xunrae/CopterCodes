using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLerp : MonoBehaviour
{

    public GameObject Helico;
    public GameObject Self;
    public Vector3 positionFinale;

    //public GameObject[] Cameras;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        // La position finale que devra avoir la caméra
        positionFinale = Helico.transform.TransformPoint(0, 5, -10);
        // Crée la transition entre la position actuelle et la position finale de la caméra
        transform.position = Vector3.Lerp(transform.position, positionFinale, 0.2f);
        // Regarde l'Helico
        transform.LookAt(Helico.transform);
    }
}
