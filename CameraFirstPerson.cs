using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFirstPerson : MonoBehaviour
{

    public GameObject refHelice;
    public GameObject Helico;

    //position initiale de l'helico
    Vector3 positionInitiale;

    // Update is called once per frame
    void Update()
    {
        //on va chercher la vitesse de rotation de l'helice dans le script d'une des helices
        var vitHelice = refHelice.GetComponent<TourneObjet>().vitRot;

        //position que la caméra devrait avoir relativement à l'Helico
        positionInitiale = Helico.transform.TransformPoint(0.7f, 0.16f, 2.0f);

        //On change la position de la caméra pour la position ci-haut
        transform.position = positionInitiale;

        //si l'helice a une vitesse
        if (vitHelice > 0)
        {
            // crée un nombre aléatoire entre -0.1f et 0.1f
            var rdm = Random.Range(-0.1f, 0.1f);

            // déplace sur la caméra sur l'axe x, avec la valeur rdm
            transform.Translate(rdm, 0, 0);
            
        }
    }
}
