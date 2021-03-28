import Scene from "../Scene";
import Frame from "./Frame";


export class FrameComponent {
    frame: Frame;
    onInitialized?: (c: FrameComponent, scene: Scene) => void;
    onFrameUpdated?: (c: FrameComponent) => void;

    constructor(frame: Frame) {
        this.frame = frame;
    }

    initialize(scene: Scene) {
        if (this.onInitialized) this.onInitialized(this, scene);

    }

    onFrameUpdate() {
        if (this.onFrameUpdated) this.onFrameUpdated(this);
    }

    toString() {
        return this.frame.name;
    }
}
