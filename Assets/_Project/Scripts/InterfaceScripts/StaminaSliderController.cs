using _Project.Scripts.PlayerScripts;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.InterfaceScripts 
{
    // скрипт отвечающий за управлением слайдера для сынсы мыши (в настройках игры регулируется)
    public class StaminaSliderController : MonoCache
    {
        [SerializeField] RectTransform sliderRectTransform;

        [SerializeField] PlayerMovement playerMovement;

        [SerializeField] public Slider sliderStamina;

        [SerializeField] Image fillSlider;

        [SerializeField] float speedDecreasedStaminaSlider = 1f;
        [SerializeField] float speedIncreasedStaminaSlider = 1f;

        float multiplier = 2f;

        internal bool endStamina = false;
        int mediumCountStaminaForJumping = 100;

        int maxValueSlider = 750;

        public bool isHasAdrenaline = false;

        private void Awake()
        {
            sliderStamina.value = maxValueSlider;
            sliderStamina.gameObject.SetActive(false);
        }

        public override void OnTick()
        {
            CheckStaminaSlider();

            // пример как должно быть -------------
            if (isHasAdrenaline == true)
                sliderRectTransform.DOShakeScale(1f, 
                    strength: .1f, 
                    vibrato: 0, 
                    randomness: 0f, 
                    fadeOut: false);
        }

        public void CheckStaminaSlider()
        {
            if (sliderStamina.value < maxValueSlider 
                && playerMovement.IsKeyPressLeftShift == false) // is working!!!
                IncreasedStaminaWalk(); // if player walking

            if (sliderStamina.value == 0) // if stamina the end
                endStamina = true;

            if (sliderStamina.value == maxValueSlider)
            {
                sliderStamina.gameObject.SetActive(false);
                endStamina = false;
            }

            // пример как должно быть -------------

            if (Input.GetKeyDown(KeyCode.UpArrow) /*isHasAdrenaline == true*/) 
            {
                fillSlider.DOColor(Color.red, 1f);

                isHasAdrenaline = true;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                fillSlider.DOColor(Color.white, 1f);

                // он должен работать до определенного момента
                isHasAdrenaline = false;
            }

            // -------------------------------------
        }

        public void DecreasedStamina()
        {
            sliderStamina.gameObject.SetActive(true);
            sliderStamina.value -= speedDecreasedStaminaSlider;
        }

        public void IncreasedStaminaIdle()
        {
            sliderStamina.gameObject.SetActive(true);
            sliderStamina.value += speedIncreasedStaminaSlider * multiplier;
        }

        public void IncreasedStaminaWalk()
        {
            sliderStamina.gameObject.SetActive(true);
            sliderStamina.value += speedIncreasedStaminaSlider;
        }

        public void DecreasedStaminaFromJump() => sliderStamina.value -= mediumCountStaminaForJumping;
    }
}


