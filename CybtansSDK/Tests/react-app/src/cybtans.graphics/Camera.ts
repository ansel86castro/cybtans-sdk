import { mat4 } from "gl-matrix";
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
    localMatrix?: mat4;
    viewMatrix?: mat4;
    projMatrix?: mat4;
    viewProjMatrix?: mat4;
    id: string;
    
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
        this.localMatrix = data.localMatrix as mat4;
        this.viewMatrix = data.viewMatrix as  mat4;
        this.projMatrix = data.projMatrix as mat4;
        this.viewProjMatrix = mat4.create();
    
        mat4.mul(this.viewProjMatrix, this.localMatrix, this.viewMatrix);        
    }


}