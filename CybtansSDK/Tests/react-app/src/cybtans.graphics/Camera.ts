import { mat4, vec2, vec3 } from "gl-matrix";
import { float3, matrix } from "./MathUtils";
import { CameraDto, ProjectionType } from "./models";

export default class Camera {    
    projType: ProjectionType;
    name?: string|null;
    nearPlane: number;
    farPlane: number;
    fieldOfView: number;
    aspectRatio: number;
    width: number;
    height: number;
    localMtx: mat4;
    viewMtx: mat4;
    projMtx: mat4;
    viewProjMtx: mat4;
    viewInvertMtx:mat4;
    id: string;
    position:vec3;
    
    constructor(data:CameraDto){
        this.projType = data.projType;
        this.name = data.name;
        this.nearPlane = data.nearPlane;
        this.farPlane = data.farPlane;
        this.fieldOfView = data.fieldOfView;
        this.aspectRatio = data.aspectRatio;
        this.width = data.width;
        this.height = data.height;
        this.id = data.id;
        this.localMtx = matrix(); //matrix(data.localMatrix);
        this.viewMtx =  matrix(data.viewMatrix);
        this.projMtx = matrix(data.projMatrix);;
        this.viewProjMtx = matrix();
        this.viewInvertMtx = matrix();
        this.position = float3();

        this.projMtx = mat4.perspective(this.projMtx, this.fieldOfView, this.width /this.height, this.nearPlane, this.farPlane);
    
       this.onViewUpdated();             

    }

    onViewUpdated(){
        mat4.mul(this.viewProjMtx, this.projMtx, this.viewMtx);
        mat4.invert(this.viewInvertMtx, this.viewMtx);

        this.position[0] = this.viewInvertMtx[12];
        this.position[1] = this.viewInvertMtx[13];
        this.position[2] = this.viewInvertMtx[14];
    }

    transform(transform: mat4) {
       //view =  transform * local
       
        // mat4.mul(this.viewInvertMtx,  transform, this.localMtx);
        // this.position[0] = this.viewMtx[12];
        // this.position[1] = this.viewMtx[13];
        // this.position[2] = this.viewMtx[14];

        // mat4.invert(this.viewMtx, this.viewInvertMtx);

        mat4.mul(this.viewProjMtx, this.projMtx, this.viewMtx);

    }

}