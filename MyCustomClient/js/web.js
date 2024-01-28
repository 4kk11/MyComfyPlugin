import { app } from "../../scripts/app.js";
import { api } from "../../scripts/api.js";

app.registerExtension({
    name: "MyCustomClient",

    async setup() {
        api.addEventListener("update_text", ({detail}) => {
            console.log("update_text", detail);
            const nodes = app.graph.findNodesByType("TextInput");
            for(const node of nodes) {
                const text = detail["text"];
                node.widgets[0].value = text;
            }
        });
    }
})