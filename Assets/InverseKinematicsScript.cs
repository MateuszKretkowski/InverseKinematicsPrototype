using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseKinematicsScript : MonoBehaviour
{
    public Transform limb;
    public Transform target;
    public int depthReach;
    public float speed;
    public List<Transform> limbTransforms = new List<Transform>();

    float defaultDistance;

    void Start()
    {
        GetLimbTransforms();
        foreach (Transform currTransform in limbTransforms)
        {
            Debug.Log(currTransform);
        }
        defaultDistance = Vector3.Distance(limbTransforms[0].GetChild(0).position, limbTransforms[limbTransforms.Count - 1].position);
    }

    void Update()
    {
        // if (distance between hand and limb is lower than distance between hand and limb + hand and target
        Debug.Log(defaultDistance < Vector3.Distance(limbTransforms[limbTransforms.Count - 1].position, target.position));

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

        for (int i=limb.root.childCount-1; i>=0; i--)
        {
            limbTransforms.Add(limb.parent.GetChild(i));
        }
    }

    public void SetLimbOuterPosition()
    {
        if (limb == null || target == null)
        {
            Debug.LogError("Limb or target is not assigned.");
            return;
        }


        float angle = Vector3.Angle(limbTransforms[limbTransforms.Count - 1].position, target.position);
        float angleRad = Mathf.Deg2Rad * angle;

        Quaternion lookRotation = Quaternion.LookRotation(target.position - limbTransforms[limbTransforms.Count - 1].position);

        limb.parent.rotation = Quaternion.Slerp(
            limb.parent.rotation,
            lookRotation,
            speed * Time.deltaTime
        );
    }
}
