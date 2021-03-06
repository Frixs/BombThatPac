﻿using UnityEngine;

namespace Cameras
{
    /// <summary>
    /// This class is able to manipulate with the main camera.
    /// </summary>
    public class GameCameraControl : MonoBehaviour
    {
        [SerializeField] private float _dampTime = 0.8f; // Approximate time for the camera to refocus.

        [SerializeField]
        private float _screenEdgeBuffer = 4f; // Space between the top/bottom most target and the screen edge.

        [SerializeField] private float _minSize = 6.0f; // The smallest orthographic size the camera can be.

        [HideInInspector] public Transform[] Targets; // All the targets the camera needs to encompass.

        private UnityEngine.Camera _camera; // Used for referencing the camera.
        private Vector3 _cameraDefaultPosition; // Used for centering map.
        private float _zoomSpeed; // Reference speed for the smooth damping of the orthographic size.
        private Vector3 _moveVelocity; // Reference velocity for the smooth damping of the position.
        private Vector3 _mDesiredPosition; // The position the camera is moving towards.

        private void Awake()
        {
            _camera = GetComponentInChildren<UnityEngine.Camera>();
            _cameraDefaultPosition = _camera.transform.position;
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
            transform.position =
                Vector3.SmoothDamp(transform.position, _mDesiredPosition, ref _moveVelocity, _dampTime);
        }

        private void FindAveragePosition()
        {
            Vector3 averagePos = new Vector3();
            int numTargets = 0;
            
            // Go through all the targets and add their positions together.
            for (int i = 0; i < Targets.Length; i++)
            {
                // If the target isn't active, go on to the next one.
                if (!Targets[i].gameObject.activeSelf)
                    continue;

                // Add to the average and increment the number of targets in the average.
                averagePos += Targets[i].position;
                numTargets++;
            }

            // If there are targets divide the sum of the positions by the number of them to find the average.
            if (numTargets > 0)
                averagePos /= numTargets;

            // Keep the same y value.
            //averagePos.y = transform.position.y;

            // The desired position is the average position;
            if (numTargets > 0)
                _mDesiredPosition = averagePos;
            else
                _mDesiredPosition = _cameraDefaultPosition;
        }

        private void Zoom()
        {
            // Find the required size based on the desired position and smoothly transition to that size.
            float requiredSize = FindRequiredSize();
            _camera.orthographicSize =
                Mathf.SmoothDamp(_camera.orthographicSize, requiredSize, ref _zoomSpeed, _dampTime);
        }

        private float FindRequiredSize()
        {
            // Find the position the camera rig is moving towards in its local space.
            Vector3 desiredLocalPos = transform.InverseTransformPoint(_mDesiredPosition);

            // Start the camera's size calculation at zero.
            float size = 0f;

            // Go through all the targets...
            for (int i = 0; i < Targets.Length; i++)
            {
                // ... and if they aren't active continue on to the next target.
                if (!Targets[i].gameObject.activeSelf)
                    continue;

                // Otherwise, find the position of the target in the camera's local space.
                Vector3 targetLocalPos = transform.InverseTransformPoint(Targets[i].position);

                // Find the position of the target from the desired position of the camera's local space.
                Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

                // Choose the largest out of the current size and the distance of the tank 'up' or 'down' from the camera.
                size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));

                // Choose the largest out of the current size and the calculated size based on the tank being to the left or right of the camera.
                size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / _camera.aspect);
            }

            // Add the edge buffer to the size.
            size += _screenEdgeBuffer;

            // Make sure the camera's size isn't below the minimum.
            size = Mathf.Max(size, _minSize);

            return size;
        }

        public void SetStartPositionAndSize()
        {
            // Find the desired position.
            FindAveragePosition();

            // Set the camera's position to the desired position without damping.
            transform.position = _mDesiredPosition;

            // Find and set the required size of the camera.
            _camera.orthographicSize = FindRequiredSize();
        }
    }
}