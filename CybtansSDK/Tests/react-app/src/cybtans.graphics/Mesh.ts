import { IndexBuffer, VertexBuffer } from "./Buffer";
import { MeshDto, MeshPartDto, MeshPrimitive, VertexDefinitionDto } from "./models";
import Scene from "./Scene";

export default class Mesh {
    id: string;
    materialSlots?: string[]|null;
    adjacency?: number[]|null;
    vertexCount: number;
    faceCount: number;
    vertexDeclaration?: VertexDefinitionDto|null;
    vertexBuffer?: VertexBuffer;
    indexBuffer?: IndexBuffer;
    primitive: MeshPrimitive;
    layers?: MeshPartDto[]|null;
    name?: string|null;
    sixteenBitsIndices: boolean;
    
    constructor(scene:Scene, data:MeshDto){
        this.id = data.id;
        this.name = data.name;
        this.materialSlots = data.materialSlots;
        this.adjacency = data.adjacency;
        this.vertexCount = data.vertexCount;
        this.faceCount = data.faceCount;
        this.vertexDeclaration = data.vertexDeclaration;
        this.primitive = data.primitive;
        this.layers = data.layers;
        this.sixteenBitsIndices = data.sixteenBitsIndices;
        
        if(data.vertexBuffer){
            this.vertexBuffer = new VertexBuffer(scene.gl, data.vertexDeclaration);
            this.vertexBuffer.setDataBase64(data.vertexBuffer);
        }

        if(data.indexBuffer){
            this.indexBuffer = new IndexBuffer(scene.gl);
            this.indexBuffer.setDataBase64(data.indexBuffer);
        }
        
    }
}