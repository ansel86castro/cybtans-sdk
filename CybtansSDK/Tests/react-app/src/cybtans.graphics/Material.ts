import { vec4 } from "gl-matrix";
import { MaterialDto } from "./models";
import Scene from "./Scene";
import Texture from "./Textures";

type TextureMap = {[semantic:string]: Texture};

export default class Material {
    name?: string|null;
    diffuse?: vec4;
    specular?: vec4;
    emissive?: vec4;    
    id: string;
    textures:TextureMap = {};

    constructor( scene:Scene, data:MaterialDto){
        this.id = data.id;
        this.name = data.name;
        this.diffuse = data.diffuse as vec4;
        this.specular = data.specular as vec4;
        this.emissive = data.emissive as vec4;
        
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