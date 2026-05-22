using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using CitiesHarmony.API;
using HarmonyLib;

namespace LaneController.KianCommons.Utils
{
    public static class HarmonyUtil
    {
        private static bool harmonyInstalled_;

        private const string errorMessage_ = "****** ERRRROOORRRRRR!!!!!!!!!! **************\n**********************************************\n    HARMONY MOD DEPENDANCY IS NOT INSTALLED!\n\nsolution:\n - exit to desktop.\n - unsub harmony mod.\n - make sure harmony mod is deleted from the content folder\n - resub to harmony mod.\n - run the game again.\n**********************************************\n**********************************************\n";

        internal static void AssertCitiesHarmonyInstalled()
        {
            if (!HarmonyHelper.IsHarmonyInstalled)
            {
                throw new Exception("****** ERRRROOORRRRRR!!!!!!!!!! **************\n**********************************************\n    HARMONY MOD DEPENDANCY IS NOT INSTALLED!\n\nsolution:\n - exit to desktop.\n - unsub harmony mod.\n - make sure harmony mod is deleted from the content folder\n - resub to harmony mod.\n - run the game again.\n**********************************************\n**********************************************\n");
            }
        }

        internal static void InstallHarmony(string harmonyID)
        {
            try
            {
                if (harmonyInstalled_)
                {
                    Log.Info("skipping harmony installation because its already installed");
                    return;
                }
                AssertCitiesHarmonyInstalled();
                Log.Info("Patching...");
                PatchAll(harmonyID, null, null);
                harmonyInstalled_ = true;
                Log.Info("Patched.");
            }
            catch (TypeLoadException inner)
            {
                new TypeLoadException("****** ERRRROOORRRRRR!!!!!!!!!! **************\n**********************************************\n    HARMONY MOD DEPENDANCY IS NOT INSTALLED!\n\nsolution:\n - exit to desktop.\n - unsub harmony mod.\n - make sure harmony mod is deleted from the content folder\n - resub to harmony mod.\n - run the game again.\n**********************************************\n**********************************************\n", inner).Exception();
            }
            catch (Exception ex)
            {
                ex.Exception();
            }
        }

        internal static void InstallHarmony<T>(string harmonyID) where T : Attribute
        {
            try
            {
                AssertCitiesHarmonyInstalled();
                Log.Info("Patching...");
                PatchAll(harmonyID, typeof(T));
                Log.Info("Patched.");
            }
            catch (TypeLoadException inner)
            {
                new TypeLoadException("****** ERRRROOORRRRRR!!!!!!!!!! **************\n**********************************************\n    HARMONY MOD DEPENDANCY IS NOT INSTALLED!\n\nsolution:\n - exit to desktop.\n - unsub harmony mod.\n - make sure harmony mod is deleted from the content folder\n - resub to harmony mod.\n - run the game again.\n**********************************************\n**********************************************\n", inner).Exception();
            }
            catch (Exception ex)
            {
                ex.Exception();
            }
        }

