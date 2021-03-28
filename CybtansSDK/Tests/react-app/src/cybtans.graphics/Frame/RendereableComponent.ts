import { IRenderable } from "../Interfaces";
import SceneManager from "../SceneManager";
import { FrameComponent } from "./FrameComponent";
import Frame from "./Frame";

export type RenderCallback = (ctx: SceneManager) => void;

export abstract class RendereableComponent extends FrameComponent implements IRenderable {
    visible: boolean = true;
    onRenderBegin?: RenderCallback;
    onRenderEnd?: RenderCallback;

    constructor(frame: Frame) {
        super(frame);
    }

    render(ctx: SceneManager) {
        ctx.setSource('Frame', this.frame);

        if (this.onRenderBegin) {
            this.onRenderBegin(ctx);
        }

        this.onRender(ctx);

        if (this.onRenderEnd) {
            this.onRenderEnd(ctx);
        }
    }

    protected abstract onRender(ctx: SceneManager): void;
}
