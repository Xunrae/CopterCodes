using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TourneObjet : MonoBehaviour
{
    //vecteur de vitesse de rotation
    public Vector3 vitesseRotation = new Vector3(20,45,10);

    //floats de vitesses
    public float vitRot = 0f;
    public float vitRotMax = 50f;
    public float vitRotMin = 0f;

    //booléens nécéssaires au script
    public bool moteurAllume = false;
    public bool tempsEcoule = false;
    public bool pasDessence = false;

    // Update is called once per frame
    void Update()
    {
        // si le temps n'est pas écoulé on peut allumer le moteur
        if (tempsEcoule == false)
        {

            if (moteurAllume == true)
            {
                //si l'objet auquel est collé le script est l'hélice arriere
                if (gameObject.name == "Helice_arriere")
                {
                    //rotation sur l'axe x
                    transform.Rotate(vitRot, 0, 0);
                }
                //Helice du haut, rotation sur l'axe y
                else { transform.Rotate(0, vitRot, 0); }

                //augmente vitesse de rotation
                vitRot += 0.2f;

                //plafond de la vitesse à 50f
                if (vitRot >= vitRotMax) { vitRot = vitRotMax; }
            }

            if (moteurAllume == false)
            {
                //réduit la vitesse de rotation
                vitRot -= 0.2f;
                // quand la vitesse de rotation va sous 0, rotation = 0
                if (vitRot < vitRotMin) { vitRot = vitRotMin; }

                //si l'objet auquel est collé le script est l'hélice arriere
                if (gameObject.name == "Helice_arriere")
                {
                    //rotation sur l'axe x
                    transform.Rotate(vitRot, 0, 0);
                }

                //Helice du haut, rotation sur l'axe y
                else { transform.Rotate(0, vitRot, 0); }
            }

            //si on appuie sur enter
            if (Input.GetKeyDown(KeyCode.Return))
            {
                //on change l'état du bouléen
                moteurAllume = !moteurAllume;
            }
        }

        // si le temps est écoulé, le moteur n'est pas allumé
        else {
            moteurAllume = false;

            //il n'y a pas de vitesse de rotation
            vitRot = 0;
            transform.Rotate(vitRot, 0, 0);
        }

    }
}
