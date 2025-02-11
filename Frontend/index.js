const baseUrl = "https://localhost:7214/api/Products"

const CategoryEnum = Object.freeze({
    ELECTRONICS: "Electronics",
    CLOTHING: "Clothing",
    FOOD: "Food",
});

Vue.createApp({
    data() {
        return {
            products: [],       // Array to hold list of objects
            idToGetBy: -1,         // ID to retrieve a specific object
            product: null,      // Holds a single object
            deleteId: 0,           // ID of object to delete
            deleteMessage: "",     // Message to display after delete operation
            addData: {             // Data for adding a new object
                name: "", 
                serialnumber: 0, 
                category: 0,
                price: 0,
                inStock: true,

            },
            addMessage: "",        // Message to display after add operation
            categories: CategoryEnum // Store enum for easy access in UI
        }
    },
    methods: {
        // Get all objects
        getAllProducts() {
            this.getProducts(baseUrl)
        },

        // Helper function for GET requests, either all or by name
        async getProducts(url) {
            try {
                const response = await axios.get(url)
                this.products = await response.data
                console.log(this.products)
            } catch (ex) {
                console.log(ex.message);
                alert(ex.message) // Alerts if error occurs
            }
        },

        async deleteProduct(deleteId) {
            const url = baseUrl + "/" + deleteId
            try {
                response = await axios.delete(url)
                this.deleteMessage = response.status + " " + response.statusText
                this.getAllProducts() // Refresh the object list
            } catch (ex) {
                alert(ex.message)
            }
        },

        async addProduct() {
            try {
                response = await axios.post(baseUrl, this.addData)
                this.addMessage = "response " + response.status + " " + response.statusText
                this.getAllProducts() // Refresh the object list
            } catch (ex) {
                alert(ex.message)
            }
        },
    }
}).mount("#app")
