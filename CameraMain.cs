using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMain : MonoBehaviour
{
    public GameObject Helico;
    Vector3 positionFinale;
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
        // position relative de l'objet Helico
        positionFinale = Helico.transform.TransformPoint(0, 5, -10);

        // la caméra va se placer à la position definie ci-haut
        transform.position = positionFinale;

        // la caméra regarde l'Helico
        transform.LookAt(Helico.transform);

    }
}
