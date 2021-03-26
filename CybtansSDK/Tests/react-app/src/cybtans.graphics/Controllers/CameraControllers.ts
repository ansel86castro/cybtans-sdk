import { mat4, vec3 } from "gl-matrix";
import Frame from "../Frame";
import { euler, eulerUtils, float3, matrix, matRotationYawPitchRoll, UnitY } from "../MathUtils";

export class CameraController{
    view:mat4;
    trans:mat4;
    euler:euler;
    rot:mat4;
    localMtx:mat4;
    displacement:vec3;
    frame:Frame;    

    constructor(frame:Frame){
        this.view = matrix();
        this.trans = matrix();
        this.euler =float3();
        this.rot = matrix();
        this.localMtx = matrix();
        this.displacement = float3();
        this.frame = frame;
    }

    update(position:vec3, dy:number, dx:number, elapsed?:number){      
        mat4.lookAt(this.view, this.frame.worldPosition, position, UnitY);         
        vec3.transformMat4(this.displacement, position, this.view);
        vec3.negate(this.displacement, this.displacement);
        mat4.fromTranslation(this.trans, this.displacement);

        this.euler[0]+=dy;
        this.euler[1]+=dx;

        //this.euler[0] = eulerUtils.normalizeHeading(this.euler[0]);
        //this.euler[1] = eulerUtils.normalizePith(this.euler[1]);

        //matRotationYawPitchRoll(this.rot, this.euler[0], this.euler[1], this.euler[2]);
         mat4.fromYRotation(this.rot ,this.euler[0]);

         mat4.mul(this.localMtx, this.trans, this.view);
         mat4.mul(this.localMtx, this.rot, this.localMtx);         
         
         vec3.negate(this.displacement, this.displacement);
         mat4.fromTranslation(this.trans, this.displacement);

         mat4.mul(this.localMtx, this.trans ,this.localMtx);

        mat4.invert(this.localMtx, this.localMtx);
        this.frame.localMtx = this.localMtx;
        this.frame.commitChanges();
    }
}