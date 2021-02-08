import { vec3, vec4 } from "gl-matrix";
import { float3, float4 } from "./MathUtils";
import { MaterialDto } from "./models";
import Scene from "./Scene";
import Texture from "./Textures";

type TextureMap = {[semantic:string]: Texture};

export default class Material {
    name?: string|null;
    diffuse: vec4;
    specular: vec3;
    specularPower: number;
    emissive: vec3;    
    id: string;
    reflectivity: number;
    refractivity: number;
    textures:TextureMap = {};

    constructor( scene:Scene, data:MaterialDto){
        this.id = data.id;
        this.name = data.name;
        this.diffuse = float4(data.diffuse);
        this.specular = float3(data.specular);
        this.emissive = float3(data.emissive);
        this.specularPower = data.specularPower;
        this.reflectivity=data.reflectivity;
        this.refractivity = data.refractivity;
        
        if(data.textures){            
            for (const key in data.textures) {
                if (Object.prototype.hasOwnProperty.call(data.textures, key)) {
                    const id = data.textures[key];
                    let tex =  scene.getTextureById(id);
                    if(tex){
                        this.textures[key] = tex;
                    }
                }
            }
        }
    }
}