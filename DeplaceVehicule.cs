using System.Security.Principal;
using System.Threading;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeplaceVehicule : MonoBehaviour
{
    //float pour les vitesses variées
    public float vitesseAvant = 0f;
    public float vitesseVerticale = 0f;
    public float vitesseAvantMax = 10000;
    public float vitesseTourneMax = 4000f;
    public float vitesseMonte = 1f;
    public float rot;
    float vitesseHeliceMax;

    //references aux helices
    public GameObject refHelice;
    public GameObject refHelice2;
    public float vitesseHelice;
    
    //l'explosion qui sera appelée si on touche un mur
    public GameObject objetExplosion;

    //camera qui reste toujours à distance fixe de l'helico
    public GameObject cameraDistanceFixe;

    public Rigidbody Helico;

    //détermine si la partie est terminée ou non
    public bool finJeu = false;

    //sons reliés à l'helicoptere
    AudioSource sonHelico;
    AudioSource sonExplosion;

    //joue quand on touche un bidon
    public AudioClip sonCollecte;

    //bidon d'essence du UI
    public Image barreEssence;
    public float quantiteEssence = 1;

    //scripts des helices
    TourneObjet heliceScript;
    TourneObjet heliceScript2;

    //objets pour les particules
    public GameObject jetEau1;
    public GameObject jetEau2;
    public GameObject brouillard;


    //objet pour le texte quand il n'y a pas assez d'essence
    public GameObject texteEssence;

    //objet et renderer pour le brouillard dans l'eau
    public GameObject Eau;
    Renderer eauRender;

    //couleurs du brouillard dans l'eau
    Color fogSurface = Color.cyan;
    Color fogFond = Color.blue;

    public GameObject Missile;
    Rigidbody rbMissile;
    Quaternion axeHelico;
    Vector3 forceMissile;

    // Start is called before the first frame update
    void Start()
    {
        Helico = GetComponent<Rigidbody>();
        sonHelico = GetComponent<AudioSource>();

        sonExplosion = objetExplosion.GetComponent<AudioSource>();

        //scripts des helices
        heliceScript = refHelice.GetComponent<TourneObjet>();
        heliceScript2 = refHelice2.GetComponent<TourneObjet>();

        //vitesseMax d'une helice
        vitesseHeliceMax = heliceScript.vitRotMax;

        eauRender = Eau.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {

        //si il y a du fog présent
        if (RenderSettings.fog == true)
        {
            // on calcule la distance entre la caméra et l'eau
            var profondeur = (eauRender.bounds.max.y - cameraDistanceFixe.transform.position.y) / 100;

            //la couleur varie graduellement entre cyan vers bleu plus la profondeur augmente
            var fogCouleur = Color.Lerp(fogSurface, fogFond, profondeur);

            //change la couleur du fog
            RenderSettings.fogColor = fogCouleur;

            // la densité du fog augmente plus la distance entre la caméra et l'eau augmente
            RenderSettings.fogDensity = profondeur/40;

            // je ne veux pas qu'il n'y ait pas de densité (ou qu'elle soit négative) ou qu'il y en ait trop
            if (RenderSettings.fogDensity < 0.005f) { RenderSettings.fogDensity = 0.005f; }
            if (RenderSettings.fogDensity > 0.05f) { RenderSettings.fogDensity = 0.05f; }
        }

        // À placer ici pour que la touche se détecte bien
        //Si M est enfoncé, met le volume en pause.
        if (Input.GetKeyDown(KeyCode.M))
        {
            AudioListener.pause = !AudioListener.pause;
        }

        if (finJeu == true)
        {
            //la gravité embarque
            Helico.GetComponent<Rigidbody>().useGravity = true;
            //augmente le drag
            Helico.GetComponent<Rigidbody>().drag = 0.5f;
            Helico.GetComponent<Rigidbody>().angularDrag = 1f;
            //enleve les contraintes sur les axes de l'helico
            Helico.constraints = RigidbodyConstraints.None;

            // la vitesse des deux helices devient 0
            heliceScript.vitRot = 0;
            heliceScript2.vitRot = 0;

            //réduit le volume de l'helico au maximum
            sonHelico.volume = 0;

            //désactive le texte de l'essence qui clignote s'il est présent
            texteEssence.SetActive(false);

            //désactive le brouillard s'il est présent
            brouillard.SetActive(false);

            // Ferme l'audio listener (eviter bugs possibles)
            Camera.main.GetComponent<AudioListener>().enabled = false;
            // Désactive la caméra utilisée en ce moment
            Camera.main.gameObject.SetActive(false);
            // Active la caméra demandée avec les touches 1 à 4
            cameraDistanceFixe.SetActive(true);
            // Active l'AudioListener de la-dite caméra
            cameraDistanceFixe.GetComponent<AudioListener>().enabled = true;

            //la fin du jeu est appelée dans 8 secondes
            Invoke("finDuJeu", 8f);
        } 
        Invoke("finDuJeu", 8f);
        
        //code pour tirer des missiles
        if (Input.GetKeyDown(KeyCode.Space) && vitesseHelice == vitesseHeliceMax)
        {
            axeHelico = transform.rotation;

            //crée un clone du missile
            GameObject MissileClone = Instantiate(Missile);
            MissileClone.SetActive(true);

            //la position du clone = la position de l'helico ajustée
            MissileClone.transform.position = transform.position + new Vector3(0, -3, 0);

            // le missile a la même axe de rotation que l'helico
            MissileClone.transform.rotation = axeHelico;

            //Rigidbody du clone
            rbMissile = MissileClone.GetComponent<Rigidbody>();

            //force qui pousse le missile -> (0,-1,5) par rapport à son axe de rotation
            forceMissile = (transform.forward * 5) + (transform.up * -1);

            //on ajoute la force définie en haut
            rbMissile.AddRelativeForce(forceMissile);

            //apres 5s, si le missile n'a rien touché il MEURT
            Destroy(MissileClone, 10f);
        }

        //chaque frame, la fonction vérifie si on est près de l'eau pour créer du brouillard
        emetBrouillard();

    }


    private void FixedUpdate()
    {


        //update la vitesse de l'helice
        vitesseHelice = refHelice.GetComponent<TourneObjet>().vitRot;

        //

        if (finJeu == false)
        {


            // si l'helice a une vitesse
            if (vitesseHelice > 0)
            {

                //Empêche les bugs de collisions
                transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);

                //on appelle la gestion d'essence pour diminuer le niveau d'essence
                gestionEssence();

                // On part le son si le son n'est pas dejà lancé
                if (!sonHelico.isPlaying)
                {
                    sonHelico.Play();
                }

                // Si le son est lancé
                else
                {
                    // Le volume augmente/descend entre 0 et 1
                    sonHelico.volume = vitesseHelice /vitesseHeliceMax;

                    // Le pitch varie entre 0.5f et 1
                    sonHelico.pitch = 0.5f + (vitesseHelice / (2 * vitesseHeliceMax));
                }
            }

            // Si l'helice n'a pas de vitesse
            else
            {
                //Le son est mis en pause
                sonHelico.Pause();
                //Et on lui enlève son volume, au cas où ca ferait un bug quelque part
                sonHelico.volume = 0;
            }



            if (vitesseHelice == vitesseHeliceMax)
            {
                //On enlève la gravité
                Helico.GetComponent<Rigidbody>().useGravity = false;

                // La vitesse de rotation est augmentée/diminuée
                rot = Input.GetAxis("Horizontal") * vitesseTourneMax;
                //Forces de rotation du véhicule
                Helico.AddRelativeTorque(0, rot, 0);

                //Faire bouger l'Helico vers l'avant par rapport à lui-meme
                // La vitesse avant de l'helico est augmentée/diminuée
                vitesseAvant = Input.GetAxis("Vertical") * vitesseAvantMax;

                //Ascension ou Descente
                if (Input.GetKey(KeyCode.E))
                {
                    // La vitesse de descente est de -1000 - le quart de la vitesse avant
                    vitesseVerticale = -(vitesseAvant / 4) - 1000f;
                }
                else if (Input.GetKey(KeyCode.Q))
                {
                    // La vitesse de descente est de 1000 + le quart de la vitesse avant
                    vitesseVerticale = vitesseAvant / 4 + 1000f;
                }

                // Si on ne pèse sur rien, la vitesse d'ascension est de 0
                else { vitesseVerticale = 0; }

            }

            //Si l'helice ne tourne pas au maximum, on remet la gravité
            else { Helico.GetComponent<Rigidbody>().useGravity = true; }

            //Forces de propulsion
            Helico.AddRelativeForce(0, vitesseVerticale, vitesseAvant);
        }
    }

    // détecte les collisions du personnage (l’objet de ce script) s’il touche l’objet “Mur”, active l’explosion ….

    void OnCollisionEnter(Collision infoCollision) // le type de la variable est Collision
    {
        //si on touche un mur et que la partie n'est pas deja finie
        if (infoCollision.collider.tag == "Mur")
        {
            //active l'explosion de l'helico si la partie n'est pas déja terminée
            if (finJeu == false)
            {
                //active l'explosion de l'helico
                objetExplosion.SetActive(true);
                //si le son ne joue pas
                if (!sonExplosion.isPlaying)
                {
                    //il joue
                    sonExplosion.Play();
                }
            }
            //la partie est finie
            finJeu = true;
        }
        
        //si on touche un drone et que la partie n'est pas deja finie
        if (infoCollision.collider.tag == "drone")
        {
            //active l'explosion de l'helico si la partie n'est pas déja terminée
            if (finJeu == false)
            {
                //active l'explosion de l'helico
                objetExplosion.SetActive(true);
                //si le son ne joue pas
                if (!sonExplosion.isPlaying)
                {
                    //il joue
                    sonExplosion.Play();
                }
            }
            //la partie est finie
            finJeu = true;
        }

    }

    private void OnTriggerEnter(Collider objetTouche)
    {
        //si on touche l'eau
        if (objetTouche.tag == "Eau") {
            //active l'explosion de l'helico si la partie n'est pas déja terminée
            if (finJeu == false)
            {
                objetExplosion.SetActive(true);
                //si le son ne joue pas
                if (!sonExplosion.isPlaying)
                {
                    //il joue
                    sonExplosion.Play();
                }
            }
            //part la fonction qui affiche du brouillard dans 0.65s (pour que la caméra ait le temps d'aller sous l'eau avant d'afficher le fog)
            Invoke("allumeFog", 0.65f);

            //la partie est finie
            finJeu = true;
        }

        //si on touche un bidon
        else if (objetTouche.tag == "bidon")
        {
            //on enleve le bidon
            objetTouche.gameObject.SetActive(false);
            //joue le son de collecte du bidon
            Helico.GetComponent<AudioSource>().PlayOneShot(sonCollecte);

            //on redonne de l'essence
            quantiteEssence += 0.33f;
            //si le niveau d'essence dépasse 1, il devient 1
            if (quantiteEssence > 1) { quantiteEssence = 1; }
        }

        //si on touche le collider qui déclenche les particules d'eau
        else if(objetTouche.tag == "jetEau")
        {
            //active les particules d'eau
            jetEau1.SetActive(true);
            jetEau2.SetActive(true);
        }
    }


    private void OnTriggerStay(Collider objetTouche)
    {
        //si on est dans le collider qui active les jets d'eau
        if (objetTouche.tag == "jetEau")
        {
            //va chercher la propriété emission des 2 systemes de particules
            var particulesEau1 = jetEau1.GetComponent<ParticleSystem>().emission;
            var particulesEau2 = jetEau2.GetComponent<ParticleSystem>().emission;

            //si l'helico n'avance pas
            if (vitesseAvant == 0)
            {
                //pas de particules
                particulesEau1.rateOverTime = 0;
                particulesEau2.rateOverTime = 0;
            }
            else {
                //nombre de particules par emission devient le 1/5 de la vitesse (2000 max)
                particulesEau1.rateOverTime = vitesseAvant*0.2f;
                particulesEau2.rateOverTime = vitesseAvant*0.2f;
            }
        }
    }


    private void OnTriggerExit(Collider objetTouche)
    {
        //si on quitte le collider qui active les jets d'eau
        if (objetTouche.tag == "jetEau")
        {
            //on désactive les jets d'eau
            jetEau1.SetActive(false);
            jetEau2.SetActive(false);
        }
    }

    /*fonction gestionEssence
     * fonction qui baisse la valeur de la quantité d'essence, de 1 à 0
     * si l'essence atteint 0, la partie est finie
     * 
     * @param : none
     * return : none
     * 
     */
    void gestionEssence()
    {
        //réduit la quantité d'essence
        quantiteEssence -= 0.0003f;
        //changement de la hauteur de l’image
        barreEssence.fillAmount = quantiteEssence;

        //si l'essence atteint 0
        if(quantiteEssence <= 0)
        {
            //la partie est perdue
            finJeu = true;
        }

        //si l'essence atteint 30%, le texte d'alerte de l'essence s'active
        if(quantiteEssence < 0.3f) { texteEssence.SetActive(true); }
        else { texteEssence.SetActive(false); }
    }
    
    /*fonction emetBrouillard
     * Projette un rayon qui détecte les tags des éléments, dirigé vers le bas de l'hélico
     * si le tag de l'élément touché est jetEau, le brouillard se déclenche
     * 
     * @param : none
     * return : none
     * 
     */
    void emetBrouillard()
    {
        RaycastHit objetTouche;
        if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out objetTouche, 25))
        {
            //si on touche l'élément qui active les jets d'eau à moins de 20m de l'helico et que les helices tournent assez vite
            if (objetTouche.collider.tag == "jetEau" && objetTouche.distance < 20 && vitesseHelice >25)
            {
                
                brouillard.SetActive(true);
                //met le brouillard à l'endroit où le rayon a touché le collider
                brouillard.transform.position = objetTouche.point;

                //cherche le parametre emission du brouillard
                var particulesBrouillard = brouillard.GetComponent<ParticleSystem>().emission;

                //quand on approche du collider, le rateOverTime augmente jusqu'a 100
                particulesBrouillard.rateOverTime = 100 - (objetTouche.distance * 5);

                //variable pour aller chercher la vitesse des particules
                var brouillardMain = brouillard.GetComponent<ParticleSystem>().main;

                //la vitesse varie de 8/20 à 8
                float startSpeed = 8 / objetTouche.distance;
                // les valeurs peuvent se rendre à infini si distance très petite, si la valeur est trop grande on garde 8
                if (startSpeed > 8) { startSpeed = 8; }

                //les particules vont à la vitesse déterminée
                brouillardMain.startSpeed = startSpeed;
            }

            //si on touche le collider et qu'on est à une distance plus grande que 20m
            else if(objetTouche.collider.tag == "jetEau" && (objetTouche.distance >= 20 || vitesseHelice < 25))
            {
                //cherche le parametre emission des particules
                var particulesBrouillard = brouillard.GetComponent<ParticleSystem>().emission;
                //met l'émission de particules à 0
                particulesBrouillard.rateOverTime = 0;
                //désactive le brouillard
                brouillard.SetActive(false);
            }
            // on peut dessiner la ligne de détection pour débogage
            //Debug.DrawLine( positionDepart , positionFinale , couleur de ligne
            Debug.DrawLine(transform.position, transform.position + new Vector3(0, -25f, 0), Color.red);

        }
    }

    /*fonction allumeFog
     * Quand l'helico touche l'eau, on allume l'effet de brouillard du Renderer 
     * @param : none
     * return : none
     */
    void allumeFog()
    {
        //active le brouillard du renderSettings
        RenderSettings.fog = true;
        RenderSettings.fogDensity = 0.02f;
    }


    void finDuJeu()
    {
        //charge la scene de fin de jeu (mais il n'y en a pas alors on joue en boucle)
        SceneManager.LoadScene(0);
    }
}
