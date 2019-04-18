using Harmony;
using System.Reflection;

namespace MechSkinLoader
{
    public class MechSkinLoader {
        internal static Logger Logger;
        internal static string ModDirectory;
        public static void Init(string directory, string settingsJSON) {
            ModDirectory = directory;
            Logger = new Logger(directory, "log", false);
            var harmony = HarmonyInstance.Create("de.morphyum.MechSkinLoader");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
