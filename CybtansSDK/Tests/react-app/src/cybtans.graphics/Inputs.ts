


export class MouseManager {
    down = false;
    /** 0 : Left mouse button
     *  1 : Wheel button or middle button (if present)
     *  2 : Right mouse button
     */
    mouseButton = -1;
    x = 0;
    y = 0;

    dx = 0;
    dy = 0;

    private clientX = 0;
    private clientY = 0;

    constructor(private canvas: HTMLCanvasElement) {
        canvas.addEventListener('mousedown', this.onMouseDown, true);
        canvas.addEventListener('mouseup', this.onMouseUp, true);
        canvas.addEventListener('mousemove', this.onMouseMove, true);
        canvas.addEventListener('mouseenter', this.onMouseEnter, true);
        canvas.addEventListener('mouseleave', this.onMouseLeave, true)
    }

    onMouseDown = (ev: MouseEvent) => {
        this.down = true;
        this.mouseButton = ev.button;
    }

    onMouseUp = (ev: MouseEvent) => {
        this.down = false;
        this.mouseButton = -1;
    }

    onMouseMove = (ev: MouseEvent) => {
        this.x = ev.clientX;
        this.y = ev.clientY;
    }

    onMouseEnter = (ev: MouseEvent) => {
        this.x = ev.clientX;
        this.y = ev.clientY;
        this.clientX = this.x;
        this.clientY = this.y;
        this.mouseButton = ev.button || -1;
    }

    onMouseLeave = (ev: MouseEvent) => {
        this.mouseButton = -1;
        this.x = 0;
        this.y = 0;
        this.clientX = 0;
        this.clientY = 0;
    }

    update(dt: number) {
        this.dx = this.clientX - this.x;
        this.dy = this.clientY - this.y;

        this.clientX = this.x;
        this.clientY = this.y;
    }
}

export class ScrollManager {
    top = 0;
    percent = 0
    movement = 0;
    height = 0;
    el: HTMLElement;
    lastTop = 0;

    constructor(el?: HTMLElement) {
        this.onScroll = this.onScroll.bind(this);
        this.setTarget(el);
    }

    setTarget(el?: HTMLElement) {
        if (this.el) {
            this.el.removeEventListener('scroll', this.onScroll);
        }

        if (el) {
            el.addEventListener('scroll', this.onScroll);
            this.el = el;
            this.height = el.scrollHeight;
            this.top = el.scrollTop;
        } else {
            window.addEventListener('scroll', this.onScroll);
            this.top = document.scrollingElement.scrollTop;
            this.height = document.scrollingElement.scrollHeight;
        }
        this.movement = 0;
        this.lastTop = this.top;

        this.percent = this.top / this.height;
    }

    onScroll() {
        if (this.el) {
            this.top = this.el.scrollTop;
            this.height = this.el.scrollHeight;
        } else {
            this.top = document.scrollingElement.scrollTop;
            this.height = document.scrollingElement.scrollHeight;
        }

        this.percent = this.top / this.height;
    }

    update(dt: number) {
        this.movement = this.top - this.lastTop;
        this.lastTop = this.top;
    }
}
