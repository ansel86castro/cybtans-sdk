import { glMatrix, mat4, vec3, vec4 } from "gl-matrix";
import Camera from "./Camera";
import Light from "./Light";
import Material from "./Material";
import { float3, matrix, transformNormal } from "./MathUtils";
import Mesh from "./Mesh";
import { FrameDto, FrameLightDto, FrameType } from "./models";
import Scene from "./Scene";
import MeshSkin from "./SkinMesh";
import { decomposeMatrix } from "./MathUtils";
import { IRenderable } from "./Interfaces";
import SceneManager from "./SceneManager";

export default class Frame {
    id:string;
    name?: string|null;

    //transforms
    localScale:mat4;
    localRotation: mat4;
    localTranslation:mat4;
    localMtx: mat4;
    bindParentMtx: mat4;
    bindAffectorMtx: mat4;
    worldMtx: mat4;

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
        this.localMtx = matrix(data.localTransform);
        this.bindParentMtx = matrix(data.bindParentTransform);
        this.worldMtx = matrix(data.worldTransform);
        this.bindAffectorMtx = matrix( data.bindAffectorTransform);
        this.type = data.type;
        this.range = data.range;
        this.tag = data.tag;
        this.parent = parent;

        let [scale, translation, rotation] = decomposeMatrix(this.localMtx);
        this.localScale = mat4.create();
        mat4.scale(this.localScale, this.localScale, scale);
        
        this.localTranslation = mat4.create();
        mat4.translate(this.localTranslation, this.localTranslation, translation);

        this.localRotation = rotation;

        // mat4.transpose(this.localScale, this.localScale);
        // mat4.transpose(this.localTranslation, this.localTranslation);
        // mat4.transpose(this.localRotation, this.localRotation);
        
          
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

    updateLocalPose(){
        mat4.mul(this.localMtx, this.localRotation, this.localScale);
        mat4.mul(this.localMtx, this.localMtx, this.localTranslation);
    }

    updateWorldPose(){
        // world =  (localMtx * (bindParentMtx * parentMtx))
        if(!this.parent){
            this.worldMtx = this.localMtx;
        }else{
            mat4.mul(this.worldMtx, this.bindParentMtx, this.parent.worldMtx);
            mat4.mul(this.worldMtx, this.localMtx, this.worldMtx);
        }        

        if(this.component){
            this.component.onFrameUpdate();
        }
    }

    commitChanges(){
        this.updateWorldPose();

        if(this.childrens){
            for (const item of this.childrens) {
                item.commitChanges();
            }
        }
    }

    initialize(scene:Scene){
        if(this.component){
            this.component.initialize(scene);
        }

        for (const item of this.childrens) {
            item.initialize(scene);
        }
    }
}

export class FrameComponent {
    constructor(public frame:Frame){
    }

    initialize(scene:Scene){
        
    }

    onFrameUpdate(){

    }
}

export class CameraComponent extends FrameComponent {    
    constructor(public camera:Camera, frame:Frame){
        super(frame);        
    }

    initialize(scene:Scene){
        if(scene.currentCamera == this.camera){
            
        }
    }

    onFrameUpdate(){
        this.camera.transform(this.frame.worldMtx);        
    }
}

export class MeshComponent extends FrameComponent implements IRenderable{
    visible: boolean = true;

    constructor(
        public mesh:Mesh, 
        public materials:Material[], 
        frame:Frame){
            super(frame);
        }
    
    initialize(scene:Scene){
        scene.renderables.push(this);
    }

    render(context:SceneManager): void {
        context.programSource(Frame, this.frame);
        this.mesh.render(context, this.materials);
    }

}

export class LightComponent extends FrameComponent{
    localPosition: vec3;
    localDirection: vec3;
    worldPosition:vec3;
    worldDirection:vec3;

    constructor(
        public light:Light,
        data: FrameLightDto,
        frame:Frame
    ){
        super(frame);

        this.localDirection = float3(data.localDirection);
        this.localPosition = float3(data.localPosition);        
        this.worldDirection = vec3.clone(this.localDirection);
        this.worldPosition = vec3.clone(this.localPosition)
    }

    initialize(scene:Scene){
        
    }


    onFrameUpdate(){
         transformNormal (this.worldDirection ,this.localDirection, this.frame.worldMtx);
         vec3.transformMat4(this.worldPosition, this.localPosition, this.frame.worldMtx);
    }
}

export class MeshSkinComponent extends MeshComponent{
    constructor(
        public skin:MeshSkin, 
        public materials:Material[], 
        frame:Frame){
            super(skin.mesh , materials, frame);
        }

    initialize(scene:Scene){
        scene.renderables.push(this);
    }
}


