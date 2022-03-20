import { mat4, vec3 } from "gl-matrix";
import { Behavior } from "./Behavior";
import Frame from "../Frame/Frame";
import { CameraComponent } from "../Frame/CameraComponent";
import { float3, matrix, toRadians } from "../MathUtils";
import Scene from "../Scene";

export default class CameraOrbitController extends Behavior {
    speed: number;
    target: Frame;
    bindMtx: mat4;
    transform: mat4;
    position: vec3;
    offset: number;
    updated: boolean;

    constructor(scene: Scene, cameraNode: Frame, targetNode: Frame, offset = 0, speed = 1) {
        super(scene, cameraNode);

        this.bindMtx = matrix();
        this.transform = matrix();
        this.position = float3();
        this.offset = offset;
        this.speed = toRadians(speed);
        this.target = targetNode;

        let cameraComponent = (cameraNode.component as CameraComponent);
        mat4.identity(cameraComponent.camera.localMtx);
        this.setTarget(targetNode);
    }

    setTarget(target: Frame) {
        this.target = target;
    }

    update(dt: number) {
        let frame = this.frame;
        let target = this.target;

        mat4.invert(this.bindMtx, target.worldMtx);

        let yaw = dt * this.speed;
        if (!this.updated) {
            yaw += this.offset;
            this.updated = true;
        }

        mat4.fromYRotation(this.transform, yaw);

        mat4.mul(this.transform, this.transform, this.bindMtx);
        mat4.mul(this.transform, target.worldMtx, this.transform);

        //position = (RotRawPitchRoll *  bindMtx) * position  
        vec3.transformMat4(this.position, frame.worldPosition, this.transform);


        //frame.lookAt(target.worldPosition);
        frame.translate(this.position[0], this.position[1], this.position[2]);
        frame.commitChanges(true);

        super.update(dt);
    }
}