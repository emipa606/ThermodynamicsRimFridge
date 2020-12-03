using DThermodynamicsCore.Comps;
using HarmonyLib;
using RimFridge;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace ThermodynamicsRimFridge
{

    [StaticConstructorOnStartup]
    static class ThermodynamicsRimFridge
    {
        static ThermodynamicsRimFridge()
        {
            var harmony = new Harmony("Mlie.ThermodynamicsRimFridge");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        [HarmonyPatch(typeof(CompDTemperature))]
        [HarmonyPatch("AmbientTemperature",MethodType.Getter)]
        public class Prefix_IncidentWorker_VisitorGroup
        {
            [HarmonyPrefix]
            public static bool Prefix(ref CompDTemperature __instance, ref double __result)
            {
                if (!__instance.parent.Spawned)
                {
                    return true;
                }
                List<Thing> thingList = __instance.parent.PositionHeld.GetThingList(__instance.parent.MapHeld);
                foreach (Thing thing in thingList)
                {
                    var compRefrigerator = thing.TryGetComp<CompRefrigerator>();
                    if (compRefrigerator == null)
                    {
                        continue;
                    }
                    __result = compRefrigerator.currentTemp;
                    //Log.Message($"Current temp: {__result}");
                    return false;
                }
                return true;
            }
        }
    }
}