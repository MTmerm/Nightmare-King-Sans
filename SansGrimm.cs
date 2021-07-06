using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using GlobalEnums;
using Modding;
using UnityEngine;

namespace SansGrimm
{
    
    public class SansGrimm : Mod
    {
        public override string GetVersion()
        {
            return "1.0";
        }

        public override List<(string, string)> GetPreloadNames()
        {
            return new List<(string, string)>
            {
                ("Grimm_Nightmare", "Grimm Control/Nightmare Grimm Boss")
            };
        }

        public GameObject bd;
        public IEnumerator ReplaceGrimm()
        {
            yield return new WaitWhile(() => HeroController.instance == null);
            Assembly asm = Assembly.GetExecutingAssembly();
            foreach (string res in asm.GetManifestResourceNames())
            {   
                if(!res.EndsWith("nkg.png")) {
                    continue;
                } 
                using (Stream s = asm.GetManifestResourceStream(res))
                {
                        if (s == null) continue;
                        var buffer = new byte[s.Length];
                        s.Read(buffer, 0, buffer.Length);
                        var texture2 = new Texture2D(2, 2);
                        texture2.LoadImage(buffer.ToArray(),true);
                        bd.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = texture2;
                        s.Dispose();
                        
                }
            }
           
        }

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Log("started initialize");
            bd = preloadedObjects["Grimm_Nightmare"]["Grimm Control/Nightmare Grimm Boss"];
            GameManager.instance.StartCoroutine(ReplaceGrimm());
            ModHooks.Instance.LanguageGetHook += Grimm;
            Log("Initialize done");
        }

        private string Grimm(string key, string sheetTitle) 
        {
            //Log($"key: {key}, sheet: {sheetTitle}, text: {Language.Language.GetInternal(key, sheetTitle)}");
            if (key == "NIGHTMARE_GRIMM_MAIN")
            {
                return "Sans";
            }
            return Language.Language.GetInternal(key, sheetTitle);
        }
    }
}
