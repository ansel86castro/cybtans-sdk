import { mat4, vec2, vec3 } from "gl-matrix";
import Frame from "./Frame/Frame";
import { float3, matrix } from "./MathUtils";
import { CameraDto, ProjectionType } from "./models";
import SceneManager from "./SceneManager";

export default class Camera {
    static readonly type = 'Camera';

    projType: ProjectionType;
    name?: string | null;
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
    viewInvertMtx: mat4;
    id: string;
    position: vec3;

    constructor(data: CameraDto) {
        this.projType = data.projType || ProjectionType.perspective;
        this.name = data.name;
        this.nearPlane = data.nearPlane || 0.1;
        this.farPlane = 1000;//data.farPlane || 1000;
        this.fieldOfView = data.fieldOfView;
        this.aspectRatio = data.aspectRatio;
        this.width = data.width;
        this.height = data.height;
        this.id = data.id;
        this.localMtx = matrix(data.localMatrix);
        this.viewMtx = matrix();
        this.projMtx = matrix();
        this.viewProjMtx = matrix();
        this.viewInvertMtx = matrix();
        this.position = float3();

        this.projMtx = mat4.perspective(this.projMtx, this.fieldOfView, this.width / this.height, this.nearPlane, this.farPlane);

        this.onViewUpdated();

    }

    registerForSizeChanged(sceneManger: SceneManager) {
        sceneManger.sizeChangedEmitter.addListener(size => {
            this.width = size.width;
            this.height = size.height;
            this.projMtx = mat4.perspective(this.projMtx, this.fieldOfView, this.width / this.height, this.nearPlane, this.farPlane);
            this.onViewUpdated();

        });
    }

    onViewUpdated() {
        mat4.mul(this.viewProjMtx, this.projMtx, this.viewMtx);
        mat4.invert(this.viewInvertMtx, this.viewMtx);

        this.position[0] = this.viewInvertMtx[12];
        this.position[1] = this.viewInvertMtx[13];
        this.position[2] = this.viewInvertMtx[14];
    }

    transform(frame: Frame) {
        /** view = inverse(transform * local) */
        mat4.mul(this.viewInvertMtx, frame.worldMtx, this.localMtx);

        mat4.invert(this.viewMtx, this.viewInvertMtx);

        mat4.mul(this.viewProjMtx, this.projMtx, this.viewMtx);

        this.position[0] = this.viewInvertMtx[12];
        this.position[1] = this.viewInvertMtx[13];
        this.position[2] = this.viewInvertMtx[14];
    }

}