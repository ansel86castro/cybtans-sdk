
import * as React from 'react';
import { SceneDto } from '../cybtans.graphics/models';
import ProgramRepository from '../cybtans.graphics/ProgramRepository';
import Scene from '../cybtans.graphics/Scene';
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
        
        await sceneMgr.current?.loadScene(
        `${process.env.REACT_APP_API_URL}/api/scene/sample1`, 
        `${process.env.REACT_APP_API_URL}/api/scene/texture/sample1`);  
    }  

    return (
         <canvas id="scene" ref={ref} width="800" height="600"></canvas>
    );
}
