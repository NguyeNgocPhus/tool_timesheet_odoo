
Vue.component("sm-datagrid-pager", {
    template: `
        <div>
            <div>
               nguyen ngoc phu
            </div>
        </div>
    `,

    props: {
        options: {
            type: Object,
            required: false,
            default: {}
        },

        datasource: {
            type: Object,
            required: false
        },

    },
    mounted() {
        console.log("options", this.options);
        console.log("datasource", this.datasource)
    },
    computed: {
        
    },

    methods: {
        
    }
});