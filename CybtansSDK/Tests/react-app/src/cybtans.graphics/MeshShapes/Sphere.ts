import { vec3 } from "gl-matrix";
import { sphericalToCartesian } from "../MathUtils";
import Mesh from "../Mesh";
import { MeshPrimitive, MeshPartDto, MeshDto } from "../models";
import { uuidv4 } from "../utils";
import { MeshVertex, VertexDefinitions } from "./Vertex";



export function sphere(gl: WebGL2RenderingContext, stacks: number, slices: number, radius: number, name = 'sphere') {
    var vertices = new Array<MeshVertex>((stacks - 1) * (slices + 1) + 2);
    var indices = new Uint16Array((stacks - 2) * slices * 6 + slices * 6);

    var phiStep = Math.PI / stacks;
    var thetaStep = 2.0 * Math.PI / slices;
    // do not count the poles as rings
    var numRings = stacks - 1;

    // Compute vertices for each stack ring.
    var k = 0;
    for (var i = 1; i <= numRings; ++i) {
        var phi = i * phiStep;
        // vertices of ring
        for (var j = 0; j <= slices; ++j) {
            var theta = j * thetaStep;
            let pos = sphericalToCartesian(phi, theta, radius);
            var v: MeshVertex =
            {
                Pos: pos,
                Normal: vec3.normalize(vec3.create(), pos),
                TCoords: [-theta / (2.0 * Math.PI), phi / Math.PI]
            };
            // spherical to cartesian
            vertices[k++] = v;
        }
    }

    var northPoleIndex = vertices.length - 1;
    var southPoleIndex = vertices.length - 2;

    // poles: note that there will be texture coordinate distortion
    vertices[southPoleIndex] = {
        Pos: [0.0, -radius, 0.0],
        Normal: [0.0, -1.0, 0.0],
        TCoords: [0.0, 1.0]
    };

    vertices[northPoleIndex] = {
        Pos: [0.0, radius, 0.0],
        Normal: [0.0, 1.0, 0.0],
        TCoords: [0.0, 0.0]
    };

    var numRingVertices = slices + 1;

    // Compute indices for inner stacks (not connected to poles).
    k = 0;
    for (var i = 0; i < stacks - 2; ++i) {
        for (var j = 0; j < slices; ++j) {
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
    for (var i = 0; i < slices; ++i) {
        indices[k++] = i;
        indices[k++] = i + 1;
        indices[k++] = northPoleIndex;
    }

    // Compute indices for bottom stack.  The bottom stack was written
    // last to the vertex buffer, so we need to offset to the index
    // of first vertex in the last ring.
    var baseIndex = (numRings - 1) * numRingVertices;
    for (var i = 0; i < slices; ++i) {
        indices[k++] = baseIndex + (i + 1);
        indices[k++] = baseIndex + i;
        indices[k++] = southPoleIndex;
    }

    let vd = VertexDefinitions.PNTVertex;
    let vertexBufferSize = vertices.length * 8;

    let vertexfloatArray = new Float32Array(vertexBufferSize);
    let offset = 0;
    for (let i = 0; i < vertices.length; i++) {
        const v = vertices[i];

        vertexfloatArray[offset++] = v.Pos[0];
        vertexfloatArray[offset++] = v.Pos[1];
        vertexfloatArray[offset++] = v.Pos[2];

        vertexfloatArray[offset++] = v.Normal[0];
        vertexfloatArray[offset++] = v.Normal[1];
        vertexfloatArray[offset++] = v.Normal[2];

        vertexfloatArray[offset++] = v.TCoords[0];
        vertexfloatArray[offset++] = v.TCoords[1];
    }

    let mesh = new Mesh(gl, {
        id: uuidv4(),
        name: name,
        faceCount: indices.length / 3,
        sixteenBitsIndices: true,
        vertexCount: vertices.length,
        primitive: MeshPrimitive.triangleList,
        vertexDeclaration: vd,
        materialSlots: ['default'],
        layers: [{
            materialIndex: 0,
            vertexCount: vertices.length,
            indexCount: indices.length,
            layerId: 0,
            startIndex: 0,
            startVertex: 0,
            primitiveCount: indices.length / 3
        }]
    });

    mesh.setVertexBuffer(vertexfloatArray.buffer, vd);
    mesh.setIndexBuffer(indices.buffer, true);

    return mesh;
}