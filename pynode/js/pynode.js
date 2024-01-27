import { app } from "../../scripts/app.js";
import { api } from "../../scripts/api.js";

app.registerExtension({
    name: "MyComfyPlugin",

    async setup() {
        console.log("MyComfyPlugin setup")
    },
    async beforeRegisterNodeDef(nodeType, nodeData, app) {
        if(nodeData.name === "TextInput") {
            const origOnNodeCreated = nodeType.prototype.onNodeCreated;
            nodeType.prototype.onNodeCreated = function () {
                const r = origOnNodeCreated ? origOnNodeCreated.apply(this) : undefined;
                for(const w of this.widgets) {
                    if(w.name === "seed")
                    {
                        w.type = "converted-widget";
                        if(w.linkedWidgets) {
                            for(const lw of w.linkedWidgets) {
                                lw.type = "converted-widget";
                            }
                        }
                    }
                }
                return r;
            }
        }
    },
    async nodeCreated(node, app) {

    }

})