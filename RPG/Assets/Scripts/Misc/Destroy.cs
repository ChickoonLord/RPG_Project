using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    [SerializeField] private float delay = 1;
    private void Start() {
        Destroy(gameObject,delay);
    }
}
