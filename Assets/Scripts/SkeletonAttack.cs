using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkeletonNamespace {
    public class SkeletonAttack : MonoBehaviour
    {
        public GameObject esqueleto;

        [SerializeField]
        private Conductor conductor;

        public float beat_detection_range = 0.15f;
        public float speed = 3f;

        private PlayerMovement controls;

        private Vector3 playerPosition;

        private void MoveEsqueleto()
        {
            if (conductor.seconds_off_beat() < beat_detection_range)
            {
                Vector3 direction = playerPosition - esqueleto.transform.position;
                direction.z = 0f; // mantém o esqueleto na mesma altura que o player
                direction.Normalize();
                esqueleto.transform.position += direction * speed * Time.deltaTime;
            }
        }

        private void UpdatePlayerPosition()
        {
            playerPosition = GameObject.Find("Player").transform.position;
        }

        void Start()
        {
            InvokeRepeating("UpdatePlayerPosition", 0f, 1f); // atualiza a posição do player a cada 1 segundo
        }

        void Update()
        {
            MoveEsqueleto();
        }
    }
}





