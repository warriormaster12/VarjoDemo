using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShowOutline : MonoBehaviour
{

    private Material material = null;
    private void Awake()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer)
        {
            material = meshRenderer.materials[meshRenderer.materials.Length - 1];
        }

        EyeTracking eyetracking = Camera.main.GetComponent<EyeTracking>();

        if (eyetracking)
        {
            eyetracking.onGazeEnter.AddListener(onGazeEnter);
            eyetracking.onGazeExit.AddListener(onGazeExit);
        }
    }

    private void onGazeEnter(GameObject body)
    {
        if (body == this.gameObject)
        {
            material.SetFloat("_Power", 2.0f);
            material.SetFloat("_Scale", 1.03f);
        }
    }

    private void onGazeExit(GameObject body)
    {
        if (body == this.gameObject)
        {
            material.SetFloat("_Power", 0.0f);
            material.SetFloat("_Scale", 0.0f);
        }
    }
}
