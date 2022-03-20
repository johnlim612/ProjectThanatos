using UnityEngine;

public class LightAdjuster : MonoBehaviour {
    public static LightType SabotageLevel;

    [SerializeField] private Light _light;
    [Range(0.2f, 2f)]
    [SerializeField] private float _sabotageFlickerRate;
    private float _tempFlickerRate;
    private Material _shaderMaterial;
    private Color _defaultColor;

    private void Awake() {
        SabotageLevel = LightType.EMERGENCY;
        _shaderMaterial = gameObject.GetComponent<Renderer>().material;
        _defaultColor = _light.color;
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
            if (_light.intensity >= 0.75) {
                _tempFlickerRate = -_sabotageFlickerRate;
            } else if  (_light.intensity == 0) {
                _tempFlickerRate = _sabotageFlickerRate;
            }
            _light.intensity += _tempFlickerRate * Time.deltaTime;
        }
    }
    private void LightsOut() {
        if (SabotageLevel == LightType.LIGHTS_OUT) {
            _light.intensity = 0;
            _shaderMaterial.DisableKeyword("_EMISSION");
        }
    }

    public enum LightType {
        NORMAL,
        LIGHTS_OUT,
        EMERGENCY,
    }
}


