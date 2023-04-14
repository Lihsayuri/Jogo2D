using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetWeaponImage : MonoBehaviour
{
    public Sprite simpleSwordSprite;
    public Sprite axeSprite;

    public Sprite knifeSprite;
    public Sprite doubleAxeSprite;
    public Sprite hammerSprite;
    public Sprite persianSaberSprite;
    public Sprite maceSprite;
    public Sprite fireSwordSprite;
    public Image weaponImage;

    private void Start()
    {
        // Inicialmente, definimos a imagem da arma como nula (sem arma equipada)
        weaponImage.sprite = null;
    }

    public void SetImage(string weaponName)
    {
        switch (weaponName)
        {
            case "SimpleSword":
                weaponImage.sprite = simpleSwordSprite;
                break;
            case "SimpleAxe":
                weaponImage.sprite = axeSprite;
                break;
            case "Knife":
                weaponImage.sprite = knifeSprite;
                break;
            case "DoubleAxe":
                weaponImage.sprite = doubleAxeSprite;
                break;
            case "Hammer":
                weaponImage.sprite = hammerSprite;
                break;
            case "PersianSaber":
                weaponImage.sprite = persianSaberSprite;
                break;
            case "Mace":
                weaponImage.sprite = maceSprite;
                break;
            case "FireSword":
                weaponImage.sprite = fireSwordSprite;
                break;
            // caso você tenha outras armas, basta adicionar mais casos aqui
            default:
                break;
        }
    }
}
