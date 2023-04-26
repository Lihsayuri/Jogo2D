using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public GameObject player;

    public Animator animator;

    private PlayerMovement controls;

    [SerializeField]
    private Sprite [] _liveSprites;

    [SerializeField]
    private Image _liveImage;

    [SerializeField]
    private Tilemap groundTilemap;

    [SerializeField]
    private Tilemap wallTilemap;
    
    [SerializeField]
    private Conductor conductor;

    private int raycast_mask;

    public float beat_detection_range = 0.15f;

    public int vida = 5;

    [SerializeField]
    private bool hasKey = false;

    [SerializeField]
    private GameObject gameOverPanel;

    [SerializeField]
    private GameObject bossObject;

    [SerializeField]
    private GameObject winPanel;

    [SerializeField]
    private Image metronome;

    
    // private List<string> weapons = new List<string> ();
    
    private Dictionary<string, int> weaponDamage = new Dictionary<string, int> ();

    [SerializeField] 
    private GameObject NewWeapon;

    [SerializeField]
    private GameObject WeaponSelected;


    [SerializeField]
    private GameObject WeaponSelectedPopUp;

    // INFOS DO GAUGE DE ESPECIAL
    public float specialAmount; // Quantidade atual de especial do personagem.
    public float specialIncrease = 0.1f; // Quantidade de especial adicionada a cada batida.
    public float maxSpecial = 1f; // Quantidade máxima de especial que o personagem pode ter.
    public Image specialBar; // Referência à imagem da gauge de especial.

    public Image fullSpecialBarImage;
    private int dmg_playerAttack;

    private int tres_ataques_especiais;

    private bool onSpecialAttack = false;

    [SerializeField]
    private Light2D spriteLight;

    public Image fadeOut;

    public AudioSource audioSource; // Referência ao componente AudioSource
    public AudioClip hitSound; // Som que será tocado quando o personagem receber um hit

    public AudioClip lifePotion;

    public AudioClip openChest;

    public AudioClip foundWeapon;

    public AudioClip foundKey;


