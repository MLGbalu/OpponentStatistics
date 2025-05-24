using HarmonyLib;

namespace OpponentStatistics
{
    [HarmonyPatch(typeof(LobbyController))]
    [HarmonyPatch(nameof(LobbyController.CreateHostPlayerItem))]
    class CreateHostPlayerItemHook
    {
        static void Postfix()
        {
            Plugin.FindAndShowOpponent();
        }
    }

    [HarmonyPatch(typeof(LobbyController))]
    [HarmonyPatch(nameof(LobbyController.CreateClientPlayerItem))]
    class CreateClientPlayerItemHook
    {
        static void Postfix()
        {
            Plugin.FindAndShowOpponent();
        }
    }
}
