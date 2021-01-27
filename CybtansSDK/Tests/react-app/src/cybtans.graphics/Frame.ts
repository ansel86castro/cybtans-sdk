import { mat4, vec3 } from "gl-matrix";
import Camera from "./Camera";
import Light from "./Light";
import Material from "./Material";
import Mesh from "./Mesh";
import { FrameDto, FrameLightDto, FrameType } from "./models";
import Scene from "./Scene";
import MeshSkin from "./SkinMesh";

export default class Frame {
    id:string;
    name?: string|null;
    localTransform: mat4;
    bindParentTransform: mat4;
    bindAffectorTransform: mat4;
    worldTransform: mat4;
    parent?: Frame;
    type: FrameType;
    range: number;
    childrens: Frame[]= [];
    bindTargetId?: string|null;
    tag?: string|null;
    component?: FrameComponent|null;
    
    constructor(scene:Scene, data:FrameDto, parent?:Frame){
        this.id = data.id;
        this.name = data.name;
        this.localTransform = data.localTransform as mat4;
        this.bindParentTransform = data.bindParentTransform as mat4;
        this.worldTransform = data.worldTransform as mat4;
        this.bindAffectorTransform = data.bindAffectorTransform as mat4;
        this.type = data.type;
        this.range = data.range;
        this.tag = data.tag;
        this.parent = parent;
        
        if(data.childrens){
            for (const item of data.childrens) {
                this.childrens?.push(new Frame(scene, item, this));   
            }
        }

        if(data.component){
            if(data.component.camera){
                this.component = new CameraComponent(scene.cameras.get(data.component.camera)!, this);
            }else if(data.component.light){
                this.component = new LightComponent(scene.lights.find(x=>x.id === data.component?.light?.light)!, 
                data.component.light, this);
            }else if(data.component.mesh){
                let materials = data.component.mesh.materials!.map(x=>scene.materials.get(x)!);
                let mesh = scene.meshes.get(data.component.mesh.mesh)!;
                this.component = new MeshComponent(mesh, materials, this);
            }else if(data.component.meshSkin){
                let materials = data.component.meshSkin.materials!.map(x=>scene.materials.get(x)!);
                let skin = scene.skins.get(data.component.meshSkin.meshSkin)!;
                this.component = new MeshSkinComponent(skin, materials, this);
            }
        }
    }

    forEach(action:(frame:Frame)=>any): Frame|null {
        if(action(this) === true) return this;
        
        for (const item of this.childrens) {
            let frame = item.forEach(action);
            if(frame)
                return frame;
        }
        return null;
    }

    unwrap():{[key:string]: Frame}{
        let map:{[key:string]: Frame} = {};
        this.forEach(x=> map[x.id] = x);

        return map;
    }
}

export class FrameComponent {
    constructor(public frame:Frame){

    }
}

export class CameraComponent extends FrameComponent {    
    constructor(public camera:Camera, frame:Frame){
        super(frame);        
    }
}

export class MeshComponent extends FrameComponent{
    constructor(
        public mesh:Mesh, 
        public materials:Material[], 
        frame:Frame){
            super(frame);
        }
}

export class LightComponent extends FrameComponent{
    localPosition?: vec3;
    localDirection?: vec3;

    constructor(
        public light:Light,
        data: FrameLightDto,
        frame:Frame
    ){
        super(frame);

        this.localDirection = data.localDirection as vec3;
        this.localPosition = data.localPosition as vec3;
    }
}

export class MeshSkinComponent extends MeshComponent{
    constructor(
        public skin:MeshSkin, 
        public materials:Material[], 
        frame:Frame){
            super(skin.mesh , materials, frame);
        }
}


