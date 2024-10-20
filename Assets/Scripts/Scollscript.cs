using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scollscript : MonoBehaviour
{
    public float ScrollX = -0.5f;
    public float ScrollY = -0.5f;
    
    public float speedMultiplier = 15f; // Facteur de multiplication de la vitesse

    private void OnCollisionEnter(Collision collision)
    {
        // Vérifiez si l'objet qui entre en collision est la balle (par son tag par exemple)
        if (collision.gameObject.CompareTag("Player"))
        {
            // Obtenez le Rigidbody de la balle
            Rigidbody ballRigidbody = collision.gameObject.GetComponent<Rigidbody>();

            if (ballRigidbody != null)
            {
                // Multipliez la vitesse actuelle par le facteur de multiplication
                ballRigidbody.velocity *= speedMultiplier;

                Debug.Log("Vitesse doublée au contact de la surface !");
            }
        }
    }
    void Update(){
        float UpdateXpos = Time.time * ScrollX; 
        float UpdateYpos = Time.time * ScrollY;
        this.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(UpdateXpos, UpdateYpos);
    }
}

