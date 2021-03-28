import Material from "../Material";
import Scene from "../Scene";
import MeshSkin from "../SkinMesh";
import { MeshComponent } from "./MeshComponent";
import Frame from "./Frame";



export class MeshSkinComponent extends MeshComponent {

    constructor(
        public skin: MeshSkin,
        public materials: Material[],
        frame: Frame) {
        super(skin.mesh, materials, frame);
    }
}
