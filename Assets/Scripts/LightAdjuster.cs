using UnityEngine;

public class LightAdjuster : MonoBehaviour {
    public static LightType SabotageLevel;

    [SerializeField] private Light _light;
    [Range(0.2f, 2f)]
    [SerializeField] private float _sabotageFlickerRate;
    private float _tempFlickerRate;
    private Material _shaderMaterial;
    private Color _defaultColor;
    private float _lightLevel;

    private void Awake() {
        SabotageLevel = LightType.NORMAL;
        _shaderMaterial = gameObject.GetComponent<Renderer>().material;
        _defaultColor = _light.color;
        _lightLevel = _light.intensity;
        _tempFlickerRate = _sabotageFlickerRate;
    }

    private void Update() {
        SabotageLights();
        LightsOut();
        if (SabotageLevel == LightType.NORMAL) {
            _light.intensity = 1;
            _light.color = _defaultColor;
            _shaderMaterial.EnableKeyword("_Emission");
        }
    }
    
    private void SabotageLights() {
        if (SabotageLevel == LightType.EMERGENCY) {
            _light.color = Color.red;
            _shaderMaterial.DisableKeyword("_EMISSION");
            if (_light.intensity >= 1) {
                _tempFlickerRate = -_sabotageFlickerRate;
            } else if  (_light.intensity == 0) {
                FindObjectOfType<AudioManager>().Play("alarm");
                _tempFlickerRate = _sabotageFlickerRate;
            }
            _light.intensity += _tempFlickerRate * Time.deltaTime;
        } else {
            FindObjectOfType<AudioManager>().Stop("alarm");
        }
    }
    private void LightsOut() {
        if (SabotageLevel == LightType.LIGHTS_OUT) {
            _light.intensity = 0;
            _shaderMaterial.DisableKeyword("_EMISSION");
        }
    }
    
    public void LightDecrease() {
        _lightLevel -= 0.05f;
        _light.intensity = _lightLevel;
    }
    public enum LightType {
        NORMAL,
        LIGHTS_OUT,
        EMERGENCY,
    }
}


