module.exports = function (plop) {
  plop.setGenerator("file", {
    description:
      "Tạo file Vue component, constant, enum, model, plugin, util hoặc service",
    prompts: [
      {
        type: "list",
        name: "type",
        message: "Chọn loại file:",
        choices: [
          "component",
          "constant",
          "enum",
          "model",
          "plugin",
          "util",
          "service",
        ],
      },
      {
        type: "input",
        name: "name",
        message: "Nhập tên file (VD: components/bar → src/components/bar.vue):",
      },
    ],
    actions: (data) => {
      const basePaths = {
        component: "src",
        constant: "src",
        enum: "src",
        model: "src",
        plugin: "src",
        util: "src",
        service: "src",
      };

      const templates = {
        component: "plop-templates/Component.vue.hbs",
        constant: "plop-templates/Constant.js.hbs",
        enum: "plop-templates/Enum.js.hbs",
        model: "plop-templates/Model.js.hbs",
        plugin: "plop-templates/Plugin.js.hbs",
        util: "plop-templates/Util.js.hbs",
        service: "plop-templates/Service.js.hbs",
      };

      const pathParts = data.name.split("/");
      const fileName = pathParts.pop();
      const filePath = `${basePaths[data.type]}/${
        pathParts.length ? pathParts.join("/") + "/" : ""
      }${fileName}`;

      return [
        {
          type: "add",
          path: `${filePath}.${data.type === "component" ? "vue" : "js"}`,
          templateFile: templates[data.type],
        },
      ];
    },
  });
};
