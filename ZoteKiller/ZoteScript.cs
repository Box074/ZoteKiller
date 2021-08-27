using System.Collections;
using UnityEngine;

namespace ZoteKiller
{
    public class ZoteScript : MonoBehaviour
    {
        void Awake()
        {
            gameObject.layer = (int)GlobalEnums.PhysLayers.ENEMIES;
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.layer == (int)GlobalEnums.PhysLayers.HERO_ATTACK)
            {
                GameObject boss = Instantiate(ZoteKillerMod.zoteBoss);
                boss.transform.position = transform.position;
                boss.SetActive(true);
                boss.AddComponent<ZoteBoss>();
                Destroy(gameObject);
            }
        }
    }
}