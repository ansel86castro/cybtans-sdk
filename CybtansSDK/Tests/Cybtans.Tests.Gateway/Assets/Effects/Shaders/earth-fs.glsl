﻿#version 300 es
precision mediump float;

struct Light {
    vec3 pos;
    vec3 dir;
    vec3 diffuse;
    vec3 specular;
    float spotPower;
};


struct Material {
    vec4 diffuse;
    vec3 specular;
    float specularPower;
};

in vec3 v_positionW;
in vec3 v_normalW;
in vec3 v_tangentW;
in vec3 v_binormalW;
in vec2 v_texCoord;
in float v_occ;
in float v_screenCoord;


uniform Light uLight;

uniform vec3 uAmbient;
uniform vec3 uEyePos;
uniform Material uMaterial;

uniform sampler2D uDiffuseSampler;
uniform sampler2D uNormalSampler;
uniform sampler2D uCloudSampler;
uniform sampler2D uSpecularSampler;
uniform sampler2D uNightSampler;

out vec4 Color;

vec4 lit(float NdotL, float NdotH, float m){

  float ambient = 1.0;
  float diffuse = max(NdotL, 0.0);
  float specular = step(0.0, NdotL) * max(NdotH, 0.0);
  specular = pow(specular, m);
  return vec4(ambient, diffuse, specular, 1.0);
}

vec3 DirectionalLight(vec3 normal, vec3 diffuse, vec3 specular, float specularPower)
{
	vec3 toEye = normalize(uEyePos - v_positionW);	
    vec3 lightDir = -uLight.dir;

    float nDotL = dot(normal, lightDir);
    vec3 reflection = (2.0 * normal * nDotL) - lightDir;
    float rDotV = dot(reflection, toEye);

	vec4 l = lit(nDotL, rDotV , specularPower);	
	
	//add diffuse contribution
	vec3 color = (diffuse * uLight.diffuse) * l.y + (specular * uLight.specular) * l.z;

      // Add ground lights if the area is not in sunlight
    float sunlitRatio = clamp(2.0 * nDotL, 0.0, 1.0);
    vec4 nightColor = texture(uNightSampler, v_texCoord);
    color += mix(nightColor.rgb, color, vec3( sunlitRatio, sunlitRatio, sunlitRatio));

     // Add atmosphere
    float atmosphereRatio = 1.0 - clamp( dot(toEye, normal) ,0.0, 1.0);
    color.rgb += 0.30 * vec3(0.3, 0.5, 1.0) * pow(atmosphereRatio, 2.0);

    return color;
}


vec3 normalMap(){
    // Fetch the tangent space normal from normal map
    vec3 normal = texture( uNormalSampler, v_texCoord).xyz;
    
    // Scale and bias from [0, 1] to [−1, 1]
     normal = (normal * 2.0) - 1.0;

    // Construct a matrix to transform from tangent to
    // world space
    mat3 tangentToWorldMat = mat3( v_tangentW, v_binormalW, v_normalW );
    // Transform normal to world space and normalize
    normal = normalize( tangentToWorldMat * normal );
    
    return normal;
}

void main() 
{
    vec3 normal = normalMap();
    vec4 diffuse  = texture(uDiffuseSampler, v_texCoord);
    diffuse *= uMaterial.diffuse;

    diffuse.rgb += texture(uCloudSampler, v_texCoord).rgb * 0.8;

    vec3 specular = uMaterial.specular  * texture(uSpecularSampler, v_texCoord).rgb;

    Color.rgb += DirectionalLight(normal, diffuse.rgb, specular, uMaterial.specularPower);
    Color.a = diffuse.a;
}
