
import { vec2, vec3 } from "gl-matrix";
import { IndexBuffer, VertexBuffer } from "./Buffer";
import Camera from "./Camera";
import Material from "./Material";
import { sphericalToCartesian } from "./MathUtils";
import { MeshDto, MeshPartDto, MeshPrimitive, VertexDefinitionDto } from "./models";
import Program from "./Program";
import Scene from "./Scene";
import SceneManager from "./SceneManager";
import { checkError } from "./utils";

export default class Mesh {
    static readonly type = 'Mesh';

    id: string;
    materialSlots?: string[] | null;
    adjacency?: number[] | null;
    vertexCount: number;
    faceCount: number;
    vertexDeclaration?: VertexDefinitionDto | null;
    vertexBuffer?: VertexBuffer;
    indexBuffer?: IndexBuffer;
    primitive: MeshPrimitive;
    layers: MeshPartDto[];
    name?: string | null;
    sixteenBitsIndices: boolean;

    shader: string = 'basic';
    gl: WebGL2RenderingContext;

    constructor(gl: WebGL2RenderingContext, data: MeshDto) {
        this.id = data.id;
        this.name = data.name;
        this.materialSlots = data.materialSlots;
        this.adjacency = data.adjacency;
        this.vertexCount = data.vertexCount;
        this.faceCount = data.faceCount;
        this.vertexDeclaration = data.vertexDeclaration;
        this.primitive = data.primitive;
        this.layers = data.layers || [];
        this.sixteenBitsIndices = data.sixteenBitsIndices;
        this.gl = gl;

        if (data.vertexBuffer) {
            this.vertexBuffer = new VertexBuffer(this.gl, data.vertexDeclaration);
            this.vertexBuffer.setDataBase64(data.vertexBuffer as any);
        }

        if (data.indexBuffer) {
            this.indexBuffer = new IndexBuffer(this.gl, data.sixteenBitsIndices);
            this.indexBuffer.setDataBase64(data.indexBuffer as any);
        }

    }

    setVertexBuffer(data: ArrayBuffer, vd: VertexDefinitionDto) {
        if (!this.vertexBuffer) {
            this.vertexBuffer = new VertexBuffer(this.gl, vd);
        }
        this.vertexBuffer.setData(data);
    }

    setIndexBuffer(data: ArrayBuffer, sixteenBitsIndices: boolean) {
        if (!this.indexBuffer) {
            this.indexBuffer = new IndexBuffer(this.gl, sixteenBitsIndices);
        }
        this.indexBuffer.setData(data);
    }

    render(ctx: SceneManager, materials?: Material[], renderType?: string) {
        if (!this.vertexBuffer || !this.indexBuffer)
            return;

        renderType = renderType || Mesh.type;
        let gl = ctx.gl;
        let currentProgram: Program | null = null;

        for (const layer of this.layers) {
            if (materials) {
                let material = materials[layer.materialIndex];
                ctx.setSource(Material.type, material);
            }

            const program = ctx.getProgramByType(renderType);

            if (program && program !== currentProgram) {
                ctx.program = program;

                program.useProgram(ctx);

                this.vertexBuffer.setVertexBuffer(program);
                this.indexBuffer.setIndexBuffer();
                currentProgram = program;

            } else if (currentProgram && materials) {
                currentProgram.bindSource(Material.type, ctx);
            }

            if (currentProgram) {
                gl.drawElements(gl.TRIANGLES, layer.indexCount, this.sixteenBitsIndices === true ? gl.UNSIGNED_SHORT : gl.UNSIGNED_INT, layer.startIndex);

                checkError(gl);

                currentProgram.clearSamplers(Material.type);
            }
        }
    }

    dispose() {
        this.vertexBuffer?.dispose();
        this.indexBuffer?.dispose();
    }

    toString() {
        return this.name;
    }
}
