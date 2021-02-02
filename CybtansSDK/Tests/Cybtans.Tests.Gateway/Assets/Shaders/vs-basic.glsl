﻿#version 300 es
precision mediump float;

in vec3 a_position;
in vec3 a_normal;
in vec3 a_tangent;
in vec2 a_texCoord;
in float a_occ;

uniform mat4 u_World;
uniform mat4 u_ViewProj;

out highp vec3 v_positionW;
out highp vec3 v_normalW;
out highp vec2 v_texCoord;
out float v_occ;
out vec4 v_screenCoord;

void main(){
    v_positionW =  vec3(u_World * vec4(a_position, 1));

    v_normalW = normalize(mat3(u_World) * a_normal);    
    gl_Position =  u_ViewProj * vec4(v_positionW ,1) ;

    v_screenCoord.x = (gl_Position.x + gl_Position.w) * 0.5f;
    v_screenCoord.x = (gl_Position.w + gl_Position.y) * 0.5f;
    v_screenCoord.zw = gl_Position.ww;
    v_texCoord = a_texCoord;
    v_occ = 1.0 - a_occ;
}