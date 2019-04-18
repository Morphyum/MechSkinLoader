using BattleTech;
using BattleTech.Rendering.MechCustomization;
using Harmony;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MechSkinLoader {

    [HarmonyPatch(typeof(MechRepresentationSimGame), "LoadCustomization")]
    public static class MechRepresentationSimGame_LoadCustomizationy_Patch {
        static void Prefix(MechRepresentationSimGame __instance) {
            try {
                string folder = Path.Combine(MechSkinLoader.ModDirectory, __instance.mechDef.Chassis.PrefabBase);
                if (Directory.Exists(folder)) {
                    List<string> skins = new List<string>();
                    string[] files = Directory.GetFiles(folder);
                    foreach (string file in files) {
                        skins.Add(Path.GetFileName(file));
                    }
                    foreach (string skinname in skins) {
                        bool flag = false;
                        Texture2D[] array = __instance.mechCustomization.paintPatterns;
                        for (int i = 0; i < array.Length; i++) {
                            if (array[i].name == skinname) {
                                flag = true;
                            }
                        }
                        if (!flag) {
                            Texture2D texture2D = UnityEngine.Object.Instantiate<Texture2D>(__instance.mechCustomization.paintPatterns[0]);
                            texture2D.name = skinname;
                            byte[] data = File.ReadAllBytes(Path.Combine(folder, skinname));
                            texture2D.LoadImage(data);
                            Array.Resize<Texture2D>(ref __instance.mechCustomization.paintPatterns, __instance.mechCustomization.paintPatterns.Length + 1);
                            __instance.mechCustomization.paintPatterns[__instance.mechCustomization.paintPatterns.Length - 1] = texture2D;
                            MechSkinLoader.Logger.Log("Added " + skinname + " For " + __instance.mechDef.Chassis.PrefabBase);
                        }
                    }
                }
                else {
                    MechSkinLoader.Logger.LogIfDebug("Not found " + __instance.mechDef.Chassis.PrefabBase);
                }
            }
            catch (Exception e) {
                MechSkinLoader.Logger.LogError(e);
            }
        }
    }

    [HarmonyPatch(typeof(Mech), "AddToTeam")]
    public static class Mech_AddToTeam_Patch {
        static void Prefix(Mech __instance) {
            try {
                string folder = Path.Combine(MechSkinLoader.ModDirectory, __instance.MechDef.Chassis.PrefabBase);
                if (Directory.Exists(folder)) {
                    PilotableActorRepresentation pilotableActorRepresentation = __instance.GameRep as PilotableActorRepresentation;
                    if (pilotableActorRepresentation == null) {
                        MechSkinLoader.Logger.Log("pilotableActorRepresentation is null");
                    }
                    else {
                        MechCustomization ___mechCustomization = (MechCustomization)AccessTools.Method(typeof(PilotableActorRepresentation), "get_mechCustomization").Invoke(pilotableActorRepresentation, new object[] { });
                        if (___mechCustomization == null) {
                            MechSkinLoader.Logger.Log("___mechCustomization is null");
                        }
                        else {
                            List<string> skins = new List<string>();
                            string[] files = Directory.GetFiles(folder);
                            foreach (string file in files) {
                                skins.Add(Path.GetFileName(file));
                            }
                            foreach (string skinname in skins) {
                                bool flag = false;
                                Texture2D[] array = ___mechCustomization.paintPatterns;
                                for (int i = 0; i < array.Length; i++) {
                                    if (array[i].name == skinname) {
                                        flag = true;
                                    }
                                }
                                if (!flag) {
                                    Texture2D texture2D = UnityEngine.Object.Instantiate<Texture2D>(___mechCustomization.paintPatterns[0]);
                                    texture2D.name = skinname;
                                    byte[] data = File.ReadAllBytes(Path.Combine(folder, skinname));
                                    texture2D.LoadImage(data);
                                    Array.Resize<Texture2D>(ref ___mechCustomization.paintPatterns, ___mechCustomization.paintPatterns.Length + 1);
                                    ___mechCustomization.paintPatterns[___mechCustomization.paintPatterns.Length - 1] = texture2D;
                                }
                            }
                        }
                    }
                }
                else {
                    MechSkinLoader.Logger.LogIfDebug("Not found " + __instance.MechDef.Chassis.PrefabBase);
                }
            }
            catch (Exception e) {
                MechSkinLoader.Logger.LogError(e);
            }
        }
    }

}