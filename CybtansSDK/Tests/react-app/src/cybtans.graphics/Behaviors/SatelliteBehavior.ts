import { mat4, vec3 } from "gl-matrix";
import { Behavior } from "./Behavior";
import Frame from "../Frame/Frame";
import { CameraComponent } from "../Frame/CameraComponent";
import { float3, matrix, toRadians } from "../MathUtils";
import Scene from "../Scene";
import { IAffector } from "../IAffector";

export default class SatelliteBehavior extends Behavior implements IAffector {
    private bindMtx: mat4 = matrix();
    private position: vec3 = float3();
    speed: number = toRadians(1);
    target: Frame;
    axis = float3([0, 1, 0]);
    worldMtx: mat4 = matrix();

    constructor(scene: Scene, frame: Frame | string, target: Frame | string) {
        super(scene, frame);

        if (typeof target === 'string') {
            this.target = scene.getNodeByName(target);
        } else {
            this.target = target;
        }

        // this.worldMtx = mat4.clone(this.target.worldMtx);
        // this.frame.attachAffector(this);
    }

    update(dt: number) {
        let frame = this.frame;
        let target = this.target;

        mat4.invert(this.bindMtx, target.worldMtx);

        let rot = dt * this.speed;
        mat4.fromRotation(this.worldMtx, rot, this.axis);

        mat4.mul(this.worldMtx, this.worldMtx, this.bindMtx);
        mat4.mul(this.worldMtx, target.worldMtx, this.worldMtx);

        //position = (RotRawPitchRoll *  bindMtx) * position  
        vec3.transformMat4(this.position, frame.worldPosition, this.worldMtx);

        //mat4.fromTranslation(this.worldMtx, this.position);
        frame.translate(this.position[0], this.position[1], this.position[2]);

        frame.commitChanges(true);
        //mat4.mul(frame.worldMtx, this.worldMtx, frame.worldMtx);

        super.update(dt);
    }
}