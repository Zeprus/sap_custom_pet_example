using System;
using UnityEngine;
using HarmonyLib;
using Spacewood.Core.Enums;
using Spacewood.Core.Models.Abilities;
using Zeprus.Sap;


namespace Zeprus.Example {
    public class PetRockExample : MonoBehaviour {
        
        private static BepInEx.Logging.ManualLogSource log;
        
        public PetRockExample(IntPtr ptr) : base(ptr) {
            log = BepInExLoader.log;
        }

        static void createPetRock() {
            CustomPet petRock = CustomPetManager.CreateCustomPet("Pet Rock");
            MinionTemplate petRockTemplate = petRock.GetTemplate();
            MinionEnum petRockEnum = petRock.GetEnum();
            
            petRockTemplate.SetName("Pet Rock");
            petRockTemplate.SetAbout("Your only true friend.");
            petRockTemplate.SetStats(1, 50);
            // add a pre-existing ability
            petRockTemplate.AddAbility(AbilityEnum.OnHurtGainShield);
            // add some default food-effect to the pet
            petRockTemplate.SetPerk(Perk.CoconutShield);
            petRockTemplate.SetPerkAbout(Perk.CoconutShield);
            // assign the pet to a pack
            petRockTemplate.AddPack(Pack.Pack1);
            // idk what this does or if it's necessary
            petRockTemplate.SetActive(true);
            // same here, no clue what purpose archetypes serve
            petRockTemplate.SetArchetype(Archetype.Standard);
            // this is probably used for sloth? maybe?
            petRockTemplate.SetRollable(true);
        }

        [HarmonyPatch]
        class MinionEnumHooks {
            // Early hook to add the pet to a pack and get it initialized, there's a probably a better way
            [HarmonyPrefix]
            [HarmonyPatch(typeof(MinionConstants), "CreatePackOneAndTwoMinions")]
            public static void prefixCreatePackOneAndTwoMinions() {
                createPetRock();
            }
        }
    }
}