import { basename } from "path";
import { toRadians } from "../MathUtils";
import Scene from "../Scene";
import { Behavior } from "./Behavior";

export class FpsCameraController extends Behavior {
    speed = toRadians(0.5);

    constructor(scene: Scene, name: string) {
        super(scene, name);

    }

    update(dt: number) {
        if (this.sceneManager.mouse.mouseButton == 0) {
            let dx = this.sceneManager.mouse.dx;
            let dy = this.sceneManager.mouse.dy;

            this.frame.yaw += this.speed * dx;
            this.frame.pitch += this.speed * dy;
            this.frame.commitChanges(true);
        }
    }
}