using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public GameObject player;

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
    private GameObject winPanel;

    [SerializeField]
    private Image metronome;

    private bool ganhou = false;

    private int level = 0;

    // private List<string> weapons = new List<string> ();
    

    private Dictionary<string, int> weaponDamage = new Dictionary<string, int> ();

    [SerializeField] 
    private GameObject NewWeapon;

    [SerializeField]
    private GameObject WeaponSelected;
    

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
        
        if (PlayerManager.Instance != null && PlayerManager.Instance.weapons.Count != 0)
        {
            SetWeaponImage weaponUI = WeaponSelected.GetComponent<SetWeaponImage>();
            Debug.Log("Arma selecionada: " + PlayerManager.Instance.weapons[PlayerManager.Instance.weapons.Count-1]);
            weaponUI.SetImage(PlayerManager.Instance.weapons[PlayerManager.Instance.weapons.Count-1]);
        }
        else
        {
            WeaponSelected.SetActive(false);
            Debug.Log("Nenhuma arma selecionada");
        }


        controls.Main.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());
        conductor.enabled = true;
        gameOverPanel.SetActive(false);
        metronome.enabled = true;
        winPanel.SetActive(false);
        
    }

    void Update(){
        if (ganhou){
            metronome.enabled = false;
            Conductor conductorScript = conductor.GetComponent<Conductor>() as Conductor;
            conductorScript.musicSource.Stop();
            winPanel.SetActive(true);
            WeaponSelected.SetActive(false);
            _liveImage.enabled = false;
            return;
        }

    }

    void LateUpdate()
    {
        if (PlayerManager.Instance != null && PlayerManager.Instance.weapons.Count != 0)
        {
            SetWeaponImage weaponUI = WeaponSelected.GetComponent<SetWeaponImage>();
            // Debug.Log("Arma selecionada: " + PlayerManager.Instance.weapons[PlayerManager.Instance.weapons.Count-1]);
            weaponUI.SetImage(PlayerManager.Instance.weapons[PlayerManager.Instance.weapons.Count-1]);
        }
        if (conductor.GetComponent<Conductor>().musicSource.isPlaying == false){
            if (level == 1){
                level+=1;
                SceneManager.LoadScene(level);
                Vector3 spawnPosition = new Vector3(33, 0f, 0f);
                player.transform.position = spawnPosition;
            }
            if (level == 2){
                level+=1;
                SceneManager.LoadScene(level);
                Vector3 spawnPosition = new Vector3(0.5f, 5.5f, 0f);
                player.transform.position = spawnPosition;
            }
            if (level == 3){
                    _liveImage.sprite = _liveSprites[0];
                    player.SetActive(false);
                    metronome.enabled = false;
                    gameOverPanel.SetActive(true);
                    WeaponSelected.SetActive(false);
                    _liveImage.enabled = false;
                    Conductor conductorScript = conductor.GetComponent<Conductor>() as Conductor;
                    conductorScript.musicSource.Stop();
                    conductor.enabled = false;
                    return;
            }
        } 

    }

    private void Move(Vector2 direction)
    {
        if (conductor.seconds_off_beat() < beat_detection_range) {

            if (CanMove(direction))
                transform.position += (Vector3)direction;

        }

    }

    public void TakeDamage(int damage)
    {
        vida -= damage;
        if(vida > 0)
            _liveImage.sprite = _liveSprites[vida];
        else
        {
            _liveImage.sprite = _liveSprites[0];
            player.SetActive(false);
            metronome.enabled = false;
            gameOverPanel.SetActive(true);
            WeaponSelected.SetActive(false);
            _liveImage.enabled = false;
            Conductor conductorScript = conductor.GetComponent<Conductor>() as Conductor;
            conductorScript.musicSource.Stop();
            conductor.enabled = false;
            return; // Adicionado para interromper a execução do método
        }
    }

    public void confereWeaponSelected(){
        if (PlayerManager.Instance.weapons.Count == 0){
            WeaponSelected.SetActive(false);
        } else {
            WeaponSelected.SetActive(true);
        }
    }

    public void SorteiaItem(){
        System.Random rand = new System.Random();

        List<string> weaponList = new List<string>(weaponDamage.Keys);
        int randomIndex = rand.Next(weaponList.Count);
        string randomWeapon = weaponList[randomIndex];
        PlayerManager.Instance.weapons.Add(randomWeapon);
        SetWeaponImage weaponUI = WeaponSelected.GetComponent<SetWeaponImage>();
        weaponUI.SetImage(randomWeapon);
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

        if (hit != null && (hit.gameObject.CompareTag("Enemy")))
        {
            BatAttack batScriptConfere = hit.gameObject.GetComponent<BatAttack>() as BatAttack;
            if (batScriptConfere != null)
            {
                batScriptConfere.TakeDamageBat(selecionaUltimaArma());
                return false;
            }
            
            SlimeAttack slimeScript = hit.gameObject.GetComponent<SlimeAttack>() as SlimeAttack;
            if (slimeScript != null){
                slimeScript.TakeDamageSlime(selecionaUltimaArma());
                return false;
            }
            OogaBoongaAttack oogaScript = hit.gameObject.GetComponent<OogaBoongaAttack>() as OogaBoongaAttack;
            if (oogaScript != null){
                oogaScript.TakeDamageOoga(selecionaUltimaArma());
                return false;
            }

            SkeletonAttack skeletonScript = hit.gameObject.GetComponent<SkeletonAttack>() as SkeletonAttack;
            if (skeletonScript != null){
                skeletonScript.TakeDamageSkeleton(selecionaUltimaArma());
                return false;
            }

            KnightAttack knightScript = hit.gameObject.GetComponent<KnightAttack>() as KnightAttack;
            if (knightScript != null){
                knightScript.TakeDamageKnight(selecionaUltimaArma());
                return false;
            }

            PopGirlAttack popGirlScript = hit.gameObject.GetComponent<PopGirlAttack>() as PopGirlAttack;
            if (popGirlScript != null)
            {
                popGirlScript.TakeDamagePopGirl(selecionaUltimaArma());
                return false;
            }

            BossAttack bossScript = hit.gameObject.GetComponent<BossAttack>() as BossAttack;
            if (bossScript != null)
            {
                bossScript.TakeDamageBoss(selecionaUltimaArma());
                if (bossScript.morreu)
                {
                    ganhou = true;
                }
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
            hit.gameObject.SetActive(false);
            hasKey = true;
            return true;
        }

        if (hit != null && (hit.gameObject.CompareTag("SimpleSword")))
        {
            hit.gameObject.SetActive(false);
            PlayerManager.Instance.weapons.Add("SimpleSword");
            PopUpController popUpControllerScript = NewWeapon.GetComponent<PopUpController>() as PopUpController;
            popUpControllerScript.ShowPopup("SimpleSword", weaponDamage);
            SetWeaponImage weaponUI = WeaponSelected.GetComponent<SetWeaponImage>();
            weaponUI.SetImage("SimpleSword");
            confereWeaponSelected();
            return true;
        }

        if (hit != null && (hit.gameObject.CompareTag("Knife")))
        {
            hit.gameObject.SetActive(false);
            PlayerManager.Instance.weapons.Add("Knife");
            PopUpController popUpControllerScript = NewWeapon.GetComponent<PopUpController>() as PopUpController;
            popUpControllerScript.ShowPopup("Knife", weaponDamage);
            confereWeaponSelected();
            Debug.Log("Knife AQUIIII");
            SetWeaponImage weaponUI = WeaponSelected.GetComponent<SetWeaponImage>();
            weaponUI.SetImage("Knife");
            Debug.Log("SETTEIII");            
            return true;
        }

        if (hit != null && (hit.gameObject.CompareTag("SimpleAxe")))
        {
            hit.gameObject.SetActive(false);
            PlayerManager.Instance.weapons.Add("SimpleAxe");
            PopUpController popUpControllerScript = NewWeapon.GetComponent<PopUpController>() as PopUpController;
            popUpControllerScript.ShowPopup("SimpleAxe", weaponDamage);
            SetWeaponImage weaponUI = WeaponSelected.GetComponent<SetWeaponImage>();
            weaponUI.SetImage("SimpleAxe");
            // confere se o setactive do weaponselected é false e se for muda para true
            confereWeaponSelected();
            return true;
        }

        if (hit != null && (hit.gameObject.CompareTag("Chest")))
        {
            hit.gameObject.SetActive(false);
            SorteiaItem();
            return true;
        }

        if (hit != null && (hit.gameObject.CompareTag("Potion")))
        {
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
