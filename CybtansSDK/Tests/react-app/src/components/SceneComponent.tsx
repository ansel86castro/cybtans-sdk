import * as React from 'react';
import { SceneDto } from '../cybtans.graphics/models';
import ProgramRepository from '../cybtans.graphics/ProgramRepository';
import Scene from '../cybtans.graphics/Scene';

import "./SceneComponent.css";

export default function SceneComponent(){

    let [invalidated, setInvalidated] = React.useState(true);
    let [scene, setScene] = React.useState<Scene|undefined>();
    let [repo, setRepo] = React.useState<ProgramRepository|undefined>();

    var ref = React.createRef<HTMLCanvasElement>();
    var glRef = React.useRef<WebGL2RenderingContext>();

    React.useEffect(()=>{
        if(ref.current){
            var canvas = ref.current;
            let gl = canvas.getContext('webgl2');
            if(gl){
                glRef.current = gl;
                console.log('WebGL enable');
            }
        }
        if(invalidated === true){                        
            setInvalidated(false);
            
            loadScene();

            loadPrograms();
        }
        
    },[invalidated]);

    async function loadScene() {
        let baseUrl = process.env.REACT_APP_API_URL;
        let options:RequestInit = { method: 'GET', headers: { Accept: 'application/json' }};
        let endpoint = baseUrl+`/api/scene/sample1`;
        var response =  await  fetch(endpoint, options);
        
        let dto = await response.json();
        scene = new Scene(glRef.current!, dto);    
        setScene(scene);
    }

    async function loadPrograms(){
        let baseUrl = process.env.REACT_APP_API_URL;
        let options:RequestInit = { method: 'GET', headers: { Accept: 'application/json' }};
        let endpoint = baseUrl+`/api/scene/programs`;
        var response =  await  fetch(endpoint, options);
        let dto = await response.json();
        repo = new ProgramRepository(glRef.current!, dto);        
    }

    return (
         <canvas id="scene" ref={ref} width="1024" height="600"></canvas>
    );
}
