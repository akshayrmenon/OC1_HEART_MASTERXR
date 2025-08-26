using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MXR
{
    public enum StageEnum { Start, StartCalibration, Calibration, StartTeleoperation, Teleoperation, AutonomousOperation };

    public class MXR_GUI_Interactions : MonoBehaviour
    {
        [Header("--- Canvas ---")]
        public GameObject canvasStart;
        public GameObject canvasStartCalibration;
        public GameObject canvasCalibration;
        public GameObject canvasStartTeleoperation;
        public GameObject canvasTeleoperation;

        [Header("--- Buttons ---")]
        public GameObject confirmButton;
        public GameObject autonomousOperationButton;

        [Header("--- Message Consoles ---")]
        public TMP_Text messageStartCalibration;
        public TMP_Text messageCalibration;
        public TMP_Text messageStartTeleoperation;
        public TMP_Text messageTeleoperation;
        [Header("--- Message Console Contents ---")]
        public string startCalibrationMessage = "Make sure the glove is put on your right hand and when ready click on 'Start Calibration'";
        public string calibrationMessage = "Place your hand with the glove at the same position as the blue hand sphere and then click on 'Confirm Calibrate'";
        public string startTeleoperationMessage = "Keep your hand at the calibrated spot and if your ready start the teleoperation";
        public string autonomousOperationMessage = "The Autonomous Operation";
        public string[] taskMessage;

        [Header("--- Camera ---")]
        public TMP_Text headCamText;
        public TMP_Text sideCamText;

        [Header("--- Calibration hand ---")]
        public GameObject hand;

        [Header("--- Bool Publisher ---")]
        public BoolPublisher boolPublisher;

        [Header("--- Events ---")]
        public UnityEvent startAutonomousMode;

        [Header ("--- Debug ---")]
        public StageEnum currentStage = StageEnum.Start;

        // private vars
        private int teleoperationStage = 0;
        private bool switchCamera = false;

        private void Start()
        {
            currentStage = StageEnum.Start;
            teleoperationStage = 0;
            SetGUI();
        }

        private void SetGUI()
        {
            canvasStart.SetActive(currentStage == StageEnum.Start ? true : false);
            canvasStartCalibration.SetActive(currentStage == StageEnum.StartCalibration ? true : false);
            canvasCalibration.SetActive(currentStage == StageEnum.Calibration ? true : false);
            canvasStartTeleoperation.SetActive(currentStage == StageEnum.StartTeleoperation ? true : false);
            canvasTeleoperation.SetActive(currentStage == StageEnum.Teleoperation || currentStage == StageEnum.AutonomousOperation ? true : false);
            hand.SetActive(currentStage == StageEnum.Calibration ? true : false);

            switch (currentStage)
            {
                case StageEnum.Start:

                    break;
                case StageEnum.StartCalibration:
                    messageStartCalibration.text = startCalibrationMessage;
                    break;
                case StageEnum.Calibration:
                    messageCalibration.text = calibrationMessage;
                    break;
                case StageEnum.StartTeleoperation:
                    messageStartTeleoperation.text = startTeleoperationMessage;
                    break;
                case StageEnum.Teleoperation:
                    confirmButton.SetActive(true);
                    autonomousOperationButton.SetActive(false);
                    messageTeleoperation.text = taskMessage.Length > 0 ? taskMessage[0] : "";
                    break;
                case StageEnum.AutonomousOperation:
                    messageTeleoperation.text = autonomousOperationMessage;
                    startAutonomousMode.Invoke();
                    autonomousOperationButton.SetActive(false);
                    break;
            }
        }

        private void SetTeleoperation()
        {
            messageTeleoperation.text = taskMessage[teleoperationStage];

            // Keep BoolPublisher up to date
            boolPublisher.currentIndex = teleoperationStage;
            boolPublisher.lastIndex = taskMessage.Length - 1;

            if (teleoperationStage == taskMessage.Length - 1)
            {
                confirmButton.SetActive(false);
                autonomousOperationButton.SetActive(true);
            }
        }

        public void ButtonStart()
        {
            currentStage = StageEnum.StartCalibration;
            SetGUI();
            
        }

        public void ButtonStartCalibration()
        {
            currentStage = StageEnum.Calibration;
            SetGUI();
        }

        public void ButtonConfirmCalibration()
        {
            currentStage = StageEnum.StartTeleoperation;
            SetGUI();
        }

        public void ButtonStartTeleoperation()
        {
            currentStage = StageEnum.Teleoperation;
            SetGUI();
            SetTeleoperation();
        }

        public void ButtonConfirmTeleoperation()
        {
            teleoperationStage++;
            SetTeleoperation();
        }

        public void ButtonStartAutonomousOperation()
        {
            currentStage = StageEnum.AutonomousOperation;
            SetGUI();
        }

        public void ButtonReset()
        {
            currentStage = StageEnum.Calibration;
            teleoperationStage = 2;
            SetGUI();
        }

        public void ButtonSwitchCamera()
        {
            switchCamera = !switchCamera;

            sideCamText.text = switchCamera ? "Head Camera" : "Side Camera";
            headCamText.text = switchCamera ? "Side Camera" : "Head Camera";
        }
    }
}
