using System;
using System.IO;
using Photon.Realtime;
using UnityEditor;
using UnityEngine;

namespace Photon.Voice.Unity.Editor
{
    public static class PhotonVoiceEditorUtils
    {
        /// <summary>True if the ChatClient of the Photon Chat API is available. If so, the editor may (e.g.) show additional options in settings.</summary>
        public static bool HasChat
        {
            get
            {
                return Type.GetType("Photon.Chat.ChatClient, Assembly-CSharp") != null || Type.GetType("Photon.Chat.ChatClient, Assembly-CSharp-firstpass") != null || Type.GetType("Photon.Chat.ChatClient, PhotonChat") != null;
            }
        }

        /// <summary>True if the PhotonNetwork of the PUN is available. If so, the editor may (e.g.) show additional options in settings.</summary>
        public static bool HasPun
        {
            get
            {
                return Type.GetType("Photon.Pun.PhotonNetwork, Assembly-CSharp") != null || Type.GetType("Photon.Pun.PhotonNetwork, Assembly-CSharp-firstpass") != null || Type.GetType("Photon.Pun.PhotonNetwork, PhotonUnityNetworking") != null;
            }
        }
        
        [MenuItem("Window/Photon Voice/Remove PUN", true, 1)]
        private static bool RemovePunValidate()
        {
            #if PHOTON_UNITY_NETWORKING
            return true;
            #else
            return HasPun;
            #endif
        }

        [MenuItem("Window/Photon Voice/Remove PUN", false, 1)]
        private static void RemovePun()
        {
            DeleteDirectory("Assets/Photon/PhotonVoice/Demos/DemoProximityVoiceChat");
            DeleteDirectory("Assets/Photon/PhotonVoice/Demos/DemoVoicePun");
            DeleteDirectory("Assets/Photon/PhotonVoice/Code/PUN");
            DeleteDirectory("Assets/Photon/PhotonUnityNetworking");
            PhotonEditorUtils.CleanUpPunDefineSymbols();
        }

        [MenuItem("Window/Photon Voice/Remove Photon Chat", true, 2)]
        private static bool RemovePhotonChatValidate()
        {
            return HasChat;
        }

        [MenuItem("Window/Photon Voice/Remove Photon Chat", false, 2)]
        private static void RemovePhotonChat()
        {
            DeleteDirectory("Assets/Photon/PhotonChat");
        }

        [MenuItem("Window/Photon Voice/Leave a review", false, 3)]
        private static void OpenAssetStorePage()
        {
            Application.OpenURL("https://assetstore.unity.com/packages/tools/audio/photon-voice-2-130518");
        }

        public static void DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                if (!FileUtil.DeleteFileOrDirectory(path))
                {
                    Debug.LogWarningFormat("Directory \"{0}\" not deleted.", path);
                }
                DeleteFile(string.Concat(path, ".meta"));
            }
            else
            {
                Debug.LogWarningFormat("Directory \"{0}\" does not exist.", path);
            }
        }

        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                if (!FileUtil.DeleteFileOrDirectory(path))
                {
                    Debug.LogWarningFormat("File \"{0}\" not deleted.", path);
                }
            }
            else
            {
                Debug.LogWarningFormat("File \"{0}\" does not exist.", path);
            }
        }
        
        public static bool IsInTheSceneInPlayMode(GameObject go)
        {
            return Application.isPlaying && !PhotonEditorUtils.IsPrefab(go);
        }

        public static void GetPhotonVoiceVersionsFromChangeLog(out string photonVoiceVersion, out string punChangelogVersion, out string photonVoiceApiVersion)
        {
            string filePath = "Assets\\Photon\\PhotonVoice\\changes-voice.txt";
            photonVoiceVersion = null;
            punChangelogVersion = null;
            photonVoiceApiVersion = null;
            try
            {
                using (StreamReader file = new StreamReader(filePath))
                {
                    while (!file.EndOfStream && (string.IsNullOrEmpty(photonVoiceVersion) || string.IsNullOrEmpty(punChangelogVersion) || string.IsNullOrEmpty(photonVoiceApiVersion)))
                    {
                        string line = file.ReadLine();
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            line = line.Trim();
                            if (line.StartsWith("v2."))
                            {
                                if (!string.IsNullOrEmpty(photonVoiceVersion))
                                {
                                    break;
                                }
                                photonVoiceVersion = line.TrimStart('v');
                                continue;
                            }
                            string[] parts = line.Split(null);
                            if (line.StartsWith("PUN2: ") && parts.Length > 1)
                            {
                                punChangelogVersion = parts[1];
                                continue;
                            }
                            if (line.StartsWith("PhotonVoiceApi: ") && parts.Length > 2)
                            {
                                photonVoiceApiVersion = string.Format("rev. {0}", parts[2]);
                            }
                        }
                    }
                }
            }
            catch (IOException e)
            {
                Debug.LogErrorFormat("There was an error reading the file \"{0}\": ", filePath);
                Debug.LogError(e.Message);
            }
            if (string.IsNullOrEmpty(photonVoiceVersion))
            {
                Debug.LogError("There was an error retrieving Photon Voice version from changelog.");
            }
            if (string.IsNullOrEmpty(punChangelogVersion))
            {
                Debug.LogError("There was an error retrieving PUN2 version from changelog.");
            }
            if (string.IsNullOrEmpty(photonVoiceApiVersion))
            {
                Debug.LogError("There was an error retrieving Photon Voice API version from changelog.");
            }
        }
    }
}