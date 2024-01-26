<template>
  <!-- <HelloWorld msg="Welcome to Your Vue.js App" /> -->
  <DataGrid msg="Welcome to Your Vue.js App" />
</template>

<script>
// import HelloWorld from './components/HelloWorld.vue'
import DataGrid from './components/DataGrid.vue'
export default {
  name: 'App',
  components: {
    // HelloWorld,
    DataGrid
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
