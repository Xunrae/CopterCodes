using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI; //ajoute de bibliothèque UI



public class compteurScript : MonoBehaviour
{
    Text txtCompteur;

    GameObject Helico;
    
    //scripts qui communiquent avec ce script
    TourneObjet scriptHelice;
    DeplaceVehicule scriptVehicule;

    //booléens pour savoir si le moteur est allumé et si le temps est écoulé
    bool moteurAllume;
    bool tempsEcoule;

    // valeur du temps commence à 120
    public int valCompteur = 120;
    
    void Start()
    {
        txtCompteur = GetComponent<Text>();

        //à chaque seconde appelle Compteur
        InvokeRepeating("Compteur", 1, 1);

        //va chercher l'helice
        GameObject helice = GameObject.FindGameObjectWithTag("helice");
        scriptHelice = helice.GetComponent<TourneObjet>();

        //va chercher le vehicule
        GameObject vehicule = GameObject.FindGameObjectWithTag("Vehicule");
        scriptVehicule = vehicule.GetComponent<DeplaceVehicule>();
    }
    void Compteur()
    {
        //diminue compteur
        valCompteur -= 1;
        //convertit la valeur en texte et l'affiche
        GetComponent<Text>().text = valCompteur.ToString();

        if(valCompteur <= 0) {
            valCompteur = 0;
            // Lorsque le compteur est rendu à 0, le moteur doit s'arrêter et l'hélico commencer à tomber.
           scriptHelice.moteurAllume = false;
           scriptHelice.tempsEcoule = true;
           scriptVehicule.finJeu = true;
            
           // arrete le compteur
           CancelInvoke("Compteur");
        }

        //update le texte affiché
        txtCompteur.text = "Temps : " + valCompteur;
    }
    
}
