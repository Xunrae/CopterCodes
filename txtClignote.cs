using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class txtClignote : MonoBehaviour
{
    //booléen pour déterminer etat actif ou non
    public bool estInactif = false;

    //le texte lui-meme
    public Text textEssence;
    
    
    // Start is called before the first frame update
    void Start()
    {
        //chaque seconde, la fonction clignote est appelée
        InvokeRepeating("clignote", 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        //si le texte est supposé etre inactif
        if(estInactif == true)
        {
            //son texte est inactif
            textEssence.enabled= false;
        }
        //sinon il est actif
        else { textEssence.enabled = true; }
    }

    void clignote()
    {
        //booléen s'inverse
        estInactif = !estInactif;
    }
}
