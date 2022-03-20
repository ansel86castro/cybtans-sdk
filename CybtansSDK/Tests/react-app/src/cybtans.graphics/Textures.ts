import { URL } from "url";
import { CubeMapDto, TextureDto, TextureType } from "./models";
import Scene from "./Scene";
import { checkError } from "./utils";

export default abstract class Texture {

    id: string;
    protected type: TextureType;
    protected format?: string | null;
    protected glTexture: WebGLTexture | null = null;
    protected gl: WebGL2RenderingContext;

    constructor(gl: WebGL2RenderingContext, data: TextureDto) {
        this.type = data.type;
        this.format = data.format;
        this.id = data.id;
        this.gl = gl;
    }

    abstract load(baseUrl: string): Promise<void>;

    abstract setTexture(textureSlot: number): void;

    abstract disable(textureSlot: number): void;

    dispose() {
        if (this.glTexture) {
            this.gl.deleteTexture(this.glTexture);
            this.glTexture = null;
        }
    }
}

export class Texture2D extends Texture {
    private url?: string | null;

    constructor(gl: WebGL2RenderingContext, data: TextureDto) {
        super(gl, data);
        this.url = data.filename;
        this.type = TextureType.texture2d;
    }


    async load(baseUrl: string) {
        if (!this.url)
            throw new Error('Url not defined');

        if (!this.glTexture) {
            this.glTexture = this.gl.createTexture();

            if (!this.glTexture)
                throw new Error("Failed to create texture");
        }

        let gl = this.gl;
        let texture = this.glTexture;

        let imageUrl = `${baseUrl}/${this.url}`;
        //requestCORSIfNotSameOrigin(image, imageUrl);
        //image.src =imageUrl;

        let response = await fetch(imageUrl);
        let blob = await response.blob();
        let objectURL = window.URL.createObjectURL(blob);

        let image = new Image();
        image.onload = (e) => {
            window.URL.revokeObjectURL(objectURL);

            if (image.width == 0 || image.height == null) {
                console.error('Image failed to load');
                return;
            }

            try {
                //bind as texture 2d
                gl.bindTexture(gl.TEXTURE_2D, texture);

                //flip the image y axix
                gl.pixelStorei(gl.UNPACK_FLIP_Y_WEBGL, false);

                checkError(gl);

                gl.texImage2D(gl.TEXTURE_2D, 0, gl.RGB, gl.RGB, gl.UNSIGNED_BYTE, image);
                checkError(gl);

                gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_S, gl.CLAMP_TO_EDGE);
                gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_T, gl.CLAMP_TO_EDGE);

                if (isPowerOf2(image.width) && isPowerOf2(image.height)) {
                    gl.generateMipmap(gl.TEXTURE_2D);
                } else {
                    //set sampler states
                    gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER, gl.LINEAR);
                    gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MAG_FILTER, gl.LINEAR);
                }

                checkError(gl);
            } catch (e) {
                console.error(e);
            }
        };

        image.src = objectURL;
    }

    setTexture(textureSlot: number) {
        let gl = this.gl;
        if (!this.glTexture)
            throw new Error("Texture not created");

        gl.bindTexture(gl.TEXTURE_2D, this.glTexture);
    }

    disable(textureSlot: number): void {
        let gl = this.gl;

        this.gl.bindTexture(this.gl.TEXTURE_2D, null);
    }
}

export class TextureCube extends Texture {
    faces?: CubeMapDto;
    width: number;
    height: number;

    constructor(gl: WebGL2RenderingContext, data: TextureDto) {
        super(gl, data);

        this.faces = data.cubeMap;
        this.type = TextureType.textureCube;
    }

    setTexture(textureSlot: number) {
        let gl = this.gl;
        if (!this.glTexture)
            throw new Error("Texture not created");

        gl.bindTexture(gl.TEXTURE_CUBE_MAP, this.glTexture);
    }

    disable(textureSlot: number): void {
        this.gl.disable(this.gl.TEXTURE_CUBE_MAP);
    }

