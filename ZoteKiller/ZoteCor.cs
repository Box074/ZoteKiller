using System.Collections;
using UnityEngine;

namespace ZoteKiller
{
    public class ZoteCor : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(StartAnim());
            gameObject.AddComponent<NonBouncer>().SetActive(true);
        }

        IEnumerator StartAnim()
        {
            Rigidbody2D rig = GetComponent<Rigidbody2D>();
            gameObject.layer = (int)GlobalEnums.PhysLayers.ENEMIES;
            rig.bodyType = RigidbodyType2D.Dynamic;
            bool fr;
            if((fr = HeroController.instance.transform.position.x > transform.position.x)) //Right
            {
                transform.localScale.Set(1, 1, 1);
                rig.velocity = new Vector2(-3, 7.5f);
            }
            else
            {
                transform.localScale.Set(-1, 1, 1);
                rig.velocity = new Vector2(3, 7.5f);
            }
            GameObject fall = transform.Find("Fall Sprite").gameObject;
            fall.SetActive(true);
            //float lt = Time.time;
            yield return new WaitForFixedUpdate();
            while (rig.velocity.y != 0)
            {
                fall.transform.Rotate(0, 0, -2.8f);
                yield return new WaitForSeconds(0.1f);
            }
            fall.SetActive(false);
            GameObject burst = transform.Find("Burst").gameObject;
            burst.SetActive(true);
            Dead();
            yield return new WaitForSeconds(1.5f);
            Destroy(gameObject);
        }

        public void Dead()
        {
            ZoteData zoteData = ZoteKillerMod.zoteData;
            GameObject d = Instantiate(ZoteKillerMod.zoteDead);
            d.transform.position = transform.position - new Vector3(0, 0.88f, 0);
            d.SetActive(true);

            zoteData.KilledZote = true;
            zoteData.DeadScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            zoteData.DeadX = d.transform.position.x;
            zoteData.DeadY = d.transform.position.y;

            if (PlayerData.instance.brettaRescued && PlayerData.instance.zoteDefeated)
            {
                ZoteKillerMod.zoteData.GrilFan = true;
            }
            else
            {
                PlayerData.instance.zoteDefeated = false;
                PlayerData.instance.zoteDead = true;
            }
        }
    }
}