        internal static void InstallHarmony(string harmonyID, Type required = null, Type forbidden = null)
        {
            try
            {
                AssertCitiesHarmonyInstalled();
                Log.Info("Patching...");
                PatchAll(harmonyID, required, forbidden);
                Log.Info("Patched.");
            }
            catch (TypeLoadException inner)
            {
                new TypeLoadException("****** ERRRROOORRRRRR!!!!!!!!!! **************\n**********************************************\n    HARMONY MOD DEPENDANCY IS NOT INSTALLED!\n\nsolution:\n - exit to desktop.\n - unsub harmony mod.\n - make sure harmony mod is deleted from the content folder\n - resub to harmony mod.\n - run the game again.\n**********************************************\n**********************************************\n", inner).Exception();
            }
            catch (Exception ex)
            {
                ex.Exception();
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void PatchAll(string harmonyID)
        {
            //IL_0001: Unknown result type (might be due to invalid IL or missing references)
            //IL_0007: Expected O, but got Unknown
            Harmony val = new(harmonyID);
            val.PatchAll();
            val.LogPatchedMethods();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void PatchAll(string harmonyID, Type required = null, Type forbidden = null)
        {
            //IL_0001: Unknown result type (might be due to invalid IL or missing references)
            //IL_0007: Expected O, but got Unknown
            try
            {
                Harmony val = new(harmonyID);
                Assembly executingAssembly = Assembly.GetExecutingAssembly();
                Type[] typesFromAssembly = AccessTools.GetTypesFromAssembly(executingAssembly);
                foreach (Type type in typesFromAssembly)
                {
                    try
                    {
                        if (required != null && !type.HasAttribute(required) || forbidden != null && type.HasAttribute(forbidden))
                        {
                            continue;
                        }
                        if (type.HasAttribute<HarmonyPatch>(inherit: true))
                        {
                            Log.Info("applying " + type.FullName + " ...");
                        }
                        List<MethodInfo> list = val.CreateClassProcessor(type).Patch();
                        if (!list.IsNullorEmpty())
                        {
                            string text = list.Select((item) => item.DeclaringType?.ToString() + "." + item.Name).Join(", ");
                            Log.Info(type.FullName + " successfully patched : " + text);
                        }
                    }
                    catch (Exception innerException)
                    {
                        new Exception($"{type} failed.", innerException).Exception();
                    }
                }
                val.LogPatchedMethods();
            }
            catch (Exception ex)
            {
                ex.Exception();
            }
        }

        public static void LogPatchedMethods(this Harmony harmony)
        {
            foreach (MethodBase patchedMethod in harmony.GetPatchedMethods())
            {
                Log.Info("harmony(" + harmony.Id + ") patched: " + patchedMethod.DeclaringType.FullName + "::" + patchedMethod.Name, copyToGameLog: true);
            }
        }

        internal static void UninstallHarmony(string harmonyID)
        {
            AssertCitiesHarmonyInstalled();
            Log.Info("UnPatching...");
            UnpatchAll(harmonyID);
            harmonyInstalled_ = false;
            Log.Info("UnPatched.");
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void UnpatchAll(string harmonyID)
        {
            //IL_0001: Unknown result type (might be due to invalid IL or missing references)
            //IL_0007: Expected O, but got Unknown
            Harmony val = new(harmonyID);
            val.UnpatchAll(harmonyID);
        }

        internal static void ManualPatch<T>(string harmonyID)
        {
            AssertCitiesHarmonyInstalled();
            ManualPatchUnSafe(typeof(T), harmonyID);
        }

        internal static void ManualPatch(Type t, string harmonyID)
        {
            AssertCitiesHarmonyInstalled();
            ManualPatchUnSafe(t, harmonyID);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ManualPatchUnSafe(Type t, string harmonyID)
        {
            //IL_0070: Unknown result type (might be due to invalid IL or missing references)
            //IL_0077: Expected O, but got Unknown
            try
            {
                MethodBase methodBase = ReflectionHelpers.InvokeMethod(t, "TargetMethod") as MethodBase;
                Log.Info($"{t.FullName}.TorgetMethod()->{methodBase}", copyToGameLog: true);
                Assertion.AssertNotNull(methodBase, t.FullName + ".TargetMethod() returned null");
                HarmonyMethod harmonyMethod = GetHarmonyMethod(t, "Prefix");
                HarmonyMethod harmonyMethod2 = GetHarmonyMethod(t, "Postfix");
                HarmonyMethod harmonyMethod3 = GetHarmonyMethod(t, "Transpiler");
                HarmonyMethod harmonyMethod4 = GetHarmonyMethod(t, "Finalizer");
                Harmony val = new(harmonyID);
                val.Patch(methodBase, harmonyMethod, harmonyMethod2, harmonyMethod3, harmonyMethod4);
            }
            catch (Exception ex)
            {
                ex.Exception();
            }
        }

        public static HarmonyMethod GetHarmonyMethod(Type t, string name)
        {
            //IL_0025: Unknown result type (might be due to invalid IL or missing references)
            //IL_002b: Expected O, but got Unknown
            MethodInfo method = ReflectionHelpers.GetMethod(t, name, throwOnError: false);
            if (method == null)
            {
                return null;
            }
            Assertion.Assert(method.IsStatic, $"{method}.IsStatic");
            return new HarmonyMethod(method);
        }
    }
}
