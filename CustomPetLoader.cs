using System;
using UnityEngine;
using HarmonyLib;
using Spacewood.Core.Enums;
using Spacewood.Unity;

namespace Zeprus.Sap {
    public class CustomPetLoader : MonoBehaviour {
        
        private static BepInEx.Logging.ManualLogSource log;
        static MinionEnum petRockEnum;
        
        public CustomPetLoader(IntPtr ptr) : base(ptr) {
            log = BepInExLoader.log;
        }

        static void createPetRock() {
            // designate an enum for our pet, use length + 1 to avoid conflicts
            petRockEnum = (MinionEnum) (Enum.GetNames(typeof(MinionEnum)).Length + 1);
            // create the pets template
            MinionTemplate petRockTemplate = createPetRockTemplate(petRockEnum);
            // add the pet to the Minion dictionary so the game knows it exists
            MinionConstants.Minions.Add(petRockEnum, petRockTemplate);
        }

        static MinionTemplate createPetRockTemplate(MinionEnum minionEnum) {
            // create a MinionTemplate for our pet, this contains the pets attributes
            MinionTemplate petRockTemplate = new MinionTemplate(minionEnum);

            petRockTemplate.SetName("Pet Rock");
            petRockTemplate.SetAbout("Your only true friend.");
            petRockTemplate.SetStats(1, 50);
            // using a pre-existing ability
            petRockTemplate.AddAbility(Spacewood.Core.Models.Abilities.AbilityEnum.OnHurtGainShield);
            //add some default food-effect to the pet
            petRockTemplate.SetPerk(Perk.CoconutShield);
            petRockTemplate.SetPerkAbout(Perk.CoconutShield);
            // assign the pet to a pack
            petRockTemplate.AddPack(Pack.Pack1);
            // idk what this does or if it's necessary
            petRockTemplate.SetActive(true);
            // same here, no clue what purpose Archetypes serve
            petRockTemplate.SetArchetype(Archetype.Standard);
            // this is probably used for sloth? maybe?
            petRockTemplate.SetRollable(true);

            return petRockTemplate;
        }

        [HarmonyPatch]
        class MinionEnumHooks {
            // Early hook to add the pet to a pack and get it initialized, there's a probably a better way
            [HarmonyPrefix]
            [HarmonyPatch(typeof(MinionConstants), "CreatePackOneAndTwoMinions")]
            public static void prefixCreatePackOneAndTwoMinions() {
                log.LogInfo("CreatePackOneAndTwoMinions called!");
                log.LogInfo("Creating Pet Rock...");
                CustomPetLoader.createPetRock();
                log.LogInfo("Created Pet Rock with Enum " + CustomPetLoader.petRockEnum + ".");
            }

            [HarmonyPrefix]
            [HarmonyPatch(typeof(MinionLibrary), "Get")]
            public static bool prefixGet(MinionEnum value, ref MinionAsset __result) {
                // check if requested Assets are for our pet
                if(value == CustomPetLoader.petRockEnum) {
                    // create MinionAsset
                    MinionAsset minionAsset = ScriptableObject.CreateInstance<MinionAsset>();
                    // set fields
                    minionAsset.Name = "Pet Rock";
                    minionAsset.Enum = CustomPetLoader.petRockEnum;
                    // minionAsset.Sprite = some Sprite
                    // minionAsset.Sprite2x = some Sprite
                    // minionAsset.Animation = some UnityEngine.AnimationClip

                    // set return to our created MinionAsset
                    __result = minionAsset;

                    // prevent executing the regular MinionLibrary.Get function to avoid errors
                    return false;
                } else {
                    return true;
                }
            }
        }
    }
}