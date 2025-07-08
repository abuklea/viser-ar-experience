
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Transform target;
    public Transform toRotate;
    public Material imposterMaterial;

    public enum Axis { up, down, left, right, forward, back };
    public bool reverseFace = false;
    public Axis axis = Axis.up;
    public Vector3 axisVals;
    public Vector3 viewDir;
    public Vector3 rotationOffset;
    public Vector3 AxisLock = Vector3.up;

    public List<float> circularAngles;
    public List<Texture> circularImages;

    // return a direction based upon chosen axis
    public Vector3 GetAxis(Axis refAxis)
    {
        switch (refAxis)
        {
            case Axis.down:
                return Vector3.down;
            case Axis.forward:
                return Vector3.forward;
            case Axis.back:
                return Vector3.back;
            case Axis.left:
                return Vector3.left;
            case Axis.right:
                return Vector3.right;
        }

        // default is Vector3.up
        return Vector3.up;
    }

    void Update()
    {
        if (target)
        {
            axisVals = GetAxis(axis);

            Vector3 pos = transform.position;
            Vector3 tar = target.position;

            //Debug.DrawLine(pos, tar, Color.green);

            viewDir = pos - tar;
            viewDir.x = (axisVals.x == 1f ? 0 : viewDir.x);
            viewDir.y = (axisVals.y == 1f ? 0 : viewDir.y);
            viewDir.z = (axisVals.z == 1f ? 0 : viewDir.z);

            //Debug.DrawRay(tar, viewDir * 50f, Color.red);

            if(viewDir != Vector3.zero)
                toRotate.rotation = Quaternion.LookRotation(viewDir);

            Vector3 rot = toRotate.localEulerAngles;
            rot.x = axisVals.x == 0f ? 0 : rot.x;
            rot.y = axisVals.y == 0f ? 0 : rot.y;
            rot.z = axisVals.z == 0f ? 0 : rot.z;

            toRotate.localEulerAngles = rot;

            int imageIndex = GetClosestCircularIndex();
            //Debug.Log($"GetClosestCircularIndex: {imageIndex}");

            if (imageIndex >= 0 && imageIndex < circularImages.Count)
                imposterMaterial.mainTexture = circularImages[imageIndex];
        }
    }

    int GetClosestCircularIndex()
    {        
        float angularDistance = float.MaxValue;
        float angle = toRotate.localEulerAngles.y;
        if (angle >= 270)
            angle -= 360;

        //Debug.Log($"angle: {angle}");

        int index = 0;
        for(int i=0; i<circularAngles.Count; ++i)
        {
            float dist = Mathf.Abs(circularAngles[i] - angle);

            if (dist < angularDistance)
            {
                angularDistance = dist;
                index = i;
            }
        }

        return index;
    }
}
