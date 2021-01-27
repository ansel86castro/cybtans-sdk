import { TextureDto, TextureType } from "./models";
import Scene from "./Scene";

export default class Texture {
    url?: string|null;
    type: TextureType;
    format?: string|null;
    id: string;
    glTexture:WebGLTexture |null = null;
    gl:WebGL2RenderingContext;

    constructor( scene:Scene, data:TextureDto){
        this.url = data.filename;
        this.type = data.type;
        this.format = data.format;
        this.id = data.id;
        this.gl = scene.gl;
    }

    load(){
        if(!this.url)
            return;

        if(!this.glTexture){
            this.glTexture = this.gl.createTexture();
            if(!this.glTexture)
                throw new Error("Failed to create texture");
        }

        return new Promise<boolean>((resolve, reject)=>{
            let image = new Image();
            let gl = this.gl;
            let texture = this.glTexture;
            image.onload = (e)=>{
                if(image.width == 0 || image.height == null){
                    reject('Image failed to load');
                    return;
                }

                //flip the image y axix
                gl.pixelStorei(gl.UNPACK_FLIP_Y_WEBGL, 1);
                
                //bind as texture 2d
                gl.bindTexture(gl.TEXTURE_2D, texture);

                //set sampler states
                gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_S, gl.CLAMP_TO_EDGE);
                gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_T, gl.CLAMP_TO_EDGE);
                gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER, gl.LINEAR);


                gl.texImage2D(gl.TEXTURE_2D, 0, gl.RGBA , gl.RGB, gl.UNSIGNED_BYTE, image);

                resolve(true);
            };
            

            image.src = this.url!;
        });
    }

    setTexture(textureSlot:number){
        if(!this.glTexture)
            throw new Error("Texture not created");           
        //activate slot
        this.gl.activeTexture(textureSlot);
         //bind as texture 2d to the slot number
        this.gl.bindTexture(this.gl.TEXTURE_2D, this.glTexture);

       // this.gl.uniform1i(sampler, slot);
    }
}