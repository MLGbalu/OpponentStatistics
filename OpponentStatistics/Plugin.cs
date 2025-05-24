using BepInEx;
using BepInEx.Logging;
using FishNet.Object;
using HarmonyLib;
using HeathenEngineering.DEMO;
using HeathenEngineering.SteamworksIntegration;
using HeathenEngineering.SteamworksIntegration.UI;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace OpponentStatistics
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static new ManualLogSource Logger;
        static AccessTools.FieldRef<Settings, LeaderboardManager> _leaderBoardRef =
            AccessTools.FieldRefAccess<Settings, LeaderboardManager>("leaderboardManager");
        static AccessTools.FieldRef<LobbyChatUILogic, LobbyManager> _lobbyManagerRef =
            AccessTools.FieldRefAccess<LobbyChatUILogic, LobbyManager>("lobbyManager");
        static AccessTools.FieldRef<LobbyChatUILogic, GameObject> _theirChatTemplateRef =
            AccessTools.FieldRefAccess<LobbyChatUILogic, GameObject>("theirChatTemplate");
        static AccessTools.FieldRef<LobbyChatUILogic, Transform> _messageRootRef =
            AccessTools.FieldRefAccess<LobbyChatUILogic, Transform>("messageRoot");
        static AccessTools.FieldRef<LobbyChatUILogic, List<IChatMessage>> _chatMessagesRef =
            AccessTools.FieldRefAccess<LobbyChatUILogic, List<IChatMessage>>("chatMessages");
        static bool _requesting = false;

        public void Awake()
        {
            Logger = base.Logger;
            Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} by epenko1337 loaded, good luck!");
            Harmony harmony = new Harmony("com.opponentstatistics.patch");
            harmony.PatchAll();
        }

        public static void FindAndShowOpponent()
        {
            foreach (NetworkObject obj in SteamLobby.Instance.players)
            {
                ClientInstance client = obj.GetComponent<ClientInstance>();
                UserData userData = UserData.Get(client.PlayerSteamID);
                if (!userData.IsMe) ShowOpponentInfo(userData);
            }
        }

        public static void ShowOpponentInfo(UserData userData)
        {
            if (_requesting) return;
            _requesting = true;
            Logger.LogMessage($"Requesting info about {userData.Nickname}");
            LeaderboardManager lb = _leaderBoardRef(Settings.Instance);
            lb.leaderboard.GetEntries([UserData.Get(userData.SteamId)], delegate (LeaderboardEntry[] r, bool e)
            {
                _requesting = false;
                if (!e)
                {
                    Logger.LogMessage("Success!");
                    LobbyChatUILogic uiLogic = GameObject.Find("LobbyController").GetComponent<LobbyChatUILogic>();
                    LeaderboardEntry entry = r[0];
                    LobbyManager lobbyManager = _lobbyManagerRef(uiLogic);
                    LobbyChatMsg msg = new LobbyChatMsg();
                    msg.lobby = lobbyManager.Lobby;
                    msg.type = Steamworks.EChatEntryType.k_EChatEntryTypeChatMsg;
                    msg.sender = userData;
                    msg.data = Encoding.UTF8.GetBytes($"Rank: {entry.Rank}\nScore: {entry.Score}");
                    msg.receivedTime = System.DateTime.Now;

                    GameObject chatTemplate = Object.Instantiate<GameObject>(_theirChatTemplateRef(uiLogic), _messageRootRef(uiLogic));
                    chatTemplate.transform.SetAsLastSibling();
                    IChatMessage messageComponent = chatTemplate.GetComponent<IChatMessage>();
                    if (messageComponent != null)
                    {
                        messageComponent.Initialize(msg);
                        _chatMessagesRef(uiLogic).Add(messageComponent);
                    }
                    uiLogic.StartCoroutine("ForceScrollDown");
                }
                else Logger.LogError("LeaderBoardManager.GetEntries error!");
            });
        }
    }
}
