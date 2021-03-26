import { glMatrix, mat3, mat4, quat, vec3, vec4 } from "gl-matrix";
import Camera from "./Camera";
import Light from "./Light";
import Material from "./Material";
import { euler, eulerFromAxis, float3, getRotationAxis, matrix, matRotationYawPitchRoll, matTranslate, transformNormal, UnitY } from "./MathUtils";
import Mesh from "./Mesh";
import { FrameDto, FrameLightDto, FrameType } from "./models";
import Scene from "./Scene";
import MeshSkin from "./SkinMesh";
import { decomposeMatrix } from "./MathUtils";
import { IRenderable } from "./Interfaces";
import SceneManager from "./SceneManager";
import { HashMap } from "./utils";

export type FrameUpdateFunc = (elapsed:number, frame:Frame)=>void;

export default class Frame {
    id:string;
    name?: string|null;
    scene:Scene;
    //transforms
    localScale:mat4;
    localRotation: mat4;
    localTranslation:mat4;
    localMtx: mat4;   
    bindParentMtx: mat4;
    bindAffectorMtx: mat4;
    worldMtx: mat4;
    worldTranformNormalMtx:mat4;
    
    parent?: Frame;
    type: FrameType;
    range: number;
    childrens: Frame[]= [];
    childresMap : HashMap<Frame|null> = {};
    bindTargetId?: string|null;
    tag?: string|null;
    component?: FrameComponent|null;

    up:vec3;
    front:vec3;
    right:vec3;   
    euler:euler; 
    worldPosition:vec3;
    localPosition:vec3;

