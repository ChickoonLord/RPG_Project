using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGizmoCircle : MonoBehaviour
{
    [SerializeField] private Vector2 center = Vector2.zero;
    [SerializeField] private float range = 1;
    private void Awake() {
        Destroy(this);
    }
    private void OnDrawGizmos() {
        UnityEditor.Handles.DrawWireDisc(center,Vector3.back,range);
    }
}
