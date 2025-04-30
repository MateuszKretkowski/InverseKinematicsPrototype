using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseKinematicsScript : MonoBehaviour
{
    public Transform limb;
    public Transform target;
    public int depthReach;
    public float speed;
    List<Transform> limbTransforms = new List<Transform>();

    float defaultDistance;

    void Start()
    {
        GetLimbTransforms();
        foreach (Transform currTransform in limbTransforms)
        {
            Debug.Log(currTransform);
        }
        defaultDistance = Vector3.Distance(limbTransforms[0].position, limbTransforms[limbTransforms.Count - 1].position);


    }

    void Update()
    {
        // if (distance between hand and limb is lower than distance between hand and limb + hand and target
        Debug.Log(defaultDistance < Vector3.Distance(limbTransforms[limbTransforms.Count - 1].position, target.position));

        Debug.Log(limbTransforms[limbTransforms.Count - 1].gameObject.name);
        Debug.Log(limbTransforms[0].gameObject.name);
        if (defaultDistance < Vector3.Distance(limbTransforms[limbTransforms.Count - 1].position, target.position))
        {
            SetLimbOuterPosition();
        }
    }

    public void GetLimbTransforms()
    {
        // first index equals to f.e: hand
        // last index equals to f.e: arm

        limbTransforms.Add(limb);
        Transform currentLimb = limb;

        int i = 0;
        while (currentLimb != limb.root || i == depthReach)
        {
            limbTransforms.Add(currentLimb);
            currentLimb = currentLimb.parent;
            i++;
        }
    }

    public void SetLimbOuterPosition()
    {
        if (limb == null || target == null)
        {
            Debug.LogError("Limb or target is not assigned.");
            return;
        }

        Debug.Log("Setting Limb Outer Position");

        float angle = Vector3.Angle(limbTransforms[limbTransforms.Count - 1].position, target.position);
        float angleRad = Mathf.Deg2Rad * angle;

        Quaternion lookRotation = Quaternion.LookRotation(target.position - limbTransforms[limbTransforms.Count - 1].position);

        limbTransforms[limbTransforms.Count - 1].rotation = Quaternion.Slerp(
            limbTransforms[limbTransforms.Count - 1].rotation,
            lookRotation,
            speed * Time.deltaTime
        );
    }
}
