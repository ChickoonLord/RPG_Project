using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public float delay = 1;
    private void Awake() {
        Destroy(gameObject,delay);
    }
}
