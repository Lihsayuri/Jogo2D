using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkeletonNamespace {
   // O código do seu script virá aqui
   public class SkeletonAttack : MonoBehaviour
    {
        public GameObject esqueleto;

        [SerializeField]
        private Conductor conductor;

        public float beat_detection_range = 0.15f;

        private PlayerMovement controls;


        // Start is called before the first frame update

        public void MoveEsqueleto(Vector3 playerPosition)
        {
            if (conductor.seconds_off_beat() < beat_detection_range)
            {
                Vector3 direction = playerPosition - esqueleto.transform.position;
                direction.Normalize();
                esqueleto.transform.position += direction * Time.deltaTime;
            }
        }


        private void Move(){
            MoveEsqueleto(GameObject.Find("Player").transform.position);
            
        }

        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }

}
