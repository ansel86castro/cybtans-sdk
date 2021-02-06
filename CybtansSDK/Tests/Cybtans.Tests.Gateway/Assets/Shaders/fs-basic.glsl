#version 300 es
precision mediump float;

struct Light {
    vec3 pos;
    vec3 dir;
    vec3 diffuse;
    vec3 specular;
    float spotPower;
};

struct Hemispherical {
    vec3 skyColor;
    vec3 groundColor;
    vec3 northPole;
};

struct Material {
    vec4 diffuse;
    vec3 specular;
    float specularPower;
};

in vec3 v_positionW;
in vec3 v_normalW;
in vec2 v_texCoord;
in float v_occ;
in float v_screenCoord;


uniform Light uLight;
uniform Hemispherical uHemisphereLight;
uniform vec3 uAmbient;
uniform vec3 uEyePos;
uniform Material uMaterial;

uniform sampler2D uDiffuseSampler;

out vec4 Color;

vec4 lit(float NdotL, float NdotH, float m){

  float ambient = 1.0;
  float diffuse = max(NdotL, 0.0);
  float specular = step(0.0, NdotL) * max(NdotH, 0.0);
  specular = pow(specular, m);
  return vec4(ambient, diffuse, specular, 1.0);
}


vec3 ComputeHemisphere()
{
	float k = 0.5f + 0.5f * dot(v_normalW, uHemisphereLight.northPole);
	return mix(uHemisphereLight.groundColor ,uHemisphereLight.skyColor , k) * ( v_occ);	
}


vec3 DirectionalLight(vec3 diffuse, vec3 specular, float specularPower)
{
	vec3 toEye = normalize(uEyePos - v_positionW);	
    vec3 lightDir = -uLight.dir;

	vec3 h = normalize(lightDir + toEye);
	vec4 l = lit(dot(v_normalW, lightDir), dot(v_normalW, h), specularPower);	
	
	//add diffuse contribution
	vec3 color = (diffuse * uLight.diffuse) * l.y + (specular * uLight.specular) * l.z;
	
    return color;
}



void main() 
{
    vec4 diffuse  = texture(uDiffuseSampler, v_texCoord);
    diffuse *= uMaterial.diffuse;

    Color = diffuse;
    Color.rgb *= ComputeHemisphere();
    Color.rgb += DirectionalLight(diffuse.rgb, uMaterial.specular, uMaterial.specularPower);


//    vec4 diffuse = texture(uDiffuseSampler, v_texCoord);
//
//    diffuse.rgb *= uMaterial.diffuse.rgb;
//    
//    Color = diffuse;    
//    Color.rgb = DirectionalLight(diffuse.rgb, uMaterial.specular, uMaterial.specularPower);
//    Color.rgb += uAmbient * diffuse.rgb * v_occ;   
}