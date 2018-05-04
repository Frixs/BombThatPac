using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace UI.Gameplay
{
    public class NotificationPanel : MonoBehaviour
    {
        /// <summary>
        /// Text label of notfication. 
        /// </summary>
        [SerializeField] private TMP_Text _notificationText;

        /// <summary>
        /// Check if notification panel is free.
        /// </summary>
        private bool _isNotificationPanelFree;

        /// <summary>
        /// Home position of hidden notif.
        /// </summary>
        private float _homePositionY = 20f;
        
        /// <summary>
        /// Notification position to be visible to player.
        /// </summary>
        private float _positionToMoveY = -40f;

        /// <summary>
        /// How long the notification is visible.
        /// </summary>
        private float _notificationViewTime = 4f;

        private void Start()
        {
            _isNotificationPanelFree = true;
        }

        /// <summary>
        /// Show notification with some text.
        /// </summary>
        /// <param name="message">Message to show in notification.</param>
        public void ShowNotification(string message)
        {
            if (!_isNotificationPanelFree)
                return;
            
            _isNotificationPanelFree = false;

            _notificationText.text = message;
            
            StartCoroutine(NotificationProcess());
        }

        /// <summary>
        /// Force off notification without animation.
        /// </summary>
        public void ForceOffNotification()
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, _homePositionY);
        }

        private IEnumerator NotificationProcess()
        {
            float speedOfSlideInPx = 2f;
            float totalSlideLength = (Math.Abs(_homePositionY) + Math.Abs(_positionToMoveY)) / speedOfSlideInPx; 
            
            // Move notification down.
            for (float i = 1; i <= totalSlideLength; i++)
            {
                GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, GetComponent<RectTransform>().anchoredPosition.y - speedOfSlideInPx);
                yield return new WaitForSeconds(0.01f);
            }
            GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, _positionToMoveY);
            
            yield return new WaitForSeconds(_notificationViewTime);
            
            // Move notification up.
            for (float i = 1; i <= totalSlideLength; i++)
            {
                GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, GetComponent<RectTransform>().anchoredPosition.y + speedOfSlideInPx);
                yield return new WaitForSeconds(0.01f);
            }
            GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, _homePositionY);
            
            _isNotificationPanelFree = true;
            yield return null;
        }
    }
}