import { IRenderable } from "../Interfaces";
import Mesh from "../Mesh";
import { sphere } from "../MeshShapes/Sphere";
import { CubeMapDto, TextureType } from "../models";
import SceneManager from "../SceneManager";
import Texture, { TextureCube } from "../Textures";

export class SkyBox implements IRenderable {
    static readonly type = 'SkyBox';

    visible = true;
    texture: Texture;
    mesh: Mesh;

    constructor(gl: WebGL2RenderingContext, cubeTexture: TextureCube) {
        this.texture = cubeTexture;
        this.mesh = sphere(gl, 16, 16, 1, 'skybox');
    }


    render(ctx: SceneManager) {
        let gl = ctx.gl;

        ctx.setSource(SkyBox.type, this);

        this.mesh.render(ctx, undefined, SkyBox.type);
    }

    static async create(gl: WebGL2RenderingContext, baseUrl: string, faces: CubeMapDto) {
        let cubeTexture = new TextureCube(gl, {
            type: TextureType.textureCube,
            id: 'skybox',
            format: 'rgb',
            cubeMap: faces
        });

        await cubeTexture.load(baseUrl);

        return new SkyBox(gl, cubeTexture);
    }
}