
import { mat3, mat4, quat2, vec2, vec3 } from 'gl-matrix';
import * as React from 'react';
import { nodeModuleNameResolver } from 'typescript';
import { CameraController } from '../cybtans.graphics/Controllers/CameraControllers';
import Frame, { CameraComponent } from '../cybtans.graphics/Frame';
import { float3, toRadians, transformNormal } from '../cybtans.graphics/MathUtils';
import SceneManager from '../cybtans.graphics/SceneManager';
import "./SceneComponent.css";

let cameraType = 'fps';
let camera:Frame|null = null;
let cameraController:CameraController|null = null;
let keys: { [key:string]: boolean|undefined} = {};

export default function SceneComponent(){

    let invalidated = React.useRef(true);

    let ref = React.createRef<HTMLCanvasElement>();    
    let sceneMgr = React.useRef<SceneManager>();

    React.useEffect(()=>{
        if(invalidated.current === true){                        
            if(ref.current){
                var canvas = ref.current;
                canvas.addEventListener('keydown', keyDown, true);
                canvas.addEventListener('keyup', keyUp, true);

                let gl = canvas.getContext('webgl2');
               
                if(gl){                
                    console.log('WebGL enable');
                    sceneMgr.current = new SceneManager(gl, canvas.width, canvas.height);
                    sceneMgr.current.start();
                }
            }

            invalidated.current = false;            

            load();        
        }

        return ()=> {
            if(!ref.current){
                sceneMgr.current?.stop();
            }
        }
    });

    let capture = false;
    let x =0;
    let y =0;

    const target = float3([0, 0, 0]);
   
    function mouseDown(e:React.MouseEvent<HTMLCanvasElement, MouseEvent>){
        x= e.clientX;
        y = e.clientY;
       capture = true;              
    }
    function mouseUp(e:React.MouseEvent<HTMLCanvasElement, MouseEvent>){
        capture = false;
     }

    function mouseMove(e:React.MouseEvent<HTMLCanvasElement, MouseEvent>){
        if(capture === true){
            let dx = x - e.clientX;
            let dy = y - e.clientY;

            let scene = sceneMgr.current?.current;   
            if(!scene) return;
            
            let camera = scene.getNodeByName('Camera');
            if(camera){
                if(cameraType === 'fps'){
                    camera.yaw += toRadians(0.5) * dx;
                    camera.pitch += toRadians(0.5) * dy;
                    camera.commitChanges(true);        
                }else{
                    //camera.orbit(target, toRadians(1) * dx,  toRadians(1) * dy)
                    let heading =  toRadians(1) * dx;
                   // let pitch = toRadians(1) * dy;

                    if(cameraController){                        
                        cameraController.update(target, heading, 0);
                    }
                  
                }
            }
            x = e.clientX;
            y = e.clientY;
        }
    }

    function keyDown(e:KeyboardEvent):void{
       keys[e.key] = true;     
    }

    
    function keyUp(e:KeyboardEvent):void{
        keys[e.key] = false;
    }

    async function load() {
        if(!sceneMgr.current) return;
        
        await sceneMgr.current?.loadPrograms(`${process.env.REACT_APP_API_URL}/api/scene/programs`);
        
       let scene =  await sceneMgr.current?.loadScene(
        `${process.env.REACT_APP_API_URL}/api/scene/solar_system`, 
        `${process.env.REACT_APP_API_URL}/api/scene/texture/solar_system`);  

        camera = scene.getNodeByName('Camera');
        if(camera){
            let c = camera.component as CameraComponent;
            mat4.identity(c.camera.localMtx);
          
            camera.worldPosition = float3([0, 1, 5]);
            camera.lookAt(float3([0,0,0]));
            camera.updateTransforms();

            camera.commitChanges();
            cameraController = new CameraController(camera);

            let moveDirection = float3();
            let direction = float3();
            let speed = 2;

            scene.addUpdate(elapsed=>{
                if(!camera)
                    return;
                
                vec3.zero(moveDirection);

                if(true === keys['w']){
                    moveDirection[2]-= speed;
                }
                if(true === keys['s']){
                    moveDirection[2]+= speed;
                }
                if(true === keys['a']){
                    moveDirection[0]-=speed;
                }
                if(true === keys['d']){
                    moveDirection[0]+=speed;
                }
                if(true === keys['q']){
                    moveDirection[1]+=speed;
                }
                if(true === keys['e']){
                    moveDirection[1]-=speed;
                }

                transformNormal (direction, moveDirection, camera.localMtx);
                vec3.scale(direction, direction, elapsed);
                camera.move(direction);                                
                camera.commitChanges(true);

            });
        }
        scene.addNodeUpdate('Earth', (e, node)=>{
            node.rotateY(Math.PI*0.05 * e);        
            node.commitChanges(true);
        });
        
        // let m = mat4.create();
        // scene.addNodeUpdate('Cube', (e, node)=>{
        //     node.rotateY(Math.PI*0.0005 * e);
        //     node.updateLocalPose();

            
        //     node.apply(mat4.rotateY(m, m, Math.PI * 0.0003 * e));           
        //     node.commitChanges();
        // });


    }  

    return (
         <canvas id="scene" ref={ref} width="1024" height="768" 
            onMouseMove={mouseMove} 
            onMouseDown={mouseDown}
            onMouseUp={mouseUp}
            tabIndex={0}
           ></canvas>
    );
}
