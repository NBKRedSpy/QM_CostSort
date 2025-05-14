using HarmonyLib;
using MGSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM_ImprovedSort
{
    public static class Plugin
    {

        [Hook(ModHookType.AfterBootstrap)]
        public static void Init(IModContext modContext)
        {
            new Harmony("nbk_redspy.QM_ImprovedSort").PatchAll();
        }
    }
}
