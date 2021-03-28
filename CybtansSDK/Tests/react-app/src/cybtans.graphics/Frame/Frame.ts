import { glMatrix, mat3, mat4, quat, vec3, vec4 } from "gl-matrix";
import { euler, eulerFromAxis, float3, getRotationAxis, matrix, matRotationYawPitchRoll, matTranslate, UnitY } from "../MathUtils";
import { FrameDto, FrameType } from "../models";
import Scene from "../Scene";
import { decomposeMatrix } from "../MathUtils";
import { HashMap } from "../utils";
import { FrameComponent } from "./FrameComponent";
import { CameraComponent } from "./CameraComponent";
import { MeshComponent } from "./MeshComponent";
import { LightComponent } from "./LightComponent";
import { MeshSkinComponent } from "./MeshSkinComponent";
import { IAffector } from "../IAffector";

export type FrameUpdateFunc = (elapsed: number, frame: Frame) => void;

export default class Frame implements IAffector {
    id: string;
    name?: string | null;
    scene: Scene;

    //transforms
    localScale: mat4;
    localRotation: mat4;
    localTranslation: mat4;
    localMtx: mat4;
    bindParentMtx: mat4;
    bindAffectorMtx: mat4;
    worldMtx: mat4;
    worldTranformNormalMtx: mat4;

    parent?: Frame;
    affector?: IAffector;
    type: FrameType;
    range: number;
    childrens: Frame[] = [];
    private childresMap: HashMap<Frame | null> = {};
    bindTargetId?: string | null;
    tag?: string | null;
    component?: FrameComponent | null;

    up: vec3;
    front: vec3;
    right: vec3;
    euler: euler;
    private _worldPosition: vec3;
    private _localPosition: vec3;

    constructor(scene: Scene, data: FrameDto, parent?: Frame) {
        this.scene = scene;
        this.id = data.id;
        this.name = data.name;
        this.localMtx = matrix(data.localTransform);
        this.bindParentMtx = matrix(data.bindParentTransform);
        this.worldMtx = matrix(data.worldTransform);
        this.bindAffectorMtx = matrix(data.bindAffectorTransform);
        this.worldTranformNormalMtx = matrix();
        this.up = float3();
        this.front = float3();
        this.right = float3();
        this.euler = float3();
        this._worldPosition = float3();
        this._localPosition = float3();
        this.type = data.type;
        this.range = data.range;
        this.tag = data.tag;
        this.parent = parent;
        this.localScale = mat4.create();
        this.localTranslation = mat4.create();
        this.localRotation = mat4.create();

        this.updateTransforms();

        this._worldPosition = this._localPosition;

        mat4.invert(this.worldTranformNormalMtx, this.worldMtx);
        mat4.transpose(this.worldTranformNormalMtx, this.worldTranformNormalMtx);

        if (data.childrens) {
            for (const item of data.childrens) {
                let c = new Frame(scene, item, this);
                this.childrens?.push(c);
                if (c.name) {
                    this.childresMap[c.name] = c;
                }
            }
        }

        if (data.component) {
            if (data.component.camera) {
                this.component = new CameraComponent(scene.cameras.get(data.component.camera)!, this);
            } else if (data.component.light) {
                this.component = new LightComponent(scene.lights.find(x => x.id === data.component?.light?.light)!,
                    data.component.light, this);
            } else if (data.component.mesh) {
                let materials = data.component.mesh.materials!.map(x => scene.materials.get(x)!);
                let mesh = scene.meshes.get(data.component.mesh.mesh)!;
                this.component = new MeshComponent(mesh, materials, this);
            } else if (data.component.meshSkin) {
                let materials = data.component.meshSkin.materials!.map(x => scene.materials.get(x)!);
                let skin = scene.skins.get(data.component.meshSkin.meshSkin)!;
                this.component = new MeshSkinComponent(skin, materials, this);
            }
        }
    }


    //*************************** NODE MANAGEMENT *****************************/
    addNode(node: Frame) {
        node.parent = this;
        this.childrens.push(node);

        if (node.name)
            this.childresMap[node.name] = node;
    }

    removeNode(node: Frame) {
        let i = this.childrens.indexOf(node);
        if (i < 0) return;

        this.childrens.splice(i, 1);
        node.parent = undefined;

        if (node.name) {
            delete this.childresMap[node.name];
        }
    }

    getNodeByName(name: string): Frame | null {
        if (this.name == name)
            return this;

        let node = this.childresMap[name];
        if (node) return node;

        for (const item of this.childrens) {
            node = item.getNodeByName(name);
            if (node) {
                return node;
            }
        }

        return null;
    }

    forEach(action: (frame: Frame) => any): Frame | null {
        if (action(this) === true) return this;

        for (const item of this.childrens) {
            let frame = item.forEach(action);
            if (frame)
                return frame;
        }
        return null;
    }

    unwrap(): { [key: string]: Frame } {
        let map: { [key: string]: Frame } = {};
        this.forEach(x => map[x.id] = x);

        return map;
    }

    initialize(scene: Scene) {
        if (this.component) {
            this.component.initialize(scene);
        }

        for (const item of this.childrens) {
            item.initialize(scene);
        }
    }


    setUpdate(func: FrameUpdateFunc) {
        return this.scene.addUpdate(elapsed => func(elapsed, this));
    }

    //*************************** TRANSFORMS *****************************/    
    get position() {
        return this._localPosition;
    }

