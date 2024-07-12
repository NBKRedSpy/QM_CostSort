using HarmonyLib;
using MGSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM_CostSort
{
    [HarmonyPatch(typeof(ItemStorage), nameof(ItemStorage.SortingComparsion))]
    public static class ItemStorageSort_Patch
    {
        public static bool Prefix(BasePickupItem x, BasePickupItem y, ref int __result)
        {
            //---Default game sort by size.
            if (x.InventoryWidthSize != y.InventoryWidthSize)
            {
                if (x.InventoryWidthSize >= y.InventoryWidthSize)
                {
                    __result = -1;
                    return false;
                }

                __result = 1;
                return false;
            }

            //--Cost sort
            //  They should all be PickupItems, but just in case.
            PickupItem piX = x as PickupItem;
            PickupItem piY = y as PickupItem;

            if (piX != null && piY != null)
            {
                float xPrice = ((ItemRecord)(piX._records?.FirstOrDefault()))?.Price ?? 0f;
                float yPrice = ((ItemRecord)(piY._records?.FirstOrDefault()))?.Price ?? 0f;

                //Sort by price descending
                __result = xPrice.CompareTo(yPrice) * -1;

                if (__result != 0)
                {
                    return false;
                }
            }

            //--Game's name sort.
            __result = string.CompareOrdinal(x.Id, y.Id);

            return false;
        }

    }
}
