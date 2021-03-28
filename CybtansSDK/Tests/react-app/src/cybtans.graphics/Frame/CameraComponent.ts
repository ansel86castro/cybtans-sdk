import Camera from "../Camera";
import Scene from "../Scene";
import { FrameComponent } from "./FrameComponent";
import Frame from "./Frame";



export class CameraComponent extends FrameComponent {
    constructor(public camera: Camera, frame: Frame) {
        super(frame);
    }


    onFrameUpdate() {
        this.camera.transform(this.frame);

        super.onFrameUpdate();
    }
}
