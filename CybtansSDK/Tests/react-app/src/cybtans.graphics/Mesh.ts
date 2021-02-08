import { vec2, vec3 } from "gl-matrix";
import { IndexBuffer, VertexBuffer } from "./Buffer";
import Material from "./Material";
import { sphericalToCartesian } from "./MathUtils";
import { MeshDto, MeshPartDto, MeshPrimitive, VertexDefinitionDto } from "./models";
import Scene from "./Scene";
import SceneManager from "./SceneManager";
import { checkError } from "./utils";

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
    layers: MeshPartDto[];
    name?: string|null;
    sixteenBitsIndices: boolean;
    
    shader:string = 'basic';

    constructor(scene:Scene, data:MeshDto){
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
        
        if(data.vertexBuffer){
            this.vertexBuffer = new VertexBuffer(scene.gl, data.vertexDeclaration);
            this.vertexBuffer.setDataBase64(data.vertexBuffer as any);
        }

        if(data.indexBuffer){
            this.indexBuffer = new IndexBuffer(scene.gl, data.sixteenBitsIndices);
            this.indexBuffer.setDataBase64(data.indexBuffer as any);
        }
        
    }

    render(ctx: SceneManager, materials: Material[]) {
        if(!ctx.program || !this.vertexBuffer || !this.indexBuffer)
            return;

        let program = ctx.program;
        let gl = program.gl;
        
        program.useProgram(ctx);

        this.vertexBuffer.setVertexBuffer(program);
        this.indexBuffer.setIndexBuffer();

        checkError(gl);
        
        for (const layer of this.layers) {
            let material = materials[layer.materialIndex];

            ctx.programSource(Material, material);

            program.bindSource(Material, ctx);

            gl.drawElements(gl.TRIANGLES, layer.indexCount, this.sixteenBitsIndices === true ? gl.UNSIGNED_SHORT: gl.UNSIGNED_INT, layer.startIndex);
          
            checkError(gl);
        }

        // this.vertexBuffer.clearVertexBuffer(program);
        // this.indexBuffer.clearIndexBuffer(program);
    }
}

interface MeshVertex {
    Pos:vec3;
    Normal:vec3;
    TCoords:vec2;
}

export function sphere(stacks:number , slices:number ,radius:number){
        var vertices =  new Array<MeshVertex>((stacks - 1) * (slices + 1) + 2);
        var indices = new Array((stacks - 2) * slices * 6 + slices * 6);

        var phiStep = Math.PI / stacks;
        var thetaStep = 2.0 * Math.PI / slices;
        // do not count the poles as rings
        var numRings = stacks - 1;

        // Compute vertices for each stack ring.
        var k = 0;
        for (var i = 1; i <= numRings; ++i){
                var phi = i * phiStep;
                // vertices of ring
                for (var j = 0; j <= slices; ++j){
                        var theta = j * thetaStep;
                        let pos = sphericalToCartesian(phi, theta, radius);
                        var v:MeshVertex = 
                        {  
                            Pos : pos,
                            Normal : vec3.normalize(vec3.create(), pos),
                            TCoords : [-theta / (2.0 * Math.PI), phi / Math.PI]
                        };
                        // spherical to cartesian
                        vertices[k++] = v;
                }
        }

        var northPoleIndex = vertices.length - 1;
        var southPoleIndex = vertices.length - 2;

        // poles: note that there will be texture coordinate distortion
        vertices[southPoleIndex] = {
            Pos : [0.0, -radius, 0.0],
            Normal : [0.0, -1.0, 0.0],
            TCoords : [0.0, 1.0]
        };
        
        vertices[northPoleIndex] = {
            Pos : [0.0, radius, 0.0],
            Normal : [0.0, 1.0, 0.0],
            TCoords : [0.0, 0.0]
        };
        
        var numRingVertices = slices + 1;

        // Compute indices for inner stacks (not connected to poles).
        k = 0;
        for (var i = 0; i < stacks - 2; ++i){
                for (var j = 0; j < slices; ++j){
                        indices[k++] = (i + 1) * numRingVertices + j;
                        indices[k++] = i * numRingVertices + j + 1;
                        indices[k++] = i * numRingVertices + j;

                        indices[k++] = (i + 1) * numRingVertices + j + 1;
                        indices[k++] = i * numRingVertices + j + 1;
                        indices[k++] = (i + 1) * numRingVertices + j;

                }
        }

        // Compute indices for top stack.  The top stack was written
        // first to the vertex buffer.
        for (var i = 0; i < slices; ++i)
        {
                indices[k++] = i;
                indices[k++] = i + 1;
                indices[k++] = northPoleIndex;
        }

        // Compute indices for bottom stack.  The bottom stack was written
        // last to the vertex buffer, so we need to offset to the index
        // of first vertex in the last ring.
        var baseIndex = (numRings - 1) * numRingVertices;
        for (var i = 0; i < slices; ++i)
        {
                indices[k++] = baseIndex + (i + 1);
                indices[k++] = baseIndex + i;
                indices[k++] = southPoleIndex;
        }

        let vertexSize = (12 + 12 + 8);
        let vertexBufferSize = vertices.length * vertexSize;
        let indexBufferSize = indices.length * 2;    
        
        let vertexfloatArray = new Float32Array(vertexBufferSize);
        for (let i = 0; i < vertices.length; i++) {
            const v = vertices[i];
            
            let offset = i*vertexSize; 
            vertexfloatArray[offset++] = v.Pos[0];
            vertexfloatArray[offset++] = v.Pos[1];
            vertexfloatArray[offset++] = v.Pos[2];

            vertexfloatArray[offset++] = v.Normal[0];
            vertexfloatArray[offset++] = v.Normal[1];
            vertexfloatArray[offset++] = v.Normal[2];

            vertexfloatArray[offset++] = v.TCoords[0];
            vertexfloatArray[offset] = v.TCoords[1];
        }

        
        
}