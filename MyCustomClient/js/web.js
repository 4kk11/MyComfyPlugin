import { app } from "../../scripts/app.js";
import { api } from "../../scripts/api.js";

app.registerExtension({
    name: "MyCustomClient",

    async setup() {
        // queuePromptを呼び出すイベントリスナーを追加
        api.addEventListener("run_workflow", ({ detail }) => {
            app.queuePrompt(0);
        });

        // TextInputノードのテキストを更新するイベントリスナーを追加
        api.addEventListener("update_text", ({ detail }) => {
            const nodes = app.graph.findNodesByType("TextInput");
            for (const node of nodes) {
                const text = detail["text"];
                node.widgets[0].value = text;
            }
        });
    }
})