    set position(value: vec3) {
        let x = value[0], y = value[1], z = value[2];

        this.localTranslation[12] = x;
        this.localTranslation[13] = y;
        this.localTranslation[14] = z;

        this._localPosition[0] = x;
        this._localPosition[1] = y;
        this._localPosition[2] = z;
    }

    get worldPosition() {
        return this._worldPosition;
    }

    positionX(x: number) {
        this.translate(x, this._localPosition[1], this._localPosition[2]);
    }

    positionY(y: number) {
        this.translate(this._localPosition[0], y, this._localPosition[2]);
    }

    positionZ(z: number) {
        this.translate(this._localPosition[0], this._localPosition[1], z);
    }

    scale(x: number, y: number, z: number) {
        this.localTranslation[0] += x;
        this.localTranslation[5] += y;
        this.localTranslation[10] += z;
    }

    rotateX(rad: number) {
        mat4.rotateX(this.localRotation, this.localRotation, rad);
    }

    rotateY(rad: number) {
        mat4.rotateY(this.localRotation, this.localRotation, rad);
    }

    rotateZ(rad: number) {
        mat4.rotateZ(this.localRotation, this.localRotation, rad);
    }

    rotate(axis: vec3, rad: number) {
        mat4.fromRotation(this.localRotation, rad, axis);
    }

    apply(mat: mat4) {
        mat4.mul(this.localMtx, mat, this.localMtx);
    }

    get yaw() {
        this.updateAxis();
        return this.euler[0];
    }

    get pitch() {
        this.updateAxis();
        return this.euler[1];
    }

    get roll() {
        this.updateAxis();
        return this.euler[2];
    }

    set yaw(rad: number) {
        this.updateAxis();
        this.euler[0] = rad;

        matRotationYawPitchRoll(this.localRotation, this.euler[0], this.euler[1], this.euler[2]);
    }

    set pitch(rad: number) {
        this.updateAxis();

        this.euler[1] = rad;

        matRotationYawPitchRoll(this.localRotation, this.euler[0], this.euler[1], this.euler[2]);
    }

    set roll(rad: number) {
        this.updateAxis();

        this.euler[2] = rad;

        matRotationYawPitchRoll(this.localRotation, this.euler[0], this.euler[1], this.euler[2]);
    }

    translate(x: number, y: number, z: number) {
        this.localTranslation[12] = x;
        this.localTranslation[13] = y;
        this.localTranslation[14] = z;
        this.localTranslation[15] = 1;

        this._localPosition[0] = x;
        this._localPosition[1] = y;
        this._localPosition[2] = z;
    }

    move(direction: vec3) {
        this.translate(
            this._localPosition[0] + direction[0],
            this._localPosition[1] + direction[1],
            this._localPosition[2] + direction[2])
    }


    lookAt(target: vec3) {
        mat4.targetTo(this.localRotation, this._worldPosition, target, UnitY);
        this.localRotation[12] = 0;
        this.localRotation[13] = 0;
        this.localRotation[14] = 0;
    }

    private updateAxis() {
        getRotationAxis(this.up, this.front, this.right, this.localRotation);
        eulerFromAxis(this.euler, this.right, this.up, this.front);

    }

    updateTransforms() {
        let [scale, translation, rotation] = decomposeMatrix(this.localMtx);

        mat4.identity(this.localScale);
        mat4.scale(this.localScale, this.localScale, scale);

        mat4.identity(this.localTranslation);
        mat4.translate(this.localTranslation, this.localTranslation, translation);

        this.localRotation = rotation;
        getRotationAxis(this.up, this.front, this.right, this.localRotation);

        eulerFromAxis(this.euler, this.right, this.up, this.front);
        this._localPosition = translation;
    }

    updateLocalPose() {
        mat4.mul(this.localMtx, this.localRotation, this.localScale);
        mat4.mul(this.localMtx, this.localTranslation, this.localMtx);

        getRotationAxis(this.up, this.front, this.right, this.localRotation);
        eulerFromAxis(this.euler, this.right, this.up, this.front);

        this._localPosition[0] = this.localTranslation[12];
        this._localPosition[1] = this.localTranslation[13];
        this._localPosition[2] = this.localTranslation[14];
    }

    updateWorldPose() {
        // world =  transpose(localMtx * (bindParentMtx * parentMtx))
        if (!this.parent) {
            this.worldMtx = this.localMtx;
        } else {
            mat4.mul(this.worldMtx, this.parent.worldMtx, this.bindParentMtx);
            mat4.mul(this.worldMtx, this.worldMtx, this.localMtx);
        }

        if (this.affector && this.affector.worldMtx) {
            mat4.mul(this.worldMtx, this.bindAffectorMtx, this.worldMtx);
            mat4.mul(this.worldMtx, this.affector.worldMtx, this.worldMtx);
        }

        mat4.invert(this.worldTranformNormalMtx, this.worldMtx);
        mat4.transpose(this.worldTranformNormalMtx, this.worldTranformNormalMtx);

        this._worldPosition[0] = this.worldMtx[12];
        this._worldPosition[1] = this.worldMtx[13];
        this._worldPosition[2] = this.worldMtx[14];

        if (this.component) {
            this.component.onFrameUpdate();
        }
    }

    commitChanges(updateLocal?: boolean) {
        if (updateLocal === true) {
            this.updateLocalPose();
        }

        this.updateWorldPose();

        if (this.childrens) {
            for (const item of this.childrens) {
                item.commitChanges();
            }
        }
    }

    attachAffector(affector: IAffector) {
        this.affector = affector;
        mat4.invert(this.bindAffectorMtx, affector.worldMtx);
    }

    toString() {
        return this.name;
    }
}



