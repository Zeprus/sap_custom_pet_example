using System;
using UnityEngine;
using HarmonyLib;
using Spacewood.Unity;
using Il2CppSystem.Collections.Generic;
using Spacewood.Core.Enums;
using Spacewood.Core.Models.Abilities;
using Spacewood.Core.Models.Abilities.Triggers;
using Spacewood.Core.Models.Abilities.Effects;
using Spacewood.Core.Models.Abilities.Parameters;
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
            //create ability
            CustomAbilityCollection abilities = createPetRockAbilities();
            // add ability
            petRockTemplate.AddAbility(abilities.GetEnum());
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

        static CustomAbilityCollection createPetRockAbilities(){
            //create Base for the abilities
            CustomAbilityCollection petRockCustomAbilityCollection = CustomPetManager.CreateCustomAbilityCollection();
            AbilityCollection abilityCollection = petRockCustomAbilityCollection.GetAbilityCollection();

            // Configure Ability effects
                // create self-targeting
                TargetsSelf targetSelf = new TargetsSelf();

                // create effect to set stats
                EffectSetStats setAttack = new EffectSetStats();
                setAttack.Attack = new ParameterInt().SetMultiplier(0);
                setAttack.Health = new ParameterInt().SetMultiplier(1);
                setAttack.SetType(SetMinionStatsType.Standard);

                // create end-turn trigger
                TriggerEndTurn endTurnTrigger = new TriggerEndTurn();
                
                // configure abilities
                Ability abilityLvl1 = abilityCollection.GetAbility(1);
                // Set ability trigger
                abilityLvl1.SetTrigger(endTurnTrigger);
                // Add effect
                abilityLvl1.AddEffect(targetSelf, setAttack);
                //set flavor text
                abilityLvl1.About = "End of turn: set own Attack to 0.";
                
                Ability abilityLvl2 = abilityCollection.GetAbility(2);
                abilityLvl2.SetTrigger(endTurnTrigger);
                abilityLvl2.AddEffect(targetSelf, setAttack);
                abilityLvl2.About = "End of turn: set own Attack to 0.";

                Ability abilityLvl3 = abilityCollection.GetAbility(3);
                abilityLvl3.SetTrigger(endTurnTrigger);
                abilityLvl3.AddEffect(targetSelf, setAttack);
                abilityLvl3.About = "End of turn: set own Attack to 0.";

            // Configure Ability text
                AbilityAsset abilityAsset = petRockCustomAbilityCollection.GetAsset();
                string abilityLvl1Text = "End of turn: set own Attack to 0.";
                string abilityLvl2Text = "End of turn: set own Attack to 0.";
                string abilityLvl3Text = "End of turn: set own Attack to 0.";
                List<string> abilityText = new List<string>();
                abilityText.Add(abilityLvl1Text);
                abilityText.Add(abilityLvl2Text);
                abilityText.Add(abilityLvl3Text);
                abilityAsset.About = abilityText;

            return petRockCustomAbilityCollection;
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