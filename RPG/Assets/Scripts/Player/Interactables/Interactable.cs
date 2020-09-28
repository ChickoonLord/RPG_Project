using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 2f;
    public bool playerInRadius = false;
    public virtual void Interact(){

    }
    protected virtual void Update() {
        if (!PlayerAI.instance) return;
        if (Vector2.Distance(transform.position,PlayerAI.instance.gameObject.transform.position) <= radius){
            playerInRadius = true;
        } else {
            playerInRadius = false;
        }
    }
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,radius);
    }
}