private void Awake()
    {
        controls = new PlayerMovement();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }


    private void PreencheDicionario(){
        weaponDamage.Add("SimpleSword", 1);
        weaponDamage.Add("Knife", 1);
        weaponDamage.Add("SimpleAxe", 1);
        weaponDamage.Add("DoubleAxe", 2);
        weaponDamage.Add("Hammer", 2);
        weaponDamage.Add("PersianSaber", 3);
        weaponDamage.Add("Mace", 3);
        weaponDamage.Add("FireSword", 4);
    }

    

    // Start is called before the first frame update
    void Start()
    {
        PreencheDicionario();
        conductor.enabled = true;
        fullSpecialBarImage.enabled = true;
        fadeOut.enabled = false;
        specialBar.fillAmount = 0;

        if (PlayerManager.Instance.trocaCena == true && PlayerManager.Instance.level == 2){
                Vector3 spawnPosition = new Vector3(33.5f, 0.5f, 0f);
                player.transform.position = spawnPosition;
                PlayerManager.Instance.trocaCena = false;
        }
        if (PlayerManager.Instance.trocaCena == true && PlayerManager.Instance.level == 3){
                Vector3 spawnPosition = new Vector3(0.5f, 5.5f, 0f);
                player.transform.position = spawnPosition;
                PlayerManager.Instance.trocaCena = false;
        }
        
        if (PlayerManager.Instance != null && PlayerManager.Instance.weapons.Count != 0)
        {
            SetWeaponImage weaponUI = WeaponSelected.GetComponent<SetWeaponImage>();
            weaponUI.SetImage(PlayerManager.Instance.weapons[PlayerManager.Instance.weapons.Count-1]);
            SetWeaponImage WeaponPopUpUI = WeaponSelectedPopUp.GetComponent<SetWeaponImage>();
            WeaponPopUpUI.SetImage(PlayerManager.Instance.weapons[PlayerManager.Instance.weapons.Count-1]);
        }
        else
        {
            WeaponSelected.SetActive(false);
            WeaponSelectedPopUp.SetActive(false);
        }


        controls.Main.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());
        gameOverPanel.SetActive(false);
        metronome.enabled = true;
        winPanel.SetActive(false);
        
    }

    void Update(){
        specialBar.fillAmount = specialAmount / maxSpecial;

        if (PlayerManager.Instance.level == 3){
            if (bossObject != null) {
                EnemyBaseClass bossScript = bossObject.GetComponent<EnemyBaseClass>();
                        // Usa o componente do script para acessar as variáveis ​​do script
                if (bossScript.ganhou == true) {
                    animator.SetBool("Won", true);
                    WaitForSeconds wait = new WaitForSeconds(1.5f);
                    fadeOut.enabled = true;
                    FadeOut FadeoutScript = fadeOut.GetComponent<FadeOut>() as FadeOut;
                    FadeoutScript.StartCoroutine(FadeoutScript.FadeOutCoroutineWin());
                    metronome.enabled = false;
                    gameOverPanel.SetActive(false);
                    WeaponSelected.SetActive(false);
                    WeaponSelectedPopUp.SetActive(false);
                    _liveImage.enabled = false;
                    fullSpecialBarImage.enabled = false;
                    specialBar.enabled = false;
                    return;
                }
            }
        }
    }

    void LateUpdate()
    {
        if (PlayerManager.Instance != null && PlayerManager.Instance.weapons.Count != 0)
        {
            SetWeaponImage weaponUI = WeaponSelected.GetComponent<SetWeaponImage>();
            weaponUI.SetImage(PlayerManager.Instance.weapons[PlayerManager.Instance.weapons.Count-1]);
            SetWeaponImage WeaponPopUpUI = WeaponSelectedPopUp.GetComponent<SetWeaponImage>();
            WeaponPopUpUI.SetImage(PlayerManager.Instance.weapons[PlayerManager.Instance.weapons.Count-1]);
        }
        if (conductor.GetComponent<Conductor>().musicSource.isPlaying == false){
            if (PlayerManager.Instance.level == 1){
                PlayerManager.Instance.level+=1;
                SceneManager.LoadScene(PlayerManager.Instance.level);
                PlayerManager.Instance.trocaCena = true;
                return;
            }
            if (PlayerManager.Instance.level == 2){
                PlayerManager.Instance.level+=1;
                SceneManager.LoadScene(PlayerManager.Instance.level);
                PlayerManager.Instance.trocaCena = true;
                return;
            }
        } 

    }

    private void Move(Vector2 direction)
    {
        spriteLight.intensity = 0;
        if (conductor.seconds_off_beat() < beat_detection_range) {
            spriteLight.intensity = 1;      
            specialAmount += specialIncrease;
            Debug.Log("specialAmount: " + specialAmount);

            // Verifica se a quantidade de especial do personagem é maior que a quantidade máxima permitida.
            if (specialAmount >= 0.95 && specialAmount <= 1.05f)
            {
                Debug.Log("Ataque Especial");
                // Define a quantidade de especial do personagem como a quantidade máxima permitida.
                specialAmount = maxSpecial;
                onSpecialAttack = true;
                tres_ataques_especiais = 3;
            }
            if (specialAmount > maxSpecial)
            {
                // Define a quantidade de especial do personagem como a quantidade máxima permitida.
                specialAmount = maxSpecial;
            }

            if (CanMove(direction))
                transform.position += (Vector3)direction;
            
        } 

    }

    public void TakeDamage(int damage)
    {
        vida -= damage;
        audioSource.PlayOneShot(hitSound);
        if(vida > 0)
            _liveImage.sprite = _liveSprites[vida];
        else
        {
            _liveImage.sprite = _liveSprites[0];
            animator.SetBool("isDying", true);
            WaitForSeconds wait = new WaitForSeconds(1.5f);
            fadeOut.enabled = true;
            FadeOut FadeoutScript = fadeOut.GetComponent<FadeOut>() as FadeOut;
            FadeoutScript.StartCoroutine(FadeoutScript.FadeOutCoroutine());

            metronome.enabled = false;
            specialBar.enabled = false;
            fullSpecialBarImage.enabled = false;
            WeaponSelected.SetActive(false);
            WeaponSelectedPopUp.SetActive(false);
            _liveImage.enabled = false;
            conductor.enabled = false;
            return; // Adicionado para interromper a execução do método
        }
    }

    public void confereWeaponSelected(){
        if (PlayerManager.Instance.weapons.Count == 0){
            WeaponSelected.SetActive(false);
            WeaponSelectedPopUp.SetActive(false);
        } else {
            WeaponSelected.SetActive(true);
            WeaponSelectedPopUp.SetActive(true);
        }
    }

    public void SorteiaItem(){
        System.Random rand = new System.Random();

        List<string> weaponList = new List<string>(weaponDamage.Keys);
        int randomIndex = rand.Next(weaponList.Count);
        string randomWeapon = weaponList[randomIndex];
        PlayerManager.Instance.weapons.Add(randomWeapon);
        SetWeaponImage weaponUI = WeaponSelected.GetComponent<SetWeaponImage>();
        SetWeaponImage weaponUIPopUp = WeaponSelectedPopUp.GetComponent<SetWeaponImage>();
        weaponUI.SetImage(randomWeapon);
        weaponUIPopUp.SetImage(randomWeapon);
        confereWeaponSelected();
        PopUpController popUpControllerScript = NewWeapon.GetComponent<PopUpController>() as PopUpController;
        popUpControllerScript.ShowPopup(randomWeapon, weaponDamage);
    }


    public int selecionaUltimaArma(){
        if (PlayerManager.Instance.weapons.Count == 0){
            return 0;
        } else {
            string lastWeapon = PlayerManager.Instance.weapons[PlayerManager.Instance.weapons.Count - 1];
            return weaponDamage[lastWeapon];
        }
    }



    private bool CanMove(Vector2 direction)
    {
        Vector3 newPosition = player.transform.position + (Vector3)direction;
        Vector2 boxSize = player.GetComponent<BoxCollider2D>().size;

        // Verifica se há algum objeto com BoxCollider2D na próxima posição
        Collider2D hit = Physics2D.OverlapBox(newPosition, boxSize, 0f);

        if (hit != null && ((hit.gameObject.CompareTag("Enemy")) || (hit.gameObject.CompareTag("Boss"))))
        {
            EnemyBaseClass enemyScript = hit.gameObject.GetComponent<EnemyBaseClass>() as EnemyBaseClass;
            if (enemyScript != null)
            {
                dmg_playerAttack = selecionaUltimaArma();
                if (onSpecialAttack)
                {
                    Debug.Log("ESPECIAL");
                    if (tres_ataques_especiais > 0){
                        dmg_playerAttack += 1;
                        tres_ataques_especiais -= 1;
                        Debug.Log("Ataque especial " + tres_ataques_especiais);
                        if (tres_ataques_especiais == 0){
                            specialBar.fillAmount = 0;
                            onSpecialAttack = false;
                            specialAmount = 0;
                        }
                    }
                    else {
                        Debug.Log("Acabou os ataques especiais");
                        specialBar.fillAmount = 0;
                        onSpecialAttack = false;
                        specialAmount = 0;
                    }
                } 
                
                enemyScript.TakeDamage(dmg_playerAttack);
                return false;
            }

            return false;
        }

        if (hit != null && (hit.gameObject.CompareTag("Door")))
        {
            DoorScript doorScript = hit.gameObject.GetComponent<DoorScript>() as DoorScript;
            if (doorScript.locked && hasKey)
            {
                hasKey = false;
                doorScript.locked = false;
                doorScript.OpenDoor();
            }
            else if (!doorScript.locked)
            {
                doorScript.OpenDoor();
            }
            return false;
        }

        if (hit != null && (hit.gameObject.CompareTag("Key")))
        {
            audioSource.PlayOneShot(foundKey);
            hit.gameObject.SetActive(false);
            hasKey = true;
            return true;
        }

        if (hit != null && (hit.gameObject.CompareTag("SimpleSword")))
        {
            hit.gameObject.SetActive(false);
            audioSource.PlayOneShot(foundWeapon);
            PlayerManager.Instance.weapons.Add("SimpleSword");
            PopUpController popUpControllerScript = NewWeapon.GetComponent<PopUpController>() as PopUpController;
            popUpControllerScript.ShowPopup("SimpleSword", weaponDamage);
            SetWeaponImage weaponUI = WeaponSelected.GetComponent<SetWeaponImage>();
            weaponUI.SetImage("SimpleSword");
            SetWeaponImage weaponUIPopUp = WeaponSelectedPopUp.GetComponent<SetWeaponImage>();
            weaponUIPopUp.SetImage("SimpleSword");
            confereWeaponSelected();
            return true;
        }

        if (hit != null && (hit.gameObject.CompareTag("Knife")))
        {
            hit.gameObject.SetActive(false);
            audioSource.PlayOneShot(foundWeapon);
            PlayerManager.Instance.weapons.Add("Knife");
            PopUpController popUpControllerScript = NewWeapon.GetComponent<PopUpController>() as PopUpController;
            popUpControllerScript.ShowPopup("Knife", weaponDamage);
            confereWeaponSelected();
            SetWeaponImage weaponUI = WeaponSelected.GetComponent<SetWeaponImage>();
            weaponUI.SetImage("Knife");
            SetWeaponImage weaponUIPopUp = WeaponSelectedPopUp.GetComponent<SetWeaponImage>();
            weaponUIPopUp.SetImage("Knife");
            return true;
        }

        if (hit != null && (hit.gameObject.CompareTag("SimpleAxe")))
        {
            hit.gameObject.SetActive(false);
            audioSource.PlayOneShot(foundWeapon);
            PlayerManager.Instance.weapons.Add("SimpleAxe");
            PopUpController popUpControllerScript = NewWeapon.GetComponent<PopUpController>() as PopUpController;
            popUpControllerScript.ShowPopup("SimpleAxe", weaponDamage);
            SetWeaponImage weaponUI = WeaponSelected.GetComponent<SetWeaponImage>();
            weaponUI.SetImage("SimpleAxe");
            // confere se o setactive do weaponselected é false e se for muda para true
            SetWeaponImage weaponUIPopUp = WeaponSelectedPopUp.GetComponent<SetWeaponImage>();
            weaponUIPopUp.SetImage("SimpleAxe");
            confereWeaponSelected();
            return true;
        }

        if (hit != null && (hit.gameObject.CompareTag("Chest")))
        {
            audioSource.PlayOneShot(openChest);
            hit.gameObject.SetActive(false);
            SorteiaItem();
            return true;
        }

        if (hit != null && (hit.gameObject.CompareTag("Potion")))
        {
            audioSource.PlayOneShot(lifePotion);
            hit.gameObject.SetActive(false);
            if (vida < 5)
                vida++;
                _liveImage.sprite = _liveSprites[vida];
            return true;
        }

        Vector3Int gridPosition = groundTilemap.WorldToCell(newPosition);
        if (!groundTilemap.HasTile(gridPosition) || wallTilemap.HasTile(gridPosition))
            return false;
        return true;

    }
}