    constructor(scene:Scene, data:FrameDto, parent?:Frame){
        this.scene = scene;
        this.id = data.id;
        this.name = data.name;
        this.localMtx = matrix(data.localTransform);
        this.bindParentMtx = matrix(data.bindParentTransform);
        this.worldMtx = matrix(data.worldTransform);
        this.bindAffectorMtx = matrix( data.bindAffectorTransform);
        this.worldTranformNormalMtx = matrix();
        this.up = float3();
        this.front = float3();
        this.right = float3();
        this.euler = float3();
        this.worldPosition = float3();
        this.localPosition = float3();
        this.type = data.type;
        this.range = data.range;
        this.tag = data.tag;
        this.parent = parent;        
        this.localScale = mat4.create();
        this.localTranslation = mat4.create();
        this.localRotation = mat4.create();
        
        this.updateTransforms();

       mat4.invert(this.worldTranformNormalMtx, this.worldMtx);
       mat4.transpose(this.worldTranformNormalMtx, this.worldTranformNormalMtx);       
       
        if(data.childrens){
            for (const item of data.childrens) {
                let c = new Frame(scene, item, this);
                this.childrens?.push(c);
                if(c.name){
                    this.childresMap[c.name] = c;
                }
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

    updateTransforms(){
        let [scale, translation, rotation] = decomposeMatrix(this.localMtx);

        mat4.identity(this.localScale);
        mat4.scale(this.localScale, this.localScale, scale);
        
        mat4.identity( this.localTranslation);
        mat4.translate(this.localTranslation, this.localTranslation, translation);

        this.localRotation = rotation;
        getRotationAxis(this.up, this.front, this.right ,this.localRotation);

        eulerFromAxis(this.euler, this.right, this.up, this.front);
        this.localPosition = translation;
        this.worldPosition = translation;
    }

    addNode(node:Frame){
        node.parent = this;
        this.childrens.push(node);

        if(node.name)
            this.childresMap[node.name] = node;
    }

    removeNode(node:Frame){
        let i = this.childrens.indexOf(node);
        if(i < 0 ) return;

        this.childrens.splice(i, 1);
        node.parent = undefined;

        if(node.name){
            delete this.childresMap[node.name];
        }
    }

    getNodeByName(name:string): Frame|null{
        if(this.name == name)
            return this;

        let node = this.childresMap[name];
        if(node) return node;

        for (const item of this.childrens) {
            node = item.getNodeByName(name);
            if(node){
                return node;
            }
        }

        return null;
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
        mat4.mul(this.localMtx, this.localTranslation, this.localScale);
        mat4.mul(this.localMtx, this.localMtx, this.localRotation);
    }

    updateWorldPose(){
        // world =  transpose(localMtx * (bindParentMtx * parentMtx))
        if(!this.parent){
            this.worldMtx = this.localMtx;
        }else{
            mat4.mul(this.worldMtx, this.parent.worldMtx, this.bindParentMtx);
            mat4.mul(this.worldMtx, this.worldMtx, this.localMtx);
        }        

        mat4.invert(this.worldTranformNormalMtx, this.worldMtx);
        mat4.transpose(this.worldTranformNormalMtx, this.worldTranformNormalMtx);
         
        this.worldPosition[0] = this.worldMtx[12];
        this.worldPosition[1] = this.worldMtx[13];
        this.worldPosition[2] = this.worldMtx[14];        

        if(this.component){
            this.component.onFrameUpdate();
        }
    }

    commitChanges(updateLocal?:boolean){
        if(updateLocal === true){
            this.updateLocalPose();
        }

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


    setUpdate(func:FrameUpdateFunc){
       return this.scene.addUpdate(elapsed=> func(elapsed, this));
    }

    translate(x:number, y:number, z:number){
        this.localTranslation[12] =x;
        this.localTranslation[13] =y;
        this.localTranslation[14] =z;

        this.localPosition[0] =x;
        this.localPosition[1] =y;
        this.localPosition[2] =z;        
    }

    move(direction:vec3){
        this.translate(
            this.localPosition[0]+direction[0],
            this.localPosition[1]+direction[1], 
            this.localPosition[2]+direction[2])
    }

    get position(){
        return this.localPosition;
    }

    set position(value:vec3){
        this.translate(value[0], value[1], value[2]);
    }

    positionX(x:number){
        this.translate(x, this.localPosition[1], this.localPosition[2]);
    }
    
    positionY(y:number){
        this.translate(this.localPosition[0], y, this.localPosition[2]);
    }
    
    positionZ(z:number){
        this.translate(this.localPosition[0], this.localPosition[1], z);
    }

    scale(x:number, y:number, z:number){
        this.localTranslation[0] +=x;
        this.localTranslation[5] +=y;
        this.localTranslation[10] +=z;
    }

    rotateX(rad:number){        
         mat4.rotateX(this.localRotation, this.localRotation, rad);         
    }
    
    rotateY(rad:number){       
         mat4.rotateY(this.localRotation, this.localRotation, rad);         
    }
    
    rotateZ(rad:number){        
         mat4.rotateZ(this.localRotation,this.localRotation, rad);         
    }

    rotate(axis:vec3, rad:number){        
         mat4.fromRotation(this.localRotation, rad, axis);         
    }

    apply(mat:mat4){
        mat4.mul(this.localMtx, mat,  this.localMtx);
    }

    get yaw(){
        this.updateAxis();
        return this.euler[0];  
    }

    get pitch(){
        this.updateAxis();
        return this.euler[1];  
    }

    get roll(){
        this.updateAxis();
        return this.euler[2];  
    }

    set yaw(rad:number){
      this.updateAxis();
      this.euler[0] = rad;

      matRotationYawPitchRoll(this.localRotation, this.euler[0], this.euler[1], this.euler[2]);       
    }

    set pitch(rad:number){
      this.updateAxis();
      
      this.euler[1] = rad;

      matRotationYawPitchRoll(this.localRotation, this.euler[0], this.euler[1], this.euler[2]);       
    }

    set roll(rad:number){
        this.updateAxis();

        this.euler[2] = rad;
  
        matRotationYawPitchRoll(this.localRotation, this.euler[0], this.euler[1], this.euler[2]);   
    }

    lookAt(target:vec3){
        mat4.lookAt(this.localMtx, this.worldPosition, target, UnitY);
        mat4.invert(this.localMtx, this.localMtx);

        getRotationAxis(this.up, this.front, this.right, this.localMtx); 
        eulerFromAxis(this.euler, this.right, this.up, this.front);
    }


    updateAxis(){
        getRotationAxis(this.up, this.front, this.right, this.localRotation); 
        eulerFromAxis(this.euler, this.right, this.up, this.front);

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
        this.camera.transform(this.frame);        
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
        context.setSource(Frame, this.frame);
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
        scene.lightsComponents.push(this);
        if(!scene.currentLight){
            scene.currentLight = this;
        }
    }


    onFrameUpdate(){
         transformNormal (this.worldDirection ,this.localDirection, this.frame.worldMtx);
         vec3.transformMat4(this.worldPosition, this.localPosition, this.frame.worldMtx);
         vec3.normalize(this.worldDirection, this.worldDirection);
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


