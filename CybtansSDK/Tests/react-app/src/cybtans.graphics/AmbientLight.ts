import { vec3 } from "gl-matrix";
import { float3 } from "./MathUtils";
import { AmbientLightDto } from "./models";

export class AmbientLight {
    ambientColor?: vec3;
    skyColor?: vec3;
    groundColor?: vec3;
    intensity: number;
    northPole?: vec3;

    constructor(data?:AmbientLightDto|null){
        this.ambientColor = float3(data?.ambientColor || [0.1,0.1,0.1]);
        this.skyColor = float3(data?.skyColor || [0.3, 0.3, 0.3]);
        this.groundColor = float3(data?.groundColor || [0.1,0.1,0.1]);
        this.intensity = data?.intensity || 1;
        this.northPole = float3(data?.northPole || [0,1,0]);
    }
}