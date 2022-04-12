using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineTrigger : MonoBehaviour
{
    [SerializeField] private float _glowRange = 5;
    [SerializeField] private InteractableSabotage _sabotage;
    private GameObject _player;
    private cakeslice.Outline _outline;
    // Start is called before the first frame update
    void Awake()
    {
        _outline = gameObject.GetComponent<cakeslice.Outline>();
        _player = GameObject.Find(Constants.PlayerKey);
    }

    // Update is called once per frame
    void Update()
    {
        if (_outline != null) {
            if (_sabotage != null) {
                if (_sabotage.SabotageActive()) {
                    RegulateGlow();
                } else {
                    _outline.enabled = false;
                }
            } else {
                RegulateGlow();
            }
        }
    }

    public void RegulateGlow() {
        float dis = Vector3.Distance(transform.position, _player.transform.position);
        if (dis < _glowRange) {
            _outline.enabled = true;
        } else {
            _outline.enabled = false;
        }
    }
}
