import { vec3 } from "gl-matrix";
import Light from "../Light";
import { float3, transformNormal } from "../MathUtils";
import { FrameLightDto } from "../models";
import Scene from "../Scene";
import { FrameComponent } from "./FrameComponent";
import Frame from "./Frame";



export class LightComponent extends FrameComponent {
    static readonly type = 'LightComponent';


    localPosition: vec3;
    localDirection: vec3;
    worldPosition: vec3;
    worldDirection: vec3;

    constructor(
        public light: Light,
        data: FrameLightDto,
        frame: Frame
    ) {
        super(frame);

        this.localDirection = float3(data.localDirection);
        this.localPosition = float3(data.localPosition);
        this.worldDirection = vec3.clone(this.localDirection);
        this.worldPosition = vec3.clone(this.localPosition);
    }

    initialize(scene: Scene) {
        scene.lightsComponents.push(this);
        if (!scene.currentLight) {
            scene.currentLight = this;
        }
        super.initialize(scene);
    }


    onFrameUpdate() {
        transformNormal(this.worldDirection, this.localDirection, this.frame.worldMtx);
        vec3.transformMat4(this.worldPosition, this.localPosition, this.frame.worldMtx);
        vec3.normalize(this.worldDirection, this.worldDirection);

        super.onFrameUpdate();
    }
}
