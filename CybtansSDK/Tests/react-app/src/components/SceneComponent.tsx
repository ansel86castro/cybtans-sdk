
import * as React from 'react';
import SceneManager from '../cybtans.graphics/SceneManager';
import "./SceneComponent.css";

export default function SceneComponent(){

    let invalidated = React.useRef(true);

    let ref = React.createRef<HTMLCanvasElement>();    
    let sceneMgr = React.useRef<SceneManager>();

    React.useEffect(()=>{
        if(invalidated.current === true){                        
            if(ref.current){
                var canvas = ref.current;
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

    async function load() {
        if(!sceneMgr.current) return;
        
        await sceneMgr.current?.loadPrograms(`${process.env.REACT_APP_API_URL}/api/scene/programs`);
        
       let scene =  await sceneMgr.current?.loadScene(
        `${process.env.REACT_APP_API_URL}/api/scene/sample2`, 
        `${process.env.REACT_APP_API_URL}/api/scene/texture/sample2`);  

        scene.addNodeUpdate('Sphere', (e, node)=>{
            node.rotateY(Math.PI*0.0001 * e);
            node.commitChanges(true);
        });
        
        scene.addNodeUpdate('Cube', (e, node)=>{
            node.rotateY(Math.PI*0.0005 * e);
            node.commitChanges(true);
        });

    }  

    return (
         <canvas id="scene" ref={ref} width="800" height="600"></canvas>
    );
}
