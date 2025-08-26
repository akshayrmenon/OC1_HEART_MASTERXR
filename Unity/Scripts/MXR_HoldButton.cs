using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MXR
{
    public class MXR_HoldButton : Selectable, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        [Header("Button specs")]
        public float holdTime = 2f;  // How long to hold
        public Image loaderFillImage; // Assign in inspector

        private float holdTimer = 0f;
        private bool isHolding = false;

        [Serializable]
        public class ButtonHoldClickedEvent : UnityEvent { }

        [Header("--- Event ---")]
        [SerializeField]
        private ButtonHoldClickedEvent onClickAndHold = new ButtonHoldClickedEvent();

        protected override void  Start()
        {
            loaderFillImage.fillAmount = 0;
        }

        void Update()
        {
            if (isHolding)
            {
                holdTimer += Time.deltaTime;
                loaderFillImage.fillAmount = holdTimer / holdTime;
                if (holdTimer >= holdTime)
                {
                    isHolding = false;
                    loaderFillImage.fillAmount = 1f;
                    TriggerAction();
                }
            }
        }

        // ----- Pointer data -----
        public override void OnPointerDown(PointerEventData eventData)
        {
            isHolding = true;
            holdTimer = 0f;
            loaderFillImage.fillAmount = 0f;
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            ResetHold();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            ResetHold();
        }

        // ----- Custom functions -----
        void ResetHold()
        {
            isHolding = false;
            holdTimer = 0f;
            loaderFillImage.fillAmount = 0f;
        }

        void TriggerAction()
        {
            Debug.Log("Hold complete! Action triggered.");
            onClickAndHold.Invoke();
            // Place your logic here (e.g., loading a scene, confirming an action)
        }
    }
}