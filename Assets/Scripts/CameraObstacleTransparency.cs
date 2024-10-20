using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraObstacleTransparency : MonoBehaviour
{
    public GameObject player; // La balle que la caméra suit
    public Material transparentMaterial; // Le matériau transparent à appliquer
    public LayerMask obstacleLayer; // Le Layer qui contient les obstacles

    private Rigidbody playerRigidbody;
    private Dictionary<Renderer, Material> originalMaterials = new Dictionary<Renderer, Material>(); // Sauvegarder les matériaux d'origine

    void Start()
    {
        playerRigidbody = player.GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Calculer la distance entre la caméra et la balle
        float distanceCameraToPlayer = Vector3.Distance(Camera.main.transform.position, player.transform.position);

        // Effectuer un Raycast entre la caméra et la balle
        Vector3 directionToPlayer = player.transform.position - Camera.main.transform.position;
        Ray ray = new Ray(Camera.main.transform.position, directionToPlayer);
        RaycastHit[] hits = Physics.RaycastAll(ray, directionToPlayer.magnitude, obstacleLayer);

        // Créer une liste temporaire pour suivre les obstacles actuels
        List<Renderer> currentObstacles = new List<Renderer>();

        // Pour chaque obstacle rencontré, vérifier s'il est plus proche que la balle
        foreach (RaycastHit hit in hits)
        {
            Renderer rend = hit.collider.GetComponent<Renderer>();
            if (rend != null)
            {
                currentObstacles.Add(rend);

                // Sauvegarder le matériau d'origine si ce n'est pas déjà fait
                if (!originalMaterials.ContainsKey(rend))
                {
                    originalMaterials[rend] = rend.material;
                }

                // Remplacer le matériau de l'objet par le matériau transparent
                SetObjectTransparent(rend);
            }
        }

        // Réinitialiser les obstacles non bloqués (ceux qui ne sont pas touchés par le raycast)
        foreach (var rend in new List<Renderer>(originalMaterials.Keys))
        {
            if (!currentObstacles.Contains(rend))
            {
                SetObjectOpaque(rend);
                originalMaterials.Remove(rend); // Retirer l'obstacle restauré de la liste
            }
        }
    }

    // Fonction pour remplacer le matériau d'un objet par le matériau transparent
    private void SetObjectTransparent(Renderer rend)
    {
        if (rend != null && rend.material != transparentMaterial)
        {
            rend.material = transparentMaterial; // Remplacer par le matériau transparent fourni
        }
    }

    // Fonction pour remettre l'objet en opaque (restaurer le matériau d'origine)
    private void SetObjectOpaque(Renderer rend)
    {
        if (rend != null && originalMaterials.ContainsKey(rend))
        {
            rend.material = originalMaterials[rend]; // Restaurer le matériau d'origine
        }
    }
}
