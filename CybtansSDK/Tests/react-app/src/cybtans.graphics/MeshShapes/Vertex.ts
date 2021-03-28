import { vec2, vec3 } from "gl-matrix";
import { VertexDefinitionDto, VertexElementDto, VertexElementFormat } from "../models";

export const Semantics = {
    Position: 'POSITION',
    Normal: 'NORMAL',
    Tangent: 'TANGENT',
    Color: 'COLOR',
    TextureCoordinate: 'TEXCOORD',
    PositionTransformed: 'POSITIONH',
    PointSize: 'POINT_SIZE',
    BlendIndices: 'BLEND_INDICES',
    BlendWeights: 'BLEND_WEIGHTS',
    OcclutionFactor: 'OCC_FACTOR'
};

export function createVertexDefinition(elements: VertexElementDto[] | null): VertexDefinitionDto {
    let size = 0;
    for (const item of elements) {
        item.offset = item.offset || size;
        size += sizeOfElement(item.format, item.size);
    }

    return {
        size,
        elements
    };
}



export const VertexDefinitions = {
    PNTVertex: createVertexDefinition([
        { semantic: Semantics.Position, format: VertexElementFormat.float, size: 3 },
        { semantic: Semantics.Normal, format: VertexElementFormat.float, size: 3 },
        { semantic: Semantics.TextureCoordinate, format: VertexElementFormat.float, size: 2 }
    ])
};

function sizeOfElement(type: VertexElementFormat, size: number) {
    switch (type) {
        case VertexElementFormat.int:
        case VertexElementFormat.float:
        case VertexElementFormat.uint:
            return 4 * size;

        case VertexElementFormat.ushort:
        case VertexElementFormat.short:
            return 2 * size;

        case VertexElementFormat.byte:
        case VertexElementFormat.ubyte:
            return size;
    }
    throw new Error("type not supported");
}


export interface MeshVertex {
    Pos: vec3;
    Normal: vec3;
    TCoords: vec2;
}
