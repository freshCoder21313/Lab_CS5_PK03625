<template>
  <nav class="container-fluid">
    <div class="container gap-4">
      <router-link to="/">Home</router-link> |
      <router-link to="/about">About</router-link>
    </div>
  </nav>
  <router-view class="container justify-content-center" />
</template>

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
