using Harmony;
using UnityEngine;
using Verse;

namespace DebugWindow
{
	[StaticConstructorOnStartup]
	class Main
	{
		static Main()
		{
			var harmony = HarmonyInstance.Create("net.pardeike.harmony.DebugWindow");
			harmony.PatchAll();
		}
	}

	[HarmonyPatch(typeof(Game), nameof(Game.LoadGame))]
	static class Game_LoadGame_Patch
	{
		static void Postfix()
		{
			Find.WindowStack.TryRemove(typeof(EditWindow_DebugInspector), true);
			Find.WindowStack.Add(new EditWindow_DebugInspector());

			Find.WindowStack.TryRemove(typeof(EditWindow_Log), true);
			Find.WindowStack.Add(new EditWindow_Log());
		}
	}

	[HarmonyPatch(typeof(Window), "SetInitialSizeAndPosition")]
	static class Window_SetInitialSizeAndPosition_Patch
	{
		static void Postfix(Window __instance)
		{
			var logWindow = __instance as EditWindow_Log;
			if (logWindow != null)
				logWindow.windowRect = new Rect(2, 2, 530, UI.screenHeight - 4 - 35);

			var inspector = __instance as EditWindow_DebugInspector;
			if (inspector != null)
				inspector.windowRect = new Rect(UI.screenWidth - 2 - 350, 2, 350, 150);
		}
	}
}