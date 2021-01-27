import { vec3, vec4 } from "gl-matrix";
import { LightDto, LightType } from "./models";

export default class Light {
    diffuse?: vec3;
    specular?: vec3;
    ambient?: vec3;
    attenuation?: vec3;
    enable: boolean;
    intensity: number;
    spotPower: number;
    type: LightType;
    range: number;
    id: string;

    constructor(data:LightDto){
        this.diffuse = data.diffuse as vec3;
        this.specular = data.specular as vec3;
        this.ambient = data.ambient as vec3;
        this.attenuation = data.attenuation as vec3;
        this.enable = data.enable;
        this.spotPower = data.spotPower;
        this.type = data.type;
        this.range = data.range;
        this.id = data.id;
        this.intensity = data.intensity;        
    }
}