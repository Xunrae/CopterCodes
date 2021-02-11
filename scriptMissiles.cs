using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptMissiles : MonoBehaviour
{
    public GameObject objetExplosion;
    AudioSource sonExplosion;

    //particules
    public GameObject feuMissiles;

    //force appliquée au missile
    Vector3 forceMissile;

    Rigidbody rbMissile;

    //float acceleration = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        sonExplosion = objetExplosion.GetComponent<AudioSource>();

        // pour la fonction augmenteforce, pas utilisé à cause du bug que j'ai présentement
        //forceMissile = (transform.forward) + (transform.up *-0.5f);

        //experimental, quand j'active cette fonction certains missiles passent au travers du terrain
        //InvokeRepeating("augmenteForce", 0.5f, 1f);

        //pour la fonction augmenteForce encore
        rbMissile = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //forceMissile = (transform.forward * acceleration) + (transform.up * -(acceleration/3));
        //acceleration += 0.1f;
        //rbMissile.AddForce(forceMissile);
    }

    private void OnCollisionEnter(Collision objetTouche)
    { 
        // cherche s'il y a un enfant qui a ce nom dans l'objet du script, s'il existe son renderer est 
        transform.Find("CruiseMissile_LOD0").GetComponent<MeshRenderer>().enabled = false;
        transform.Find("CruiseMissile_LOD1").GetComponent<MeshRenderer>().enabled = false;
        transform.Find("CruiseMissile_LOD2").GetComponent<MeshRenderer>().enabled = false;
        transform.Find("CruiseMissile_LOD3").GetComponent<MeshRenderer>().enabled = false;

        // désactive l'effet de particules
        feuMissiles.SetActive(false);

        //active l'explosion
        objetExplosion.SetActive(true);
        //si le son ne joue pas
        if (!sonExplosion.isPlaying)
        {
            //il joue
            sonExplosion.Play();
        }

        if(objetTouche.collider.tag == "drone")
        {
            //détruit l'ennemi
            Destroy(objetTouche.gameObject);
        }

        //détruit le missile après un petit délai pour qu'on puisse voir son animation de frappe
        Destroy(gameObject, 0.5f);
    }

    //fonction qui augmente la force appliquée au missile
    //void augmenteForce()
    //{
    //avec invokeRepeating, crée une trajectoire parabolique.
    //forceMissile = forceMissile + forceMissile;
    //rbMissile.AddForce(forceMissile);
    //}
}
