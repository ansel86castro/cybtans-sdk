import { vec4 } from "gl-matrix";
import { float4, Size } from "./MathUtils";
import Program from "./Program";
import EffectManager from "./EffectManager";
import Scene from "./Scene";
import EventEmitter from "./Events";
import { MouseManager, ScrollManager } from "./Inputs";

interface UpdateHandler {
    (elapsed: number): void;
}


export default class SceneManager {
    scenes: Scene[] = [];
    effects: EffectManager;
    current?: Scene;
    updateHandler?: UpdateHandler;
    elapsed: number = 0;
    gl: WebGL2RenderingContext;
    backColor: vec4 = float4([0.3, 0.3, 0.3, 1]);
    canvas: HTMLCanvasElement;
    private disposed: true;

    private lastTime?: number;
    private running: boolean = false;
    sizeChangedEmitter: EventEmitter<SceneManager, Size>;
    mouse: MouseManager;
    scroll?: ScrollManager;

    constructor(gl: WebGL2RenderingContext, canvas: HTMLCanvasElement) {
        this.effects = new EffectManager(gl);
        this.canvas = canvas;
        this.onFrame = this.onFrame.bind(this);
        this.gl = gl;
        this.sizeChangedEmitter = new EventEmitter();
        this.mouse = new MouseManager(canvas);

    }

    captureScroll(el?: HTMLElement) {
        this.scroll = new ScrollManager(el);
    }

    createScene() {
        let scene = new Scene(this, 'default_1', 'default');
        this.scenes.push(scene);
        if (this.scenes.length == 1) {
            this.current = scene;
        }
    }

    get width() {
        return this.canvas.width;
    }

    get height() {
        return this.canvas.height;
    }

    get program() {
        return this.effects.currentProgram;
    }

    set program(value: Program | undefined) {
        this.effects.currentProgram = value;
    }

    get IsDisposed() {
        return this.disposed;
    }

    onSizeChanged() {
        this.sizeChangedEmitter.raiseEvent(this, { width: this.width, height: this.height });
    }


    getProgramByType(type: string) {
        return this.effects.getProgramByType(type);
    }

    setSource(type: string, value: any) {
        this.effects.setSource(type, value);
    }

    getSource(type: string) {
        return this.effects.getSource(type);
    }

    private onFrame(timestamp: DOMHighResTimeStamp) {
        if (this.running === false) return;

        if (this.lastTime === undefined)
            this.lastTime = timestamp;

        this.elapsed = (timestamp - this.lastTime) / 1000;

        if (this.updateHandler) {
            this.updateHandler(this.elapsed);
        }

        this.mouse.update(this.elapsed);
        this.scroll?.update(this.elapsed);

        if (this.current) {
            this.current.update(this.elapsed);
        }

        this.render();

        this.lastTime = timestamp;

        requestAnimationFrame(this.onFrame);

    }

    private render() {
        const gl = this.gl;
        const color = this.backColor;

        gl.viewport(0, 0, this.width, this.height);
        gl.clearColor(color[0], color[1], color[2], color[3]);  // Clear to black, fully opaque
        gl.clearDepth(1.0);                 // Clear everything
        gl.enable(gl.DEPTH_TEST);           // Enable depth testing
        gl.depthFunc(gl.LEQUAL);            // Near things obscure far things

        // Clear the canvas before we start drawing on it.

        gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);

        //gl.disable(gl.CULL_FACE);
        // gl.cullFace(gl.BACK);

        if (this.disposed === true || this.current == null) return;

        this.current.render(this);
    }


    start() {
        this.running = true;
        requestAnimationFrame(this.onFrame);
    }

    stop() {
        this.running = false;
    }

    setCurrent(sceneName: string) {
        let scene = this.scenes.find(x => x.name === sceneName);
        if (scene) {
            this.current = scene;
        }
    }

    async loadScene(url: string, baseTextureUrl: string) {
        let options: RequestInit = { method: 'GET', headers: { Accept: 'application/json' } };
        var response = await fetch(url, options);

        let dto = await response.json();

        try {
            let scene = new Scene(this, dto.id, dto.name);
            scene.loadData(dto);
            this.scenes.push(scene);
            await scene.loadTextureFromUrl(baseTextureUrl);

            if (this.scenes.length == 1) {
                this.current = scene;
            }

            return scene;
        } catch (e) {
            console.error(e);
            throw e;
        }
    }

    async loadPrograms(url: string) {
        let options: RequestInit = { method: 'GET', headers: { Accept: 'application/json' } };
        var response = await fetch(url, options);

        let dto = await response.json();
        this.effects.setPrograms(dto);

    }

    dispose() {
        stop();

        for (const scene of this.scenes) {
            scene.dispose();
        }

        this.effects.dispose();

        this.disposed = true;
    }

}