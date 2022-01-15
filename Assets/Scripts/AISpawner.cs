using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    [Tooltip("prefab du spawn")]
    public GameObject prefabAI; // fait le lien entre le script et un transform (le bot)
    [Tooltip("point de spawn des ia")]
    public Transform spawnPoint;

    void Start()
    {

    }


    Transform SpawnAI()
    {
        GameObject ai = GameObject.Instantiate(prefabAI);
        ai.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
        return ai.transform;
    }

    void AddPichenette(Transform ai, Vector3 pichenette) // variable qui est un vecteur pour décider de la direction
    {
        Rigidbody rb = ai.GetComponent<Rigidbody>(); // récupère les composants de l'AIMovers (le rigidbdoy)
        rb.AddForce(pichenette, ForceMode.Impulse); // récupère la fonction de ForceMode -> impulse pour appliquer la force qu'une seule fois 
    }

    private float time = 0;
    [Range(0, 15)]
    public float spawnTimer = 3;

    private Vector3 lastPichenette;

    void Update()
    {
        time = time + Time.deltaTime; // calcule le temps entre chaque frame 
        if (time >= spawnTimer)
        {
            Transform ai = SpawnAI();
            Vector3 pichenette = ai.forward * 5;
            pichenette = Random.insideUnitCircle;
            AddPichenette(ai, pichenette);
            lastPichenette = pichenette;
            time = 0;
        }
    }
}
