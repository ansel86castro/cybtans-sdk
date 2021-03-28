import { quat, vec3 } from "gl-matrix";
import Frame from "../Frame/Frame";
import { float3, quaternionFromMat } from "../MathUtils";
import SceneManager from "../SceneManager";

interface AnimationChannel<T> {
    from: T;
    to: T;
}


export class TransformInterpolator {
    private translation: vec3;
    private rotQuat: quat = quat.create();
    private scale: vec3 = float3();

    private time = 0;

    enabled = false;
    duration = 0;
    transChannel?: AnimationChannel<vec3>;
    rotChannel?: AnimationChannel<quat>;
    scaleChannel?: AnimationChannel<vec3>;

    constructor(private frame: Frame, private sceneMgr: SceneManager) {
        this.capture();
    }

    capture() {
        this.translation = this.frame.position;
        quaternionFromMat(this.rotQuat, this.frame.localRotation);
        this.scale[0] = this.frame.localScale[0];
        this.scale[1] = this.frame.localScale[6];
        this.scale[2] = this.frame.localScale[10];

        this.start();
    }

    start() {
        this.time = 0;
    }

    update(dt: number) {
        if (this.enabled === false || this.duration <= 0)
            return;

        if (this.time > this.duration)
            return;

        this.time += dt;
        let s = this.time / this.duration;

        if (this.transChannel) {

        }
    }
}