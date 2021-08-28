using System.Collections;
using UnityEngine;
using ModCommon.Util;

namespace ZoteKiller
{
    public class ZoteBoss : MonoBehaviour
    {
        public static ZoteKillerMod ZoteKillerMod => (ZoteKillerMod)Modding.ModHooks.GetMod("ZoteKillerMod");
        void Awake()
        {
            DamageHero dh = GetComponent<DamageHero>();
            if (dh != null) Destroy(dh);
            foreach (var v in GetComponentsInChildren<DamageHero>()) Destroy(v);

            HealthManager hm = GetComponent<HealthManager>();
            hm.IsInvincible = false;
            hm.isDead = false;
            hm.OnDeath += Hm_OnDeath;
        }

        private void Hm_OnDeath()
        {
            GameObject czo = Instantiate(ZoteKillerMod.zoteCor);
            czo.transform.position = transform.position;
            czo.SetActive(true);
            Destroy(gameObject);
        }
    }
}