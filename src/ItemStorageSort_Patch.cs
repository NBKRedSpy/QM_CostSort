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

            //Note - The game has already grouped the items by type.  This is the sort of 
            //  the items in those groupings.

            //---Default game sort by size.
            __result = x.InventoryWidthSize.CompareTo(y.InventoryWidthSize);

            if (__result != 0) return false;


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

                if (__result != 0) return false;
            }

            //--Game's id sort.
            __result = string.CompareOrdinal(x.Id, y.Id);
            if (__result != 0) return false;

            __result = x.StackCount.CompareTo(y.StackCount) * -1;
            if (__result != 0) return false;

            //---Item durability
            float xPercent = x.Comp<BreakableItemComponent>()?.CurrentPercent ?? 0;
            float yPercent = y.Comp<BreakableItemComponent>()?.CurrentPercent ?? 0;

            __result = xPercent.CompareTo(yPercent) * -1;

            return false;
        }

    }
}
