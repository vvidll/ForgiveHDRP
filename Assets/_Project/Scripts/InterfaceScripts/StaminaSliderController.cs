using _Project.Scripts.PlayerScripts;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.InterfaceScripts 
{
    // скрипт отвечающий за управлением слайдера для сынсы мыши (в настройках игры регулируется)
    public class StaminaSliderController : MonoCache
    {
        [SerializeField] PlayerMovement playerMovement;

        [SerializeField] public Slider sliderStamina;
        [SerializeField] float speedDecreasedStaminaSlider = 1f;
        [SerializeField] float speedIncreasedStaminaSlider = 1f;

        float multiplier = 2f;

        internal bool endStamina = false;
        int mediumCountStaminaForJumping = 100;

        int maxValueSlider = 750;

        private void Awake()
        {
            sliderStamina.value = maxValueSlider;
            sliderStamina.gameObject.SetActive(false);
        }

        public override void OnTick()
        {
            CheckStaminaSlider();
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


