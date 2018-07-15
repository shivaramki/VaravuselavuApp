<template>
    <div>
        <b-row>
            <b-col :offset=1 :md=10>
                <b-alert :show="showAlertVisible" dismissible :variant="showAlertType" @dismissed="closeAlert">
                    {{showAlertDesc}}
                </b-alert>
                <b-card header-tag="header">
                    <div slot="header">
                        <b-row>
                            <b-col :md=2>
                                <h3>Categories</h3>
                            </b-col>
                            <b-col :offset=8>
                                <b-button variant="primary" @click="handleAddCategory">Add category</b-button>
                            </b-col>
                        </b-row>
                    </div>
                    <b-row>
                        <b-col>
                            <b-table striped hover bordered :items="categoryList" :fields="fields">
                                <template slot="actions" slot-scope="row">
                                    <b-button size="md" @click="handleEditCategory(row)">
                                        Edit
                                    </b-button>
                                    <b-button size="md" @click="handleDelete(row)">
                                        Delete
                                    </b-button>
                                </template>
                            </b-table>
                        </b-col>
                    </b-row>
                </b-card>
            </b-col>
        </b-row>
        <br>
        <b-row>
            <b-col>
                <b-pagination @input="handlePageChange" align="center" :total-rows="totalRows" v-model="currentPage" :per-page="5">
                </b-pagination>
            </b-col>
        </b-row>
        <category-modal :open-modal="openModal" :alert-args="pageAlertObj" :title="modalTitle" :modal-entity="category" @saveEntity="saveCategory" :show-spinner="isLoading" @handleCancelModal="handleModalCancel"></category-modal>
    </div>
</template>

<script>
import CategoryModal from "./../components/category/SaveAndUpdateModal.vue";
import CategoryService from "./../services/categoryservice.js";
import categoryservice from "./../services/categoryservice.js";
export default {
  components: {
    "category-modal": CategoryModal
  },
  data: function() {
    return {
      fields: {
        Name: {
          label: "Name",
          thStyle: {
            width: "85%"
          }
        },
        actions: {
          label: "Actions"
        }
      },
      categoryList: [{ Id: 0, Name: "" }],
      openModal: false,
      modalTitle: "",
      totalRows: 0,
      currentPage: 1,
      category: { Id: 0, Name: "" },
      pageAlertObj: {
        alertDesc: [],
        alertTitle: "",
        alertVisible: false,
        alertType: ""
      },
      showAlertDesc: "Internal server issue, please contact the administrator",
      showAlertTitle: "Errors",
      showAlertVisible: false,
      showAlertType: "danger",
      isLoading: false
    };
  },

  created: function() {
    this.loadCategory(this.currentPage);
  },
  methods: {
    handleAddCategory: function() {
      this.category.Name = "";
      this.category.Id = 0;
      this.modalTitle = "New Category";
      this.pageAlertObj.alertVisible = false;
      this.openModal = true;
    },
    handleEditCategory: function(category) {
      this.category.Id = category.item.Id;
      this.category.Name = category.item.Name;
      this.modalTitle = "Edit Category";
      this.openModal = true;
    },
    handleModalCancel: function() {
      this.openModal = false;
    },

    handlePageChange: function() {
      this.loadCategory(this.currentPage);
    },
    loadCategory: function(pageNo) {
      CategoryService.getCatgeoriesPaged(pageNo).then(
        response => {
          var result = response.body;
          if (result.success) {
            this.categoryList = response.body.data.categoryList;
            this.totalRows = response.body.data.count;
          } else {
            this.showAlertType = "danger";
            this.showAlertDesc = response.data.error.join();
            this.showAlertVisible = true;
          }
        },
        response => {
          this.showAlertVisible = true;
        }
      );
    },
    handleServerError: function() {
      this.showAlertDesc =
        "Internal server issue, please contact the administrator";
      this.showAlertTitle = "Errors";
      this.showAlertVisible = true;
      this.showAlertType = "danger";
    },
    closeAlert: function() {
      this.showAlertVisible = false;
    },
    handleDelete: function(category) {
      if (confirm("Are you sure want to delete this category ?")) {
        var id = category.item.Id;
        CategoryService.deleteCategory(id).then(
          response => {
            var result = response.body;
            if (result.success) {
              this.loadCategory(this.currentPage);
              this.showAlertDesc = category.item.Name + " is deleted from list";
              this.showAlertVisible = true;
              this.showAlertType = "success";
            } else {
              this.showAlertType = "danger";
              this.showAlertDesc = resposne.data.error.join();
              this.showAlertVisible = false;
            }
          },
          response => {
            this.handleServerError();
          }
        );
      }
    },
    saveCategory: function(category) {
      this.pageAlertObj.alertVisible = false;
      this.isLoading = true;
      if (category.Id == 0) {
        CategoryService.addCategory(category).then(
          response => {
            var result = response.body;
            if (result.success) {
              this.loadCategory(this.currentPage);
              this.pageAlertObj.Visible = false;
              this.openModal = false;
              this.isLoading = false;
            } else {
              this.category = category;
              this.pageAlertObj.alertDesc = response.body.error.join();
              this.pageAlertObj.alertTitle = "Errors";
              this.pageAlertObj.alertVisible = true;
              this.pageAlertObj.alertType = "danger";
              this.isLoading = false;
            }
          },
          response => {
            this.category = category;
            this.pageAlertObj.alertDesc =
              "Internal server issue, please contact the administrator";
            this.pageAlertObj.alertTitle = "Errors";
            this.pageAlertObj.alertVisible = true;
            this.pageAlertObj.alertType = "danger";
            this.isLoading = false;
          }
        );
      } else {
        CategoryService.editCategory(category).then(
          response => {
            var result = response.body;
            if (result.success) {
              this.loadCategory(this.currentPage);
              this.pageAlertObj.Visible = false;
              this.openModal = false;
              this.isLoading = false;
            } else {
              this.category = category;
              this.pageAlertObj.alertDesc = response.data.error.join();
              this.pageAlertObj.alertTitle = "Errors";
              this.pageAlertObj.alertVisible = true;
              this.pageAlertObj.alertType = "danger";
              this.isLoading = false;
            }
          },
          response => {
            this.category = category;
            this.pageAlertObj.alertDesc =
              "Internal server issue, please contact the administrator";
            this.pageAlertObj.alertTitle = "Errors";
            this.pageAlertObj.alertVisible = true;
            this.pageAlertObj.alertType = "danger";
            this.isLoading = false;
          }
        );
      }
    }
  }
};
</script>
<style scoped>
.custom-width {
  max-width: 800px;
}
</style>
