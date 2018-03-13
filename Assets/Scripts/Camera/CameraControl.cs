using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private float _mDampTime = 0.8f; // Approximate time for the camera to refocus.

    [SerializeField]
    private float _mScreenEdgeBuffer = 4f; // Space between the top/bottom most target and the screen edge.

    [SerializeField] private float _mMinSize = 6.0f; // The smallest orthographic size the camera can be.

    /*[HideInInspector]*/
    public Transform[] MTargets; // All the targets the camera needs to encompass.


    private Camera _mCamera; // Used for referencing the camera.
    private float _mZoomSpeed; // Reference speed for the smooth damping of the orthographic size.
    private Vector3 _mMoveVelocity; // Reference velocity for the smooth damping of the position.
    private Vector3 _mDesiredPosition; // The position the camera is moving towards.


    private void Awake()
    {
        _mCamera = GetComponentInChildren<Camera>();
    }


    private void FixedUpdate()
    {
        // Move the camera towards a desired position.
        Move();

        // Change the size of the camera based.
        Zoom();
    }


    private void Move()
    {
        // Find the average position of the targets.
        FindAveragePosition();

        // Smoothly transition to that position.
        transform.position = Vector3.SmoothDamp(transform.position, _mDesiredPosition, ref _mMoveVelocity, _mDampTime);
    }


    private void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();
        int numTargets = 0;

        // Go through all the targets and add their positions together.
        for (int i = 0; i < MTargets.Length; i++)
        {
            // If the target isn't active, go on to the next one.
            if (!MTargets[i].gameObject.activeSelf)
                continue;

            // Add to the average and increment the number of targets in the average.
            averagePos += MTargets[i].position;
            numTargets++;
        }

        // If there are targets divide the sum of the positions by the number of them to find the average.
        if (numTargets > 0)
            averagePos /= numTargets;

        // Keep the same y value.
        //averagePos.y = transform.position.y;

        // The desired position is the average position;
        _mDesiredPosition = averagePos;
    }


    private void Zoom()
    {
        // Find the required size based on the desired position and smoothly transition to that size.
        float requiredSize = FindRequiredSize();
        _mCamera.orthographicSize =
            Mathf.SmoothDamp(_mCamera.orthographicSize, requiredSize, ref _mZoomSpeed, _mDampTime);
    }


    private float FindRequiredSize()
    {
        // Find the position the camera rig is moving towards in its local space.
        Vector3 desiredLocalPos = transform.InverseTransformPoint(_mDesiredPosition);

        // Start the camera's size calculation at zero.
        float size = 0f;

        // Go through all the targets...
        for (int i = 0; i < MTargets.Length; i++)
        {
            // ... and if they aren't active continue on to the next target.
            if (!MTargets[i].gameObject.activeSelf)
                continue;

            // Otherwise, find the position of the target in the camera's local space.
            Vector3 targetLocalPos = transform.InverseTransformPoint(MTargets[i].position);

            // Find the position of the target from the desired position of the camera's local space.
            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

            // Choose the largest out of the current size and the distance of the tank 'up' or 'down' from the camera.
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));

            // Choose the largest out of the current size and the calculated size based on the tank being to the left or right of the camera.
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / _mCamera.aspect);
        }

        // Add the edge buffer to the size.
        size += _mScreenEdgeBuffer;

        // Make sure the camera's size isn't below the minimum.
        size = Mathf.Max(size, _mMinSize);

        return size;
    }


    public void SetStartPositionAndSize()
    {
        // Find the desired position.
        FindAveragePosition();

        // Set the camera's position to the desired position without damping.
        transform.position = _mDesiredPosition;

        // Find and set the required size of the camera.
        _mCamera.orthographicSize = FindRequiredSize();
    }
}