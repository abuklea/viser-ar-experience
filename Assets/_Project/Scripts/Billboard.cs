
using UnityEngine;

namespace _Project {

    public class Billboard : MonoBehaviour
    {
        public Transform Target;
        
        public bool ReverseFace = false;

        public enum Axis {
            Up,
            Down,
            Left,
            Right,
            Forward,
            Back
        }
        
        public Axis axis = Axis.Up;

        public Vector3 GetAxis(Axis refAxis) {
            switch (refAxis) {
                case Axis.Down:
                    return Vector3.down;
                case Axis.Forward:
                    return Vector3.forward;
                case Axis.Back:
                    return Vector3.back;
                case Axis.Left:
                    return Vector3.left;
                case Axis.Right:
                    return Vector3.right;
                default:
                    return Vector3.up;
            }
        }

        public Vector3 AxisLock = Vector3.up;
        
        public float Offset;

        // Start is called before the first frame update
        void Start() {
            if (Application.isPlaying && !Target) Target = Camera.main?.transform;
        }

        // Update is called once per frame
        void LateUpdate() {
            if (Target != null) UpdateBillboard(Target);
        }
        
        public void UpdateBillboard(Transform target)
        {
            var targetPos = transform.position + target.transform.rotation * (ReverseFace ? Vector3.forward : Vector3.back);
            targetPos.x = Mathf.Lerp(targetPos.x, transform.position.x, AxisLock.x);
            targetPos.y = Mathf.Lerp(targetPos.y, transform.position.y, AxisLock.y);
            targetPos.z = Mathf.Lerp(targetPos.z, transform.position.z, AxisLock.z);
            transform.LookAt(targetPos, GetAxis(axis));
            var vec = transform.position - target.transform.position;
            vec.Normalize();
            transform.parent.position += (vec * Offset);
        }
    }
}