using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gestionCams : MonoBehaviour
{
    public GameObject[] Cameras;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //si on appuie sur les touches 1 à 4, changer la caméra
        if (Input.GetKeyDown(KeyCode.Alpha1)) { changerCamera(0); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { changerCamera(1); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { changerCamera(2); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { changerCamera(3); }
    }

    void changerCamera(int num)
    {
        // Ferme l'audio listener (eviter bugs possibles)
        Camera.main.GetComponent<AudioListener>().enabled = false;
        // Désactive la caméra utilisée en ce moment
        Camera.main.gameObject.SetActive(false);
        // Active la caméra demandée avec les touches 1 à 4
        Cameras[num].SetActive(true);
        // Active l'AudioListener de la-dite caméra
        Cameras[num].GetComponent<AudioListener>().enabled = true;
    }
}
