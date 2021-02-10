import { mat4 } from "gl-matrix";
import { AmbientLight } from "./AmbientLight";
import Camera from "./Camera";
import Frame, { FrameUpdateFunc, LightComponent } from "./Frame";
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

export type UpdateFunc = (elapsed:number)=>void;

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
    lightsComponents: LightComponent[] = [];
    textures:Map<string, Texture> = new Map();
    materials:Map<string, Material> = new Map();
    meshes:Map<string, Mesh> = new Map();
    skins:Map<string, SkinMesh> = new Map();
    private updates:UpdateFunc[] = [];    

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
        
        if(data.ambient){
            this.ambient = new AmbientLight(data.ambient);
        }

        if(data.cameras){
            this.cameras = new Map();
            data.cameras.forEach(dto => 
                {
                    let c = new Camera({ ...dto, width: this.manager.width, height: this.manager.height});                   
                    c.viewMtx = mat4.lookAt(c.viewMtx, float3([0, 3, -4]), float3([0,0.8,0]), float3([0,1,0]));
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
        let promises:Promise<void>[] = [];
        let completed = 0;
        try{
            if(this.textures){
                for (let [key, value] of this.textures) {
                    promises.push(value.load(baseUrl));
                }
            }

            await Promise.all(promises);

        }catch(e){
            console.error(e);
            throw e;
        }
    }

    update(elapsed:number){
       for (const item of this.updates) {
           item(elapsed);
       }
    }

    render(ctx:SceneManager){
        if(this.currentCamera){
            ctx.setSource(Camera, this.currentCamera);
        }
        
        if(this.currentLight){
            ctx.setSource(LightComponent, this.currentLight);
            ctx.setSource(Light, this.currentLight.light);
        }

        ctx.setSource(AmbientLight, this.ambient);
        
        for (const item of this.renderables) {
            item.render(ctx);
        }
    }

    addUpdate(func:UpdateFunc): ()=>void {
        this.updates.push(func);
        return ()=> {
            let idx = this.updates.indexOf(func);
            if(idx < 0) return;

            this.updates.splice(idx, 1);
        }
    }

    getNodeByName(name:string): Frame|null{
        if(!this.root) return null;
        return this.root.getNodeByName(name);
    }

    addNodeUpdate(name:string, func:FrameUpdateFunc){
        if(this.root){
            let node = this.root.getNodeByName(name);
            if(!node) return;

            return this.addUpdate(elapsed=> func(elapsed, node!));
        }
     }
}