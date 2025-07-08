
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera2 : MonoBehaviour
{
    public Transform target;
    public Transform parent;
    public Material imposterMaterial;

    public enum Axis { up, down, left, right, forward, back };
    public bool reverseFace = false;
    public Axis axis = Axis.up;

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
            Vector3 axs = GetAxis(axis);
            Vector3 targetPos = transform.position + target.transform.rotation * (reverseFace ? Vector3.forward : Vector3.back);
            Vector3 targetOrientation = target.transform.rotation * GetAxis(axis);
            transform.LookAt(targetPos, targetOrientation);

            Vector3 pRot = Quaternion.Inverse(parent.transform.rotation).eulerAngles;
            Vector3 rot = transform.localRotation.eulerAngles;

            rot.x = axs.x == 0f ? 0 : pRot.x;
            rot.y = axs.y == 0f ? 0 : pRot.y;
            rot.z = axs.z == 0f ? 0 : pRot.z;

            transform.localRotation = Quaternion.Euler(rot);

            Debug.DrawRay(transform.position, transform.position - target.transform.position, Color.red);

            int imageIndex = GetClosestCircularIndex();
            Debug.Log($"GetClosestCircularIndex: {imageIndex}");

            if (imageIndex >= 0 && imageIndex < circularImages.Count)
                imposterMaterial.mainTexture = circularImages[imageIndex];
        }
    }

    //float Clamp0to360(float eulerAngles)
    //{
    //    float result = eulerAngles - Mathf.CeilToInt(eulerAngles / 360f) * 360f;
    //    if (result < 0)
    //        result += 360f;

    //    //Debug.Log($"result: {result}");

    //    return result;
    //}

    int GetClosestCircularIndex()
    {
        float angularDistance = float.MaxValue;
        //float angle = Clamp0to360(transform.localRotation.y);
        float angle = transform.localEulerAngles.z;
        if (angle >= 270)
            angle -= 360;

        Debug.Log($"angle: {angle}");

        int index = 0;
        for (int i = 0; i < circularAngles.Count; ++i)
        {
            //float dist = Clamp0to360(circularAngles[i] - angle);

            float dist = Mathf.Abs(circularAngles[i] - angle);

            //Debug.Log($"dist: {dist}");

            if (dist < angularDistance)
            {
                angularDistance = dist;
                index = i;
            }
        }

        return index;
    }
}
