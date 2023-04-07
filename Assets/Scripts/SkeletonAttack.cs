using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace SkeletonNamespace {
    public class SkeletonAttack : MonoBehaviour
    {
        public GameObject esqueleto;

        [SerializeField]
        private Conductor conductor;

        public float beat_detection_range = 0.15f;
        public float speed = 3f;

        private Vector3 playerPosition;

        private int lastPositionInBeats;

        [SerializeField]
        private Tilemap groundTilemap;

        [SerializeField]
        private Tilemap wallTilemap;

        private void move(GameObject esqueleto, Vector3 direction)
        {
            // Pega a posição atual do esqueleto
            Vector3 currentPosition = esqueleto.transform.position;

            // Calcula a próxima posição do esqueleto para a direita
            Vector3 nextPosition = currentPosition + direction;

            // Verifica se pode se mover para a direita sem bater em uma parede
            if (CanMove(nextPosition - currentPosition))
            {
                esqueleto.transform.position = nextPosition;
            }
        }



        private bool CanMove(Vector2 direction)
        {
            Vector3 newPosition = esqueleto.transform.position + (Vector3)direction;
            Vector2 boxSize = esqueleto.GetComponent<BoxCollider2D>().size;

            // Verifica se há algum objeto com BoxCollider2D na próxima posição
            Collider2D hit = Physics2D.OverlapBox(newPosition, boxSize, 0f);

            if (hit != null && (hit.gameObject.CompareTag("Enemy") || hit.gameObject.CompareTag("Player")))
            {
                return false;
            }

            Vector3Int gridPosition = groundTilemap.WorldToCell(newPosition);
            if (!groundTilemap.HasTile(gridPosition) || wallTilemap.HasTile(gridPosition))
                return false;
            return true;
        }

        void MoveTowardsPlayer()
        {
            Vector3 skeletonPosition = esqueleto.transform.position;
            skeletonPosition.y -= 0.25f;
            Vector3 playerDirection = (playerPosition - skeletonPosition).normalized;
            Vector3 closestVector = Vector3.right;
            float smallestAngle = Mathf.Abs(Vector3.Angle(playerDirection, closestVector));


            if (smallestAngle > Mathf.Abs(Vector3.Angle(playerDirection, Vector3.up)))
            {
                smallestAngle = Mathf.Abs(Vector3.Angle(playerDirection, Vector3.up));
                closestVector = Vector3.up;
            }

            if (smallestAngle > Mathf.Abs(Vector3.Angle(playerDirection, Vector3.left)))
            {
                smallestAngle = Mathf.Abs(Vector3.Angle(playerDirection, Vector3.left));
                closestVector = Vector3.left;
            }

            if (smallestAngle > Mathf.Abs(Vector3.Angle(playerDirection, Vector3.down)))
            {
                closestVector = Vector3.down;
            }

            move(esqueleto, closestVector);


        }


        private void UpdatePlayerPosition()
        {
            playerPosition = GameObject.Find("Player").transform.position;
        }

        public bool BeatChanged(){
            if (lastPositionInBeats == conductor.songPositionInBeats)
                return false;
            lastPositionInBeats = conductor.songPositionInBeats;
            return true;
        }        


        void Start()
        {
            lastPositionInBeats = conductor.songPositionInBeats;
            InvokeRepeating("UpdatePlayerPosition", 0f, 1f); // atualiza a posição do player a cada 1 segundo
        }

        void Update()
        {

            if (BeatChanged())
            {
                MoveTowardsPlayer();
            }
        }
    }
}
