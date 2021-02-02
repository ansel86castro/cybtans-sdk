import { mat4 } from "gl-matrix";
import { AmbientLight } from "./AmbientLight";
import Camera from "./Camera";
import Frame, { LightComponent } from "./Frame";
import { IRenderable } from "./Interfaces";
import Light from "./Light";
import Material from "./Material";
import { float3 } from "./MathUtils";
import Mesh from "./Mesh";
import { SceneDto, Uomtype } from "./models";
import SceneManager from "./SceneManager";
import MeshSkin from "./SkinMesh";
import SkinMesh from "./SkinMesh";
import Texture from "./Textures";

export default class Scene implements IRenderable {  
    manager:SceneManager;
    gl:WebGL2RenderingContext;
    id: string;
    name?: string|null;
    units: number;
    unitOfMeasure: Uomtype;
    root?:Frame;
    visible:boolean = true;
    //program sources
    ambient:AmbientLight;
    currentCamera?:Camera;    
    currentLight?: LightComponent;
    renderables:IRenderable[]=[];
    
    cameras: Map<string, Camera> = new Map();
    lights:Light[]= [];
    textures:Map<string, Texture> = new Map();
    materials:Map<string, Material> = new Map();
    meshes:Map<string, Mesh> = new Map();
    skins:Map<string, SkinMesh> = new Map();
    

    constructor(manager:SceneManager, id?:string, name?:string){
        this.gl = manager.gl;
        this.manager = manager;
        this.id = id || '';
        this.name = name;      
        this.units = 1;
        this.unitOfMeasure = Uomtype.meters;
        this.cameras = new Map();
        this.ambient = new AmbientLight();
    }

    loadData(data:SceneDto){
        this.id = data.id;
        this.name = data.name;
        this.units = data.units;
        this.unitOfMeasure = data.unitOfMeasure;       
        this.ambient = new AmbientLight(data.ambient);

        if(data.cameras){
            this.cameras = new Map();
            data.cameras.forEach(dto => 
                {
                    let c = new Camera(this.manager.width, this.manager.height, dto);
                    c.projMtx = mat4.perspective(c.projMtx, dto.fieldOfView, c.width /c. height, c.nearPlane, c.farPlane);
                    c.viewMtx = mat4.lookAt(c.viewMtx, float3([5, 5, -5]), float3([0,0,0]), float3([0,1,0]));
                    c.onViewUpdated();
                    
                    this.cameras?.set(c.id, c);
                });

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

        if(data.root){
            this.root = new Frame(this, data.root);
            this.root.commitChanges();
            this.root.initialize(this);
        }
    }

    getTextureById(id: any) : Texture|undefined {
       return this.textures?.get(id);
    }

    async load(baseUrl:string){
        let promises:Promise<boolean>[] = [];
        let completed = 0;
        try{
            if(this.textures){
                for (let [key, value] of this.textures) {
                await value.load(baseUrl);
                }
            }   
        }catch(e){
            console.error(e);
            throw e;
        }
    }

    update(elapsed:number){
        if(this.root){
            this.root.commitChanges();
        }
    }

    render(ctx:SceneManager){
        if(this.currentCamera){
            ctx.programSource(Camera, this.currentCamera);
        }
        
        if(this.currentLight){
            ctx.programSource(LightComponent, this.currentLight);
            ctx.programSource(Light, this.currentLight.light);
        }

        ctx.programSource(AmbientLight, this.ambient);
        
        for (const item of this.renderables) {
            item.render(ctx);
        }
    }

}