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

        public void moveRight(GameObject esqueleto)
        {
            // Pega a posição atual do esqueleto
            Vector3 currentPosition = esqueleto.transform.position;

            // Calcula a próxima posição do esqueleto para a direita
            Vector3 nextPosition = currentPosition + new Vector3(1, 0, 0);

            // Verifica se pode se mover para a direita sem bater em uma parede
            if (CanMove(nextPosition - currentPosition))
            {
                esqueleto.transform.position = nextPosition;
            }
        }

        // Move o esqueleto uma unidade para a esquerda
        public void moveLeft(GameObject esqueleto)
        {
            // Pega a posição atual do esqueleto
            Vector3 currentPosition = esqueleto.transform.position;

            // Calcula a próxima posição do esqueleto para a esquerda
            Vector3 nextPosition = currentPosition + new Vector3(-1, 0, 0);

            // Verifica se pode se mover para a esquerda sem bater em uma parede
            if (CanMove(nextPosition - currentPosition))
            {
                esqueleto.transform.position = nextPosition;
            }
        }

        // Move o esqueleto uma unidade para frente
        public void moveUp(GameObject esqueleto)
        {
            // Pega a posição atual do esqueleto
            Vector3 currentPosition = esqueleto.transform.position;

            // Calcula a próxima posição do esqueleto para frente
            Vector3 nextPosition = currentPosition + new Vector3(0, 1, 0);

            // Verifica se pode se mover para frente sem bater em uma parede
            if (CanMove(nextPosition - currentPosition))
            {
                esqueleto.transform.position = nextPosition;
            }
        }

        // Move o esqueleto uma unidade para trás
        public void moveDown(GameObject esqueleto)
        {
            // Pega a posição atual do esqueleto
            Vector3 currentPosition = esqueleto.transform.position;

            // Calcula a próxima posição do esqueleto para trás
            Vector3 nextPosition = currentPosition + new Vector3(0, -1, 0);

            // Verifica se pode se mover para trás sem bater em uma parede
            if (CanMove(nextPosition - currentPosition))
            {
                esqueleto.transform.position = nextPosition;
            }
        }


        private bool CanMove(Vector2 direction)
        {
            Vector3Int gridPosition = groundTilemap.WorldToCell(esqueleto.transform.position + (Vector3)direction);
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

            if (playerDirection.x == 0 && playerDirection.y == 0)
            {
                return;
            }

            else{
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

                if (closestVector == Vector3.right)
                {
                    moveRight(esqueleto);
                }
                else if (closestVector == Vector3.up)
                {
                    moveUp(esqueleto);
                }
                else if (closestVector == Vector3.left)
                {
                    moveLeft(esqueleto);
                }
                else if (closestVector == Vector3.down)
                {
                    moveDown(esqueleto);
                }

            }

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
            // Debug.Log(esqueleto.transform.position);
        }

        void Update()
        {
            //Debug.Log(playerPosition);
            // InvokeRepeating("UpdatePlayerPosition", 0f, 1f); // atualiza a posição do player a cada 1 segundo
            // if (conductor.BeatChanged())
            if (BeatChanged())
            {
                MoveTowardsPlayer();
            }
        }
    }
}







//   private void MoveEsqueleto()
//         {
//             if (conductor.seconds_off_beat() < beat_detection_range)
//             {
//                 Vector3 direction = playerPosition - esqueleto.transform.position;
//                 direction.z = 0f; // mantém o esqueleto na mesma altura que o player
//                 direction.Normalize();
//                 esqueleto.transform.position += direction * speed * Time.deltaTime;
//             }
//         }