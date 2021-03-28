import { mat4 } from "gl-matrix";
import Frame from "./Frame/Frame";
import Mesh from "./Mesh";
import { MeshLayerBonesDto, MeshSkinDto } from "./models";
import Scene from "./Scene";


export default class MeshSkin {
    id: string;
    mesh: Mesh;
    bindShapeMatrix: mat4;
    boneBindingMatrices: mat4[] = [];
    layerBones?: MeshLayerBonesDto[] | null;
    bones?: Frame[];
    root?: Frame;

    constructor(scene: Scene, data: MeshSkinDto) {
        let mesh = scene.meshes?.get(data.mesh);
        if (!mesh) throw new Error(`Mesh not found ${data.mesh}`)

        this.id = data.id;
        this.mesh = mesh;
        this.bindShapeMatrix = data.bindShapeMatrix as mat4;
        this.layerBones = data.layerBones;

        if (data.boneBindingMatrices) {
            let n = data.boneBindingMatrices.length / 16;
            for (let i = 0; i < n; i++) {
                let matrix = data.boneBindingMatrices.slice(i * 16, 16);
                this.boneBindingMatrices.push(matrix as mat4);
            }
        }

        if (data.rootBone) {
            this.root = new Frame(scene, data.rootBone);
            let map = this.root.unwrap();
            this.bones = data.bones?.map(x => map[x]);
        }

    }
}