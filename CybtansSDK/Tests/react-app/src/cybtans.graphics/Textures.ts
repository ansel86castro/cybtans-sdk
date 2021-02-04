import { URL } from "url";
import { TextureDto, TextureType } from "./models";
import Scene from "./Scene";
import { checkError } from "./utils";

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

    async load(baseUrl:string){
        if(!this.url)
            throw new Error('Url not defined');

        if(!this.glTexture){
            this.glTexture = this.gl.createTexture();
            
            if(!this.glTexture)
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
        image.onload = (e)=>{
            window.URL.revokeObjectURL(objectURL);

            if(image.width == 0 || image.height == null){
                console.error('Image failed to load');
                return;
            }                
          
            try {                                       
                //bind as texture 2d
                gl.bindTexture(gl.TEXTURE_2D, texture);
               
                 //flip the image y axix
               //gl.pixelStorei(gl.UNPACK_FLIP_Y_WEBGL, true);              

                checkError(gl);

                gl.texImage2D(gl.TEXTURE_2D, 0, gl.RGB, gl.RGB, gl.UNSIGNED_BYTE, image);
                checkError(gl);

                gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_S, gl.TEXTURE_WRAP_S);
                gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_T, gl.TEXTURE_WRAP_T);

                if(isPowerOf2(image.width) && isPowerOf2(image.height)){
                    gl.generateMipmap(gl.TEXTURE_2D);
                }else{
                      //set sampler states
                    gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER, gl.LINEAR);
                    gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MAG_FILTER, gl.LINEAR);
                }

                checkError(gl);             
            }catch(e){
                console.error(e);
            }
        };         
        
        image.src = objectURL;      
    }

    setTexture(textureSlot:number){
        if(!this.glTexture)
            throw new Error("Texture not created");     

        //activate slot
        this.gl.activeTexture(this.gl.TEXTURE0 + textureSlot);

        switch(this.type){
            case TextureType.none: 
                return;
            case TextureType.texture2d:
                //bind as texture 2d to the slot number
                this.gl.bindTexture(this.gl.TEXTURE_2D, this.glTexture);
                break;
            case TextureType.texture3d:
                this.gl.bindTexture(this.gl.TEXTURE_3D, this.glTexture);
                break;
            case TextureType.cubeMap:
                this.gl.bindTexture(this.gl.TEXTURE_CUBE_MAP, this.glTexture);
                break;
        }
       
    }
}


function isPowerOf2(value:number) {
    return (value & (value - 1)) == 0;
}

function requestCORSIfNotSameOrigin(img:HTMLImageElement, url:string) {
    if ((new URL(url, window.location.href)).origin !== window.location.origin) {
      img.crossOrigin = "";
    }
  }