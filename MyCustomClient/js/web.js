import { app } from "../../scripts/app.js";
import { api } from "../../scripts/api.js";

app.registerExtension({
    name: "MyCustomClient",

    async setup() {
        api.addEventListener("run_workflow", ({ detail }) => {
            app.queuePrompt(0);
        });

        api.addEventListener("update_text", ({ detail }) => {
            const nodes = app.graph.findNodesByType("TextInput");
            for (const node of nodes) {
                const text = detail["text"];
                node.widgets[0].value = text;
            }
        });
    }
})