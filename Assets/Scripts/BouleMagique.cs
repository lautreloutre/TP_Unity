using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouleMagique : MonoBehaviour
{
    public float puissance = 100;
    public void OnCollisionEnter(Collision collision)
    {
        // si on touche un component de type AIMover = vrai -> vérifier si on touche bien le bot 
        AIMover other = collision.gameObject.GetComponent<AIMover>();
        if (other != null)
        {
            other.life -= puissance;
            Destroy(gameObject); // détruit le projectil si on touche un 

            if (other.life <= 0)
            {
                Destroy(other.gameObject);
                ScoreScript.kills++;
            }

        }

    }
}
