import { vec3, vec4 } from "gl-matrix";
import { float3 } from "./MathUtils";
import { LightDto, LightType } from "./models";

export default class Light {
    static readonly type = 'Light';


    diffuse: vec3;
    specular: vec3;
    ambient: vec3;
    attenuation: vec3;
    enable: boolean;
    intensity: number;
    spotPower: number;
    type: LightType;
    range: number;
    id: string;

    constructor(data: LightDto) {
        this.diffuse = float3(data.diffuse)
        this.specular = float3(data.specular);
        this.ambient = float3(data.ambient);
        this.attenuation = float3(data.attenuation);
        this.enable = data.enable;
        this.spotPower = data.spotPower;
        this.type = data.type;
        this.range = data.range;
        this.id = data.id;
        this.intensity = data.intensity;
    }
}