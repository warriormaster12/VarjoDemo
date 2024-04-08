using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.HID;
using UnityEngine.XR;
using Varjo.XR;
using static Varjo.XR.VarjoEyeTracking;

public class EyeTracking : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private VarjoEyeTracking.GazeCalibrationMode calibrationMode = VarjoEyeTracking.GazeCalibrationMode.Fast;
    [SerializeField] private VarjoEyeTracking.GazeOutputFrequency frequency = VarjoEyeTracking.GazeOutputFrequency.MaximumSupported;
    [SerializeField] private VarjoEyeTracking.GazeOutputFilterType filterType = VarjoEyeTracking.GazeOutputFilterType.Standard;
    [SerializeField] private float gazeRadius = 0.01f;
    [SerializeField] private float maxDistance = 100.0f;
    [SerializeField] private LayerMask mask = 1 << 0;

    [Header("Events")]
    public UnityEvent<GameObject> onGazeEnter;
    public UnityEvent<GameObject> onGazeExit;

    private Camera xrCamera;
    private VarjoEyeTracking.GazeData gazeData;
    private GameObject currentTarget = null;

    private void Awake()
    {
        xrCamera = GetComponent<Camera>();
        if (xrCamera == null ) {
            Debug.LogWarning("EyeTracking.cs should be attached to the camera");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (!VarjoEyeTracking.IsGazeAllowed())
        {
            Debug.LogWarning("Gazing is not allowed. Make sure eye tracking is enabled in Varjo Base software");
            return;
        }
        if (!VarjoEyeTracking.IsGazeCalibrated())
        {
            if (VarjoEyeTracking.RequestGazeCalibration(calibrationMode))
            {
                Debug.Log("Successfully requested gaze calibration");
            }
            else
            {
                Debug.LogWarning("Failed to request gaze calibration");
            }
        } else
        {
            VarjoEyeTracking.SetGazeOutputFrequency(frequency);
            VarjoEyeTracking.SetGazeOutputFilterType(filterType);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!VarjoEyeTracking.IsGazeCalibrated() && !VarjoEyeTracking.IsGazeCalibrating())
        {
            Debug.Log("Trying to recalibrate gaze");
            if (VarjoEyeTracking.RequestGazeCalibration(calibrationMode))
            {
                Debug.Log("Successfully requested gaze calibration");
            } else
            {
                Debug.Log("Failed to request gaze calibration");
            }
            return;
        } else if (!VarjoEyeTracking.IsGazeCalibrated() && VarjoEyeTracking.IsGazeCalibrating())
        {
            return;
        }
        gazeData = VarjoEyeTracking.GetGaze();

        if (gazeData.status != VarjoEyeTracking.GazeStatus.Invalid)
        {
            // Set gaze origin as raycast origin
            Vector3 rayOrigin = xrCamera.transform.TransformPoint(gazeData.gaze.origin);

            // Set gaze direction as raycast direction
            Vector3 direction = xrCamera.transform.TransformDirection(gazeData.gaze.forward);

            RaycastHit hit;

            if (Physics.SphereCast(rayOrigin, gazeRadius, direction,out hit, maxDistance,mask))
            {
                currentTarget = hit.collider.gameObject;
                onGazeEnter.Invoke(currentTarget);
            } 
            else
            {
                onGazeExit.Invoke(currentTarget);
                currentTarget = null;
            }
        }
    }
}
