using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    // Start is called before the first frame update
    void Awake()
    {
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            _animator.Play("DoorOpen");
            Debug.Log("Open");
            _animator.SetBool("door", true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            Debug.Log("Close");
            _animator.SetBool("door", false);
        }
    }
}
