using BepInEx;
using UnhollowerRuntimeLib;
using UnityEngine;
using HarmonyLib;

namespace Zeprus.Example {
    [BepInPlugin("com.zeprus.sap_custom_pet_example", "SAP Custom Pet Example", "1.0")]
    [BepInProcess("Super Auto Pets.exe")]
    [BepInDependency("com.zeprus.sap_custom_pet_manager")]
    public class BepInExLoader : BepInEx.IL2CPP.BasePlugin {

        public const string
            MODNAME = "sap_custom_pet_example",
            AUTHOR = "zeprus",
            GUID = "com." + AUTHOR + "." + MODNAME,
            VERSION = "1";

        public static BepInEx.Logging.ManualLogSource log;
        public BepInExLoader() {
            log = Log;

            try{
                Harmony harmonyInstance = new Harmony("zeprus.sap_custom_pet_example");
                harmonyInstance.PatchAll();
            } catch(HarmonyException e) {
                log.LogError(e.ToString());
                log.LogError(e.StackTrace);
            }
        }

        public override void Load()
        {
            log.LogMessage(GUID + ": Registering...");
            
            try {
                var gameObject = new GameObject("CustomPetExample");
                ClassInjector.RegisterTypeInIl2Cpp<PetRockExample>();;
                gameObject.AddComponent<PetRockExample>();


                log.LogMessage(GUID + ": Finished registering.");
            } catch {
                log.LogError(GUID + ": Failed to register classes.");
            }
        }
    }
}