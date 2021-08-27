using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modding;
using UnityEngine;
using UnityEngine.SceneManagement;
using ModCommon;
using ModCommon.Util;

namespace ZoteKiller
{
    public class ZoteKillerMod : Mod , ILocalSettings<ZoteData>
    {
        public static GameObject zoteBoss = null;
        public static GameObject zoteCor = null;
        public static GameObject zoteDead = null;
        public override List<(string, string)> GetPreloadNames()
        {
            return new List<(string, string)>
            {
                ("GG_Mighty_Zote","Battle Control/First Zote/Zote Boss"),
                ("GG_Mighty_Zote","Corpse Zote Ordeal First"),
                ("Fungus1_20_v02","Zote Death")
            };
        }
        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            #region ZoteBoss
            zoteBoss = preloadedObjects["GG_Mighty_Zote"]["Battle Control/First Zote/Zote Boss"];
            zoteBoss.SetActive(false);
            PlayMakerFSM zbC = zoteBoss.LocateMyFSM("Control");
            zbC.ChangeTransition("Roar?", "FINISHED", "Idle");
            zbC.ChangeTransition("Init", "FINISHED", "Trip?");

            zoteCor = preloadedObjects["GG_Mighty_Zote"]["Corpse Zote Ordeal First"];
            UnityEngine.Object.Destroy(zoteCor.transform.Find("white_solid"));
            PlayMakerFSM czoc = zoteCor.LocateMyFSM("Control");
            czoc.ChangeTransition("Burst", "FINISHED", "End");
            #endregion
            #region Zote Dead
            zoteDead = preloadedObjects["Fungus1_20_v02"]["Zote Death"];
            zoteDead.SetActive(false);
            foreach (var v in zoteDead.GetComponents<PlayMakerFSM>()) UnityEngine.Object.Destroy(v);
            #endregion

            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        }
        private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
        {
            if (zoteData == null) return;
            ModHooks.HeroUpdateHook -= FindZoteConv;
            if (zoteData.KilledZote)
            {
                if (arg1.name == "Fungus1_20_v02")
                {
                    arg1.FindGameObject("Zote Dead")?.SetActive(false);
                }
                if (arg1.name == "Town")
                {
                    if (zoteData.GrilFan)
                    {
                        PlayerData.instance.zoteDead = false;
                        PlayerData.instance.zoteDefeated = true;
                        arg1.FindGameObject("Zote Final")?.SetActive(false);
                    }
                    else
                    {
                        PlayerData.instance.zoteDead = true;
                        PlayerData.instance.zoteDefeated = false;
                    }
                }
                else
                {
                    PlayerData.instance.zoteDead = true;
                    PlayerData.instance.zoteTrappedDeepnest = false;
                    PlayerData.instance.zoteLeftCity = false;
                    PlayerData.instance.zoteSpokenCity = false;
                    PlayerData.instance.zoteSpokenColosseum = false;
                }
                if (arg1.name == zoteData.DeadScene)
                {

                    GameObject dead = UnityEngine.Object.Instantiate(zoteDead);
                    dead.transform.position = new Vector2(zoteData.DeadX, zoteData.DeadY);
                    dead.SetActive(true);
                }
            }
            else
            {
                if (arg1.name == "Fungus1_20_v02")
                {
                    ModHooks.HeroUpdateHook += FindZoteConv;
                }
                else if (arg1.name == "Town")
                {
                    arg1.FindGameObject("Zote Dead")?.AddComponent<ZoteScript>();
                    arg1.FindGameObject("Zote Town")?.AddComponent<ZoteScript>();
                }
                else
                {
                    void FZote(GameObject root)
                    {
                        if(root.name.IndexOf("zote",StringComparison.OrdinalIgnoreCase)!=-1 && root.LocateMyFSM("npc_control") != null)
                        {
                            root.AddComponent<ZoteScript>();
                        }
                        for(int i = 0; i < root.transform.childCount; i++)
                        {
                            FZote(root.transform.GetChild(i).gameObject);
                        }
                    }
                    foreach (var v in arg1.GetRootGameObjects()) FZote(v);
                }
            }
        }

        private void FindZoteConv()
        {
            GameObject z = GameObject.Find("Zote Buzzer Convo(Clone)");
            if (z != null)
            {
                if (z.GetComponent<ZoteScript>() == null) z.AddComponent<ZoteScript>();
            }
        }

        public static ZoteData zoteData = new ZoteData();
        public void OnLoadLocal(ZoteData s)
        {
            zoteData = s;
        }

        public ZoteData OnSaveLocal()
        {
            ZoteData r = zoteData;
            zoteData = null;
            return r;
        }
    }
}
