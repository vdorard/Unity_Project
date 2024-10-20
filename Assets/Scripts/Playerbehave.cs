using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Playerbehave : MonoBehaviour
{ 
    private bool isIdle; 
    private bool endupdate = false;
    public TextMeshProUGUI endText;
    public TextMeshProUGUI scoreText;
    public int score = 0;
    private bool isAiming;
    private bool isMoving = false;
    private Vector3 lastIdlePosition;
    public LineRenderer lineRenderer; 
    private float shotPower = 300f;
    private Terrain terrain;
    public LayerMask ballLayer; // Pour sélectionner uniquement la balle
    private bool musicplayed = false;
    
    private Rigidbody playerRigidbody;
    private AudioManager audioManager;
    public float maxShootDistance = 1200f;
    private Plane aimPlane;  // Plan horizontal pour le tir
    public ParticleSystem particleSystem;

    private void Awake(){
        playerRigidbody = GetComponent<Rigidbody>();
        isAiming = false;
        lastIdlePosition = transform.position;
        if (particleSystem != null)
        {
            particleSystem.Stop(); // S'assurer que les particules sont arrêtées au départ
        }
        lineRenderer.enabled = false;    
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();   
        UpdateScoreText();
        
        // Créer un plan horizontal à la hauteur de la balle
        aimPlane = new Plane(Vector3.up, transform.position);  
    }

  private void OnMouseDown(){
        if(isIdle && !Pausemenu.GamePaused){
            isAiming = true;
            audioManager.PlaySFX(audioManager.click); 

        }
    }






    private void processAim(){
        
        if(!isAiming || !isIdle || Pausemenu.GamePaused){
            return;
        }
        lastIdlePosition = transform.position;
        Vector3? worldPoint = GetMouseWorldPoint();
        if (!worldPoint.HasValue) {
            return; 
        }

        DrawLine(worldPoint.Value);  

        if(Input.GetMouseButtonUp(0)){
            Shoot(worldPoint.Value);
            
            musicplayed = false;
        }
        if(Input.GetMouseButtonUp(1)){
            CancelAim();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Vérifier si l'objet touché est le terrain
        if (collision.gameObject.CompareTag("Terrain"))
        {
            Debug.Log("La balle a touché le terrain. Retour à la dernière position immobile.");

            // Réinitialiser la position de la balle à la dernière position immobile
            transform.position = lastIdlePosition;

            // Stopper la balle immédiatement
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
        }
    }
    private void UpdateScoreText(){
        scoreText.text = "Score: " + score;  // Mettre à jour le texte affiché
    }

    private void CancelAim(){
        isAiming = false;
        lineRenderer.enabled = false;
    }

    private void Shoot(Vector3 worldPoint) {
    isAiming = false;
    lineRenderer.enabled = false;

    // Position de la balle
    Vector3 ballPosition = transform.position;

    // Calculer la direction opposée au vecteur entre la balle et le point de la souris
    Vector3 direction = (ballPosition - worldPoint).normalized;

    // Calculer la distance sur l'axe X
    float distanceToMouse = Vector3.Distance(ballPosition,worldPoint);
    Debug.Log("Distance à la souris (axe X): " + distanceToMouse);

    // Définir la force en fonction de la distance
    float strength = Mathf.Clamp(distanceToMouse * shotPower, 0, 1200f);
    Debug.Log("Force appliquée: " + strength);

    // Appliquer la force dans la direction calculée
    playerRigidbody.AddForce(direction * strength);
    audioManager.PlaySFX(audioManager.shoot);
    isIdle = false;

    // Incrémenter le score
    score++;
    GameManager.Instance.score++;   
    UpdateScoreText();
}





    void Update()
{
    if (Pausemenu.GamePaused) {
        return; 
    }

    // Vérifier si le joueur clique au milieu de l'écran
    CheckForCenterClick();

    if (playerRigidbody.velocity.magnitude < 0.5f) {
        isMoving = false;
        aimPlane = new Plane(Vector3.up, transform.position); 
        
        processAim();
        Stop();
    }
    if (playerRigidbody.velocity.magnitude > 2) {
        if (particleSystem != null && !particleSystem.isPlaying) {
            particleSystem.Play();
        }
        Vector3 particlePosition = transform.position;
        particlePosition.y = transform.position.y; 
        particleSystem.transform.position = particlePosition;
        particleSystem.transform.LookAt(Camera.main.transform.position);
    } else {
        if (particleSystem != null && particleSystem.isPlaying) {
            particleSystem.Stop();
        }
    }
}

private void CheckForCenterClick()
{
    // Si le joueur clique avec le bouton gauche de la souris
    if (Input.GetMouseButtonDown(0)) {
        // Position du clic de la souris
        Vector3 mousePos = Input.mousePosition;
        
        // Centre de l'écran
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        // Distance entre le clic et le centre de l'écran
        float distanceFromCenter = Vector3.Distance(mousePos, screenCenter);

        // Si le clic est suffisamment proche du centre (tu peux ajuster cette tolérance)
        float tolerance = 200f; // Par exemple, 50 pixels de tolérance
        Debug.Log(distanceFromCenter);
        if (distanceFromCenter <= tolerance) {
            isAiming = true;
            Debug.Log("Clic détecté au centre de l'écran, isIdle est maintenant à true.");
        }
    }
}

    private void Stop(){
        playerRigidbody.velocity = Vector3.zero;
        playerRigidbody.angularVelocity = Vector3.zero; 
        
        isIdle = true; 
    }
    void Start(){
        audioManager.PlaySFX(audioManager.stop);
        musicplayed = true;
    }
    public void playSound(){
        if (!musicplayed && playerRigidbody.velocity.magnitude == 0){
            audioManager.PlaySFX(audioManager.stop);
            musicplayed = true;
        }
    }
    private void DrawLine(Vector3 worldPoint){
        Vector3 offset = new Vector3(0f, 0.5f, 0f);  // Ajuste l'offset si nécessaire
        Vector3 startPosition = transform.position;

        // Projeter worldPoint sur le plan horizontal
        Vector3 horizontalWorldPoint = new Vector3(worldPoint.x, transform.position.y, worldPoint.z);

        // Mettre à jour le LineRenderer avec les nouvelles positions
        Vector3[] positions = { startPosition - offset, horizontalWorldPoint - offset};
        lineRenderer.SetPositions(positions);
        lineRenderer.enabled = true;
    }

    // Méthode pour obtenir le point dans le monde où le curseur clique sur le plan horizontal
    private Vector3? GetMouseWorldPoint() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float enter = 0.0f;
        if (aimPlane.Raycast(ray, out enter)) {
            return ray.GetPoint(enter);  // Retourner le point sur le plan horizontal
        }

        return null;  // Si pas d'intersection avec le plan
    }
    
    public void UpdateEndText(){
        if (endupdate == false) {
            endText.text = endText.text + " " +  GameManager.Instance.score + " Coups !";  
           
        }
        endupdate = true;
    }


}