    async load(baseUrl: string) {
        if (!this.faces)
            throw new Error('Url not defined');

        if (!this.glTexture) {
            this.glTexture = this.gl.createTexture();

            if (!this.glTexture)
                throw new Error("Failed to create texture");
        }

        let gl = this.gl;

        await Promise.all([
            this.loadFace(baseUrl, this.faces.positiveX, gl.TEXTURE_CUBE_MAP_POSITIVE_X),
            this.loadFace(baseUrl, this.faces.negativeX, gl.TEXTURE_CUBE_MAP_NEGATIVE_X),

            this.loadFace(baseUrl, this.faces.positiveY, gl.TEXTURE_CUBE_MAP_POSITIVE_Y),
            this.loadFace(baseUrl, this.faces.negativeY, gl.TEXTURE_CUBE_MAP_NEGATIVE_Y),

            this.loadFace(baseUrl, this.faces.positiveZ, gl.TEXTURE_CUBE_MAP_POSITIVE_Z),
            this.loadFace(baseUrl, this.faces.negativeZ, gl.TEXTURE_CUBE_MAP_NEGATIVE_Z)
        ]);

        //bind as texture 2d
        gl.bindTexture(gl.TEXTURE_CUBE_MAP, this.glTexture);

        gl.texParameteri(gl.TEXTURE_CUBE_MAP, gl.TEXTURE_WRAP_S, gl.CLAMP_TO_EDGE);
        gl.texParameteri(gl.TEXTURE_CUBE_MAP, gl.TEXTURE_WRAP_T, gl.CLAMP_TO_EDGE);
        gl.texParameteri(gl.TEXTURE_CUBE_MAP, gl.TEXTURE_WRAP_R, gl.CLAMP_TO_EDGE);

        checkError(gl);

        // if (isPowerOf2(this.width) && isPowerOf2(this.height)) {
        //     gl.generateMipmap(gl.TEXTURE_CUBE_MAP);
        // }
        //set sampler states
        gl.texParameteri(gl.TEXTURE_CUBE_MAP, gl.TEXTURE_MIN_FILTER, gl.LINEAR);
        gl.texParameteri(gl.TEXTURE_CUBE_MAP, gl.TEXTURE_MAG_FILTER, gl.LINEAR);

        checkError(gl);
    }

    private async loadFace(baseUrl: string, url: string, target: number) {
        let gl = this.gl;
        let texture = this.glTexture;

        let imageUrl = `${baseUrl}/${url}`;
        let response = await fetch(imageUrl);
        let blob = await response.blob();
        let objectURL = window.URL.createObjectURL(blob);

        let image = new Image();
        image.onload = (e) => {
            window.URL.revokeObjectURL(objectURL);

            if (image.width == 0 || image.height == null) {
                console.error('Image failed to load');
                return;
            }

            this.width = image.width;
            this.height = image.height;

            try {
                //bind as texture 2d
                gl.bindTexture(gl.TEXTURE_CUBE_MAP, texture);

                //flip the image y axix
                gl.pixelStorei(gl.UNPACK_FLIP_Y_WEBGL, true);

                checkError(gl);

                gl.texImage2D(target, 0, gl.RGB, gl.RGB, gl.UNSIGNED_BYTE, image);
                checkError(gl);


            } catch (e) {
                console.error(e);
            }
        };

        image.src = objectURL;
    }
}

function isPowerOf2(value: number) {
    return (value & (value - 1)) == 0;
}

function requestCORSIfNotSameOrigin(img: HTMLImageElement, url: string) {
    if ((new URL(url, window.location.href)).origin !== window.location.origin) {
        img.crossOrigin = "";
    }
}

export function createTexture(gl: WebGL2RenderingContext, data: TextureDto): Texture {
    if (!data.type) throw new Error('texture type not defined');

    switch (data.type) {
        case TextureType.texture2d:
            return new Texture2D(gl, data);
        case TextureType.textureCube:
            return new TextureCube(gl, data);
        default:
            throw new Error('texture type not supported');
    }
}