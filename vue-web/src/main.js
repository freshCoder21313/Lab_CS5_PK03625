import { createApp } from "vue";
import "bulma/css/bulma.min.css";

// Import Materialize CSS
import "materialize-css/dist/css/materialize.min.css";

// Import Materialize JavaScript
import "materialize-css/dist/js/materialize.min.js";

import App from "./App.vue";
import router from "./router";
import store from "./store";

const app = createApp(App);
app.use(store);
app.use(router);

app.mount("#app");

/*
<script>
import M from "materialize-css"; // Import Materialize JS

export default {
  name: "DropdownExample",
  mounted() {
    // Khởi tạo dropdown như sau
    const elems = document.querySelectorAll(".dropdown-trigger");
    M.Dropdown.init(elems);
  },
};
</script>
*/
