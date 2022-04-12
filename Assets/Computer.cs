using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Computer: InteractableObject {
    [SerializeField] private GameObject _screen;
    GameObject player;

    void Start() {
        player = GameObject.Find(Constants.PlayerKey);
    }

    public override void InteractObject() {
        Interact();
    }

	private void Interact() {
        print("this ran");
        StartCoroutine(ShowScreen());
    }

    IEnumerator ShowScreen() {
        player.GetComponent<PlayerController>().enabled = false;
        _screen.SetActive(true);    
        yield return new WaitForSeconds(3f);
        player.GetComponent<PlayerController>().enabled = true;
        _screen.SetActive(false);
    }

}