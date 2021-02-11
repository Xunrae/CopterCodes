using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraObservation : MonoBehaviour
{

    public GameObject Helico;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Cette caméra regarde tout le temps l'helico.
        transform.LookAt(Helico.transform);
    }
}
