using UnityEngine;

namespace _Project.Scripts.InventoryScripts
{
    // скрипт отвечающий за счет добавленных пердметов в инвентарь
    public static class CounterAddItemsController
    {
        [HideInInspector]
        public static int countAddSurvivalItems = 0, countAddRepairCarItems = 0;

        public static void CounterAddSurvivalItems(RaycastHit hit)
        {
            if (hit.collider.tag == "ItemsInteractionForSurvival")
                countAddSurvivalItems++;
        }

        public static void CounterAddRepairCarItems(RaycastHit hit) 
        {
            if (hit.collider.tag == "ItemsInteractionForCarRepair")
                countAddRepairCarItems++;
        }
    }
}