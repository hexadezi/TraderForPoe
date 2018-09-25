using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;

namespace TraderForPoe.Classes
{
    static class Poe
    {
        /// <summary>
        /// Retrieves a handle to the top-level window whose class name and window name match the specified strings.
        /// </summary>
        /// <param name="lpClassName"></param>
        /// <param name="lpWindowName"></param>
        /// <returns></returns>
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// Brings the thread that created the specified window into the foreground and activates the window
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("USER32.DLL")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// Verify that POE is a running process.
        /// </summary>
        /// <returns>True if PoE is running</returns>
        public static bool IsRunning()
        {
            if (GetHandle() == IntPtr.Zero)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Returns the current handle of PoE
        /// </summary>
        /// <returns>Handle as IntPtr</returns>
        public static IntPtr GetHandle()
        {
            return FindWindow("POEWindowClass", "Path of Exile");
        }

        /// <summary>
        /// Send a string to the chat window of poe.
        /// </summary>
        /// <param name="arg">Chat message to send</param>
        public static void SendCommand(string arg)
        {
            if (IsRunning())
            {
                InputSimulator iSim = new InputSimulator();

                // Make POE the foreground application and send input
                SetForegroundWindow(GetHandle());

                //Thread.Sleep(100);

                // Need to press ALT because the SetForegroundWindow sometimes does not work
                iSim.Keyboard.KeyPress(VirtualKeyCode.MENU);

                // Open chat
                iSim.Keyboard.KeyPress(VirtualKeyCode.RETURN);

                // Send the input
                iSim.Keyboard.TextEntry(arg);

                // Send RETURN
                iSim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
            }
            else
            {
                System.Windows.MessageBox.Show("Path of Exile is not running.", "Error - Not running", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Send a whisper to a player.
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        public static void WhisperPlayer(string character, string message)
        {
            SendCommand("@" + character + " " + message);
        }

        /// <summary>
        /// Sends a party invite to character
        /// </summary>
        /// <param name="character"></param>
        public static void InvitePlayer(string character)
        {
            SendCommand("/invite " + character);
        }

        /// <summary>
        /// Kicks character from the party.
        /// </summary>
        /// <param name="character"></param>
        public static void KickPlayer(string character)
        {
            SendCommand("/kick " + character);
        }

        /// <summary>
        /// Displays a character's level, class, league, and whether he is online.
        /// </summary>
        /// <param name="character"></param>
        public static void WhoisPlayer(string character)
        {
            SendCommand("/whois " + character);
        }

        /// <summary>
        /// Sends you to character's hideout. Only useable while in town.
        /// </summary>
        /// <param name="character"></param>
        public static void VisitCharacterHideout(string character)
        {
            SendCommand("/hideout " + character);
        }

        /// <summary>
        /// Sends you to your hideout. Only useable while in town.
        /// </summary>
        public static void VisitOwnHideout()
        {
            SendCommand("/hideout");
        }
    }
}