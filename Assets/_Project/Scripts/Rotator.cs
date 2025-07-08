
using UnityEngine;

[ExecuteInEditMode]
public class Rotator : MonoBehaviour
{
    public Vector3 axis = Vector3.up;
    public float speed = 1f;

    void Update()
    {
        transform.Rotate(axis * Time.deltaTime * speed, Space.Self);
    }
}