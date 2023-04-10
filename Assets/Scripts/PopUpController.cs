using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpController : MonoBehaviour
{

    public GameObject popupContainer;
    public  TextMeshProUGUI popupText;
    public float popupDuration = 3f;

    private void Start() {
        popupContainer.SetActive(false);
    }

    public void ShowPopup(string weaponName, Dictionary<string, int> weaponDamage) {
        popupText.text = "You have acquired the " + weaponName + "weapon that deals " +  weaponDamage[weaponName] + " damage to enemies!";
        popupContainer.SetActive(true);
        StartCoroutine(HidePopup());
    }

    private IEnumerator HidePopup() {
        yield return new WaitForSeconds(popupDuration);
        popupContainer.SetActive(false);
    }
}

