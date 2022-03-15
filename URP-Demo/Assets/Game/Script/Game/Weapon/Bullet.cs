using UnityEngine;

namespace Demo.Game.Weapon
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        private float timeToDisappear = 5.0f;
        private float livingTime;


        public float moveSpeed { private get; set; }
        public Vector3 direction { private get; set; }

        public void Update()
        {
            this.livingTime += Time.deltaTime;
            if (this.livingTime >= this.timeToDisappear)
            {
                this.gameObject.SetActive(false);
            }

            this.transform.Translate(this.direction * moveSpeed * Time.deltaTime);
        }
    }
}