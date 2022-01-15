using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Transform objectToThrow;
    public Transform playerCam;
    public bool isGrounded;
    public static bool defeat;
    private float time = 2;
    [Range(0, 2)]
    public float fireRate = 2;


    void Start()
    {
        defeat = false;
        if (playerCam == null)
        {
            Camera cam = transform.GetComponentInChildren<Camera>();
            playerCam = cam.transform;
        }
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        //Sauve la rotation
        Quaternion lastRotation = playerCam.rotation;

        //Baisse / leve la tete
        float rot = Input.GetAxis("Mouse Y") * -1;
        Quaternion q = Quaternion.AngleAxis(rot, playerCam.right);
        playerCam.rotation = q * playerCam.rotation;

        //Est ce qu'on a la tete a  l'envers ?
        Vector3 forwardCam = playerCam.forward;
        Vector3 forwardPlayer = transform.forward;
        float regardeDevant = Vector3.Dot(forwardCam, forwardPlayer);
        if (regardeDevant < 0.0f)
            playerCam.rotation = lastRotation;

        //Tourner gauche droite
        rot = Input.GetAxis("Mouse X") * 1;
        q = Quaternion.AngleAxis(rot, transform.up);
        transform.rotation = q * transform.rotation;


        time = time + Time.deltaTime;
        if ((Input.GetButtonDown("Fire1")) && (time >= fireRate))
        {
            Transform obj = GameObject.Instantiate<Transform>(objectToThrow); 
            obj.position = playerCam.position + playerCam.forward; // projectile part de la caméra + 1m (forward)
            obj.GetComponent<Rigidbody>().AddForce(playerCam.forward * 70, ForceMode.Impulse); // récupère le rigidbody et ajoute une ipulsion
            time = 0;
        }

    }

    void FixedUpdate()
    {
        Rigidbody rb;
        rb = GetComponent<Rigidbody>();

        float vert = Input.GetAxis("Vertical");
        float hori = Input.GetAxis("Horizontal");

        Vector3 horizontalVelocity = Vector3.zero;
        horizontalVelocity += vert * transform.forward * 10;
        horizontalVelocity += hori * transform.right * 10;
        rb.velocity = new Vector3(horizontalVelocity.x,
            rb.velocity.y,
            horizontalVelocity.z);

        //Est ce qu'on touche le sol ?
        isGrounded = false;
        RaycastHit infos;
        bool trouve = Physics.SphereCast(transform.position + transform.up * 0.1f,
            0.05f, -transform.up, out infos, 2);
        if (trouve && infos.distance < 0.15)
            isGrounded = true;

        if (Input.GetButton("Jump"))
        {
            if (isGrounded)
            {
                rb.AddForce(transform.up * 3, ForceMode.Impulse);
                isGrounded = false;
            }
            else
            {
                if (rb.velocity.y < 3)
                {
                    rb.AddForce(transform.up * 50);
                }
            }
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        // si on touche un component de type AIMover = vrai -> vérifier si on touche bien le bot 
        AIMover attack = collision.gameObject.GetComponent<AIMover>();
        if (attack != null)
        {
            defeat = true;      
        }

    }
}
