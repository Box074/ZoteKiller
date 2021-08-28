using System.Collections;
using System.Linq;
using UnityEngine;

namespace ZoteKiller
{
    public class ZoteScript : MonoBehaviour
    {

        void FixedUpdate()
        {
            Collider2D[] r = Physics2D.OverlapPointAll(transform.position);
            if(
                r.Any(
                    x => x.gameObject.layer == (int)GlobalEnums.PhysLayers.HERO_ATTACK ||
                        x.gameObject.tag == "Nail Attack"
                )
                )
            {
                
                Tran();
            }
        }

        public void Tran()
        {
            GameObject boss = Instantiate(ZoteKillerMod.zoteBoss);
            boss.transform.position = transform.position;
            boss.SetActive(true);
            boss.AddComponent<ZoteBoss>();
            Destroy(gameObject);
        }
    }
}