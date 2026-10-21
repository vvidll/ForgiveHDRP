using _Project.Scripts.InterfaceScripts;
using _Project.Scripts.PlayerScripts;
using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.CameraScripts 
{
    // скрипт отвечающий за управление камеры
    public class CameraController : MonoCache
    {
        [SerializeField] Transform _bodyPlayer;
        [SerializeField] Transform headPlayer;
        [SerializeField] SensivityController sensitivityController;
        [SerializeField] DrivingPlayer drivingPlayer;
        [SerializeField] PlayerAnimation playerAnimation;
        float _yRotation = 0f;
        float _xRotation = 0f;

        float limitUpY = 70f, limitDownY = -60f;
        float limitRightX = 30f, limitLeftX = -30f;

        [SerializeField] Transform _headTarget;
        [SerializeField] Camera _mainCamera;

        [SerializeField] StaminaSliderController staminaSliderController;

        [HideInInspector]
        public float mouseX;
        [HideInInspector]
        public float mouseY;

        public override void OnTick()
        {
            PlayerRotateCamera();

            /*if (staminaSliderController.isHasAdrenaline == true)
                _mainCamera.DOShakeRotation(1f,
                    strength: .1f,
                    vibrato: 0,
                    randomness: 0f,
                    fadeOut: false);*/
        }

        public void PlayerRotateCamera() 
        {
            InputMouse();

            LimitsRotateCameraY();

            if(drivingPlayer.isInCar == true)
                LimitsRotateCameraX();

            if (drivingPlayer.isInCar == false) 
            {
                transform.localRotation = Quaternion.Euler(_yRotation, _xRotation, 0f);

                /*if (limitLeftX == -30f || limitRightX == 30f) 
                {
                    _bodyPlayer.Rotate(Vector3.up * mouseX);
                    //AnimationRotateBody();
                }*/

                _bodyPlayer.Rotate(Vector3.up * mouseX);

                //AnimationRotateBody();
            }

            CreateRayForHeadRotate();

            playerAnimation.ChangeTurnAnimation();
        }

        public void CreateRayForHeadRotate()
        {
            Ray ray = _mainCamera.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));
            Vector3 positionTarget = ray.origin + ray.direction;

            _headTarget.position = positionTarget;

            transform.localRotation = Quaternion.Euler(_yRotation, _xRotation, 0f);
            //headPlayer.Rotate(positionTarget);

        }

        public void InputMouse() 
        {
            mouseX = Input.GetAxis("Mouse X") * sensitivityController.currentSensivity * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * sensitivityController.currentSensivity * Time.deltaTime;
        }

        public void LimitsRotateCameraY()
        {
            _yRotation -= mouseY;
            _yRotation = Mathf.Clamp(_yRotation, limitDownY, limitUpY);
        }

        public void LimitsRotateCameraX() 
        {
            _xRotation += mouseX;
            _xRotation = Mathf.Clamp(_xRotation, limitLeftX, limitRightX);
        }

        public void ResetCameraRotation() 
        {
            _xRotation = 0;
            _yRotation = 0;

            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }
}

