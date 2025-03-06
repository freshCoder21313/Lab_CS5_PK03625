<template>
  <nav class="container-fluid bg-light py-2 border-bottom">
    <div class="container">
      <div class="row justify-content-between align-items-center">
        <!-- Các link chính -->
        <div class="col-auto">
          <router-link class="text-decoration-none text-dark mx-2" to="/"
            >Home</router-link
          >
          <span>|</span>
          <router-link class="text-decoration-none text-dark mx-2" to="/about"
            >About</router-link
          >
          <span>|</span>

          <!-- Dropdown Manager -->
          <div class="btn-group">
            <button
              type="button"
              class="btn btn-link text-dark dropdown-toggle text-decoration-none"
              data-bs-toggle="dropdown"
              aria-expanded="false"
            >
              Manager
            </button>
            <ul class="dropdown-menu">
              <li>
                <router-link class="dropdown-item" to="/manager/user"
                  >Manager User</router-link
                >
              </li>
              <li>
                <router-link class="dropdown-item" to="/manager/product"
                  >Manager Product</router-link
                >
              </li>
            </ul>
          </div>
        </div>
        <div class="col-auto">
          <b-button v-if="isLoggedIn" @click="handleLogout">Đăng xuất</b-button>
          <b-button v-else @click="openAuthModal">Đăng nhập</b-button>
        </div>
      </div>
    </div>
  </nav>

  <router-view class="container justify-content-center" />

  <AuthModal ref="authModal" />
</template>

<script>
import AuthModal from "@/components/AuthModal.vue";
import { isAccess, logout } from "@/services/access";

export default {
  components: {
    AuthModal,
  },
  data() {
    return {
      isLoggedIn: isAccess(),
    };
  },
  methods: {
    openAuthModal() {
      this.$refs.authModal.showModal = true;
    },
    async handleLogout() {
      await logout();
      this.isLoggedIn = false;
    },
  },
};
</script>

<!-- 
<div class="container">
  <h1 class="center-align">Welcome to MaterializeCSS with Vue</h1>
  <div class="row">
    <div class="col s12 m6">
      <div class="card">
        <div class="card-content">
          <span class="card-title">Card Title</span>
          <p>This is a simple card using MaterializeCSS.</p>
        </div>
        <div class="card-action">
          <a href="#">Learn More</a>
        </div>
      </div>
    </div>
  </div>
  <button class="btn waves-effect waves-light" type="submit" name="action">
    Submit
    <i class="material-icons right">send</i>
  </button>
</div>*/ 


<style lang="scss">
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}

nav {
  padding: 30px;

  a {
    font-weight: bold;
    color: #2c3e50;

    &.router-link-exact-active {
      color: #42b983;
    }
  }
}
</style>
-->
