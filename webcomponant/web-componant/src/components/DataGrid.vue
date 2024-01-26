<template>
  <div></div>
</template>

<script>

export default {
  name: 'DataGrid',
  data(){
    return {
      dataTable: [
        { data: 'name', title: 'Name' },
        { data: 'position', title: 'Position' },
        { data: 'office', title: 'Office' },
        { data: 'extn', title: 'Extension' },
        { data: 'start_date', title: 'Start date' },
        { data: 'salary', title: 'Salary' },
      ],
      options :{
        responsive: true,
        select: true,
      }
    }
  },
  props: {
    dataSource: {
      type:Object,
      required: true
    }
  },
  mounted(){

  },
  methods:{

  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
h3 {
  margin: 40px 0 0;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  display: inline-block;
  margin: 0 10px;
}
a {
  color: #42b983;
}
</style>
