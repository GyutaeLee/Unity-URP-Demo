using UnityEngine;

namespace Demo.Game.Weapon
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        private float lifeTime = 5.0f;
        private float liveTime;


        public float moveSpeed { private get; set; }
        public Vector3 direction { private get; set; }

        public void Update()
        {
            this.liveTime += Time.deltaTime;
            if (this.liveTime >= this.lifeTime)
            {
                this.gameObject.SetActive(false);
            }

            this.transform.Translate(this.direction * moveSpeed * Time.deltaTime);
        }
    }
}