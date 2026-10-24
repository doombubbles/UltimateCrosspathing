global using System.Linq;
global using BTD_Mod_Helper;
global using BTD_Mod_Helper.Api;
using System;
using BTD_Mod_Helper.Api.Helpers;
using Il2CppAssets.Scripts.Unity.UI_New.Popups;
using MelonLoader;
using UnityEngine.Scripting;
using UltimateCrosspathing;

[assembly:
    MelonInfo(typeof(UltimateCrosspathingMod), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6-Epic")]
[assembly: MelonPriority(-500)]

namespace UltimateCrosspathing;

public class UltimateCrosspathingMod : BloonsTD6Mod
{
    private const string HelpUrl = "https://github.com/doombubbles/ultimate-crosspathing/blob/main/HELP.md";

#if RELEASE
    public override void OnLateInitializeMelon()
    {
        // There seems to be a crash from the Garbage Collector that can happen while doing the big load of UC data
        GarbageCollector.GCMode = GarbageCollector.Mode.Disabled;
    }
#endif

    public override void OnMainMenu()
    {
#if RELEASE
        // Fallback in case the `finally` in the Load Task is ever not reach somehow
        GarbageCollector.GCMode = GarbageCollector.Mode.Enabled;
#endif

        var failedTowers = ModContent.GetContent<LoadInfo>().Count(info => info.loaded != true && info.Enabled);

        if (failedTowers > 0)
        {
            TaskScheduler.ScheduleTask(() => PopupScreen.instance.ShowPopup(PopupScreen.Placement.menuCenter,
                    "Crosspathing Issue",
                    $"{failedTowers} tower(s) failed to Ultimately Crosspath. " +
                    "An update to the mod, Mod Helper, or MelonLoader may be required. Press 'Help' to be taken to the common issues page.",
                    new Action(() => ProcessHelper.OpenURL(HelpUrl)), "Help", null, "Cancel",
                    Popup.TransitionAnim.Scale),
                () => PopupScreen.instance != null && !PopupScreen.instance.IsPopupActive()
            );
        }
    }
}