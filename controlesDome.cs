using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlesDome : MonoBehaviour
{
    //animator du dome
    Animator anim;
    //son du dome
    AudioSource sonSourceDome;
    
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        sonSourceDome = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.O))
        {
            //le booléen de l'animator devient true -> le dome s'ouvre
            anim.SetBool("estOuvert", true);

        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            //le booléen de l'animator devient false -> le dome se ferme
            anim.SetBool("estOuvert", false);
        }
    }

    /*fonction jouerSon
     *Joue le son du dome
     *
     *@param : none
     * return : none
     */

    void jouerSon()
    {
        sonSourceDome.Play();
    }
}
