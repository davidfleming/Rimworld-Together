﻿using Verse;

namespace GameClient
{
    //Class that contains all the disconnection functions that the mod uses

    public static class DisconnectionManager
    {
        //Useful disconnection variables

        public enum DCReason
        {
            None,
            SaveQuitToMenu,
            SaveQuitToOS,
            QuitToMenu,
            ConnectionLost
        }

        public static bool isIntentionalDisconnect;
        public static DCReason intentionalDisconnectReason;

        //Executes different actions depending on the disconnection mode

        public static void HandleDisconnect()
        {
            if (isIntentionalDisconnect)
            {
                string reason = "ERROR";

                switch (intentionalDisconnectReason)
                {
                    case DCReason.None:
                        reason = "No reason given.";
                        DisconnectToMenu();
                        break;

                    case DCReason.QuitToMenu:
                        reason = "Quit to menu.";
                        DialogManager.PushNewDialog(new RT_Dialog_OK("Returning to main menu.", delegate { DisconnectToMenu(); }));
                        break;

                    case DCReason.SaveQuitToMenu:
                        reason = "Save and Quit to Menu.";
                        DialogManager.PushNewDialog(new RT_Dialog_OK("Your progress has been saved!", delegate { DisconnectToMenu(); }));
                        break;

                    case DCReason.SaveQuitToOS:
                        reason = "Save and Quit to OS.";
                        DialogManager.PushNewDialog(new RT_Dialog_OK("Your progress has been saved!", delegate { QuitGame(); }));
                        break;
                    case DCReason.ConnectionLost:
                        reason = "Connection to server lost";
                        DialogManager.PushNewDialog(new RT_Dialog_OK("Your progress has been saved!", delegate { DisconnectToMenu(); }));
                        break;
                    default:
                        reason = $"{intentionalDisconnectReason}";
                        DisconnectToMenu();
                        break;
                }

                Logger.Message($"Disconnected from server: {reason}");
            }

            else
            {
                Logger.Message($"Disconnected from server: Connection Lost");
                DialogManager.PushNewDialog(new RT_Dialog_YesNo("Connection lost.  Save game?", delegate { SaveManager.ForceSave(); DisconnectToMenu(); }, delegate { DisconnectToMenu(); }));
            }
        }

        //Kicks the client into the main menu

        public static void DisconnectToMenu()
        {
            Network.state = NetworkState.Disconnected;
            OnlineChatManager.CleanChat();
            ClientValues.CleanValues();
            ServerValues.CleanValues();
            Network.Cleanup();

            DialogManager.PopWaitDialog();

            if (Current.ProgramState != ProgramState.Entry)
            {
                LongEventHandler.QueueLongEvent(delegate { }, 
                    "Entry", "", doAsynchronously: false, null);
            }
        }

        //Kicks the client into closing the game

        public static void QuitGame() { Root.Shutdown(); }

        //Kicks the client into restarting the game

        public static void RestartGame() { GenCommandLine.Restart(); }
    }
}
