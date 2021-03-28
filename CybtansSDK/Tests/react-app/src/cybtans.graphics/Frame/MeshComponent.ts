import Material from "../Material";
import Mesh from "../Mesh";
import Scene from "../Scene";
import { IRenderable } from "../Interfaces";
import SceneManager from "../SceneManager";
import { FrameComponent } from "./FrameComponent";
import Frame from "./Frame";
import { RendereableComponent } from "./RendereableComponent";



export class MeshComponent extends RendereableComponent {
    mesh: Mesh;
    materials: Material[];
    constructor(mesh: Mesh, materials: Material[], frame: Frame) {
        super(frame);
        this.mesh = mesh;
        this.materials = materials;
    }

    initialize(scene: Scene) {
        scene.renderables.push(this);

        super.initialize(scene);
    }

    onRender(context: SceneManager): void {
        this.mesh.render(context, this.materials);
    }
}
