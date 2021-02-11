using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class butScript : MonoBehaviour
{
    //effets de particules du but
    public GameObject effet1;
    public GameObject effet2;

    //l'helico
    public GameObject helico;
    
    //texte de victoire
    public GameObject texteVictoire;

    //script du vehicule
    DeplaceVehicule scriptHelico;

    // Start is called before the first frame update
    void Start()
    {
        scriptHelico = helico.GetComponent<DeplaceVehicule>();
    }


    private void OnCollisionEnter(Collision autreObjet)
    {
        if (autreObjet.collider.tag == "Vehicule")
        {
            //Active les effets de particules du but
            effet1.SetActive(true);
            effet2.SetActive(true);
            //Active le texte de victoire
            texteVictoire.SetActive(true);

            //La fin du jeu est enclenchée
            scriptHelico.finJeu = true;
        }
    }
    
}
