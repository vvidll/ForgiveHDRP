using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.InventoryScripts
{
    // скрипт отвечающий за проверку выполненных задач(например игрок подобрал нужное кол-во предметов опеределенного типа - для выживания)
    public class CheckCompleteTasksNotepad : MonoCache
    {
        [SerializeField] Toggle[] toggleItemsForSurvival;

        [SerializeField] Toggle[] toggleItemsForCarRepair;

        [HideInInspector]
        public bool completeMissionItemsForSurvival = false, completeMissionItemsForRepairCar = false;

        int maxAddItemsSurvival = 6;
        int maxAddItemsRepairCar = 3;

        public override void OnTick()
        {
            CheckTogglesItemsForSurvival();
        }

        public void CheckTogglesItemsForSurvival()
        {
            if (CounterAddItemsController.countAddSurvivalItems == maxAddItemsSurvival)
                completeMissionItemsForSurvival = true;

            if (CounterAddItemsController.countAddRepairCarItems == maxAddItemsRepairCar)
                completeMissionItemsForRepairCar = true;
            
        }
    }
}
