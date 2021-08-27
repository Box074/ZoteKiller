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
            if(collision.otherCollider.gameObject.tag.IndexOf("attack",System.StringComparison.OrdinalIgnoreCase) != -1
                || collision.collider.gameObject.tag.IndexOf("attack", System.StringComparison.OrdinalIgnoreCase) != -1)
            {
                Tran();
            }
        }
        void OnCollisionStay2D(Collision2D collision) => OnCollisionEnter2D(collision);

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