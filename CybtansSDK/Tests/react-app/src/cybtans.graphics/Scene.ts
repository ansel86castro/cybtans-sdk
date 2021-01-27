import Camera from "./Camera";
import Light from "./Light";
import Material from "./Material";
import Mesh from "./Mesh";
import { SceneDto, Uomtype } from "./models";
import MeshSkin from "./SkinMesh";
import SkinMesh from "./SkinMesh";
import Texture from "./Textures";

export default class Scene {  
    gl:WebGL2RenderingContext;
    id: string;
    name?: string|null;
    units: number;
    unitOfMeasure: Uomtype;
    currentCamera?:Camera;
    cameras: Map<string, Camera> = new Map();
    lights:Light[]= [];
    textures:Map<string, Texture> = new Map();
    materials:Map<string, Material> = new Map();
    meshes:Map<string, Mesh> = new Map();
    skins:Map<string, SkinMesh> = new Map();

    constructor(gl:WebGL2RenderingContext, data:SceneDto){
        this.gl = gl;
        this.id = data.id;
        this.name = data.name;
        this.units = data.units;
        this.unitOfMeasure = data.unitOfMeasure;
        this.cameras = new Map();
        
        if(data.cameras){
            this.cameras = new Map();
            data.cameras.forEach(c=> this.cameras?.set(c.id, new Camera(c)));

            if(data.currentCamera)
                this.currentCamera = this.cameras.get(data.currentCamera);
        }

        if(data.lights){
            this.lights = data.lights.map(l=> new Light(l));
        }

        if(data.textures){            
            data.textures.forEach(t=> this.textures?.set(t.id, new Texture(this, t))); 
        }

        if(data.materials){            
            data.materials.forEach(m=> this.materials?.set(m.id, new Material(this, m)));
        }

        if(data.meshes){            
            data.meshes.forEach(m=> this.meshes?.set(m.id, new Mesh(this, m)));
        }

        if(data.skins){            
            data.skins.forEach(m=> this.skins?.set(m.id, new MeshSkin(this, m)));
        }
    }

    getTextureById(id: any) : Texture|undefined {
       return this.textures?.get(id);
    }

    load(){
        if(this.textures){
            for (let [key, value] of this.textures) {
                value.load()
            }
        }
    }


}