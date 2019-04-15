using System;
using System.Runtime.InteropServices;
using System.Threading;
using WindowsInput;
using WindowsInput.Native;

namespace TraderForPoe.Classes
{
    internal static class Poe
    {
        /// <summary>
        /// Retrieves a handle to the top-level window whose class name and window name match the specified strings.
        /// </summary>
        /// <param name="lpClassName"></param>
        /// <param name="lpWindowName"></param>
        /// <returns></returns>
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// Retrieves a handle to the foreground window (the window with which the user is currently working). The system assigns a slightly higher priority to the thread that creates the foreground window than it does to other threads.
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// Brings the thread that created the specified window into the foreground and activates the window
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("USER32.DLL")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

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
        /// Verify that POE is a running process.
        /// </summary>
        /// <returns>True if PoE is running</returns>
        public static bool IsForegroundWindow()
        {
            if (GetForegroundWindow() == GetHandle())
            {
                return true;
            }
            else
            {
                return false;
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
        public static void SendCommand(string arg, bool send = true)
        {
            if (IsRunning())
            {
                InputSimulator iSim = new InputSimulator();

                //Make POE the foreground application and send input
                SetForegroundWindow(GetHandle());

                Thread.Sleep(50);

                // Need to press ALT because the SetForegroundWindow sometimes does not work
                iSim.Keyboard.KeyPress(VirtualKeyCode.MENU);

                // Open chat
                iSim.Keyboard.KeyPress(VirtualKeyCode.RETURN);

                // Send the input
                iSim.Keyboard.TextEntry(arg);

                if (send)
                {
                    // Send RETURN
                    iSim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
                }
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
        /// Place a whisper in the chat window but do not send
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        public static void WhisperPlayerNoSend(string character, string message)
        {
            SendCommand("@" + character + " " + message, false);
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

        /// <summary>
        /// Make POE the foreground application and send input
        /// </summary>
        public static void ActivatePoeWindow()
        {
            SetForegroundWindow(GetHandle());
        }
    }
}