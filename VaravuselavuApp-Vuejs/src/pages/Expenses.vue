<template>
    <div>
        <b-row>
            <b-col :offset="1" :md="10">
                <b-alert :show="showAlertVisible" dismissible :variant="showAlertType" @dismissed="closeAlert">
                    {{showAlertDesc}}
                </b-alert>
                <b-card header-tag="header">
                    <div slot="header">
                        <b-row>
                            <b-col :md=2>
                                <h3>Expenses</h3>
                            </b-col>
                            <b-col :offset=8>
                                <b-button variant="primary" @click="handleNewExpenses">Add expenses</b-button>
                            </b-col>
                        </b-row>
                    </div>
                    <b-row>
                        <b-col>
                            <b-table striped hover bordered :fields="fields" :items="expenseList">
                                <template slot="actions" slot-scope="row">
                                    <b-button size="md" @click="handleExpenseEdit(row)">
                                        Edit
                                    </b-button>
                                    <b-button size="md" @click="handleExpenseDelete(row)">
                                        Delete
                                    </b-button>
                                </template>
                                <template slot="ExpenseDate" slot-scope="row">
                                    {{moment(row.item.ExpenseDate)}}
                                </template>
                                <template slot="DivideAmongNames" slot-scope="row">
                                    <b>{{row.item.DivideAmongNames.join(",")}}  </b>
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
        <expense-modal :open-modal="openModal" :alert-args="pageAlertObj" :title="modalTitle" @saveEntity="saveEntity" :modal-entity="expense" @handleCancelModal="handleModalCancel" :showSpinner="isLoading"></expense-modal>
    </div>
</template>

<script>
import ExpenseModal from "./../components/expenses/SaveAndUpdateModal.vue";
import ExpenseService from "./../services/expenseservice.js";
import CategoryService from "./../services/categoryservice.js";
import UserService from "./../services/userservice.js";
import userservice from "./../services/userservice.js";
import moment from "moment";
export default {
  components: {
    "expense-modal": ExpenseModal
  },
  data: function() {
    return {
      showAlertDesc: "Internal server issue, please contact the administrator",
      showAlertTitle: "Errors",
      showAlertVisible: false,
      showAlertType: "danger",
      totalRows: 0,
      currentPage: 1,
      pageAlertObj: {
        alertDesc: "",
        alertTitle: "",
        alertVisible: false,
        alertType: ""
      },
      fields: {
        CategoryName: {
          label: "Category"
        },
        ExpenseDate: {
          label: "Expense Date"
        },
        UserName: {
          label: "Paid By"
        },
        DivideAmongNames: {
          label: "Divide Among"
        },
        Amount: {
          label: "Amount"
        },
        actions: {
          label: "Actions"
        }
      },
      expenseList: [],
      openModal: false,
      modalTitle: "",
      modalEntity: [],
      categoryName: "",
      paidByUserName: "",
      expense: {
        Id: 0,
        ExpenseDate: "",
        PaidBy: 0,
        Comment: "",
        DivideAmong: [],
        Amount: 0,
        Category: 0
      },
      isLoading:false
    };
  },
  created: function() {
    this.loadExpense(this.currentPage);
  },
  methods: {
    moment: function(date) {
      return moment(date).format("DD/MM/YYYY");
    },

    intialize: function() {
      this.expense.Id = 0;
      this.expense.ExpenseDate = "";
      this.expense.PaidBy = 0;
      this.expense.Comment = "";
      this.expense.DivideAmong = [];
      this.expense.Amount = 0;
      this.expense.Category = 0;
    },

    loadExpense: function(pageNo) {
      ExpenseService.getExpenses(pageNo).then(
        response => {
          var result = response.body;
          if (result.success) {
            this.expenseList = response.body.data.expenseList;
            this.totalRows = response.body.data.count;
          } else {
            this.showAlertVisible = true;
          }
        },
        response => {
          this.handleServerError();
        }
      );
    },
    handlePageChange: function() {
      this.loadExpense(this.currentPage);
    },
    saveEntity: function(expense) {
      this.pageAlertObj.alertVisible = false;
      this.isLoading = true;
      if (expense.Id == 0) {
        ExpenseService.addExpense(expense).then(
          response => {
            var result = response.body;
            if (result.success) {
              this.loadExpense(this.currentPage);
              this.pageAlertObj.Visible = false;
              this.openModal = false;
              this.showAlertDesc = "Expense saved successfully.";
              this.showAlertTitle = "Alert";
              this.showAlertVisible = true;
              this.showAlertType = "success";
               this.isLoading = false;
            } else {
              this.expense = expense;
              this.pageAlertObj.alertDesc = response.body.error.join(", ");
              this.pageAlertObj.alertTitle = "Errors";
              this.pageAlertObj.alertVisible = true;
              this.pageAlertObj.alertType = "danger";
               this.isLoading = false;
            }
          },
          response => {
            this.expense = expense;
            this.handleServerError();
             this.isLoading = false;
          }
        );
      } else {
        ExpenseService.editExpense(expense).then(
          response => {
            var result = response.body;
            if (result.success) {
              this.loadExpense(this.currentPage);
              this.pageAlertObj.Visible = false;
              this.openModal = false;
              this.showAlertDesc = "Expense updated successfully.";
              this.showAlertTitle = "Alert";
              this.showAlertVisible = true;
              this.showAlertType = "success";
               this.isLoading = false;
            } else {
              this.expense = expense;
              this.pageAlertObj.alertDesc = response.body.error.join(", ");
              this.pageAlertObj.alertTitle = "Errors";
              this.pageAlertObj.alertVisible = true;
              this.pageAlertObj.alertType = "danger";
               this.isLoading = false;
            }
          },
          response => {
            this.expense = expense;
            this.handleServerError();
             this.isLoading = false;
          }
        );
      }
    },
    handleServerError: function() {
      this.showAlertDesc = "Internal server issue,Please contact the administrator";
      this.showAlertTitle = "Errors";
      this.showAlertVisible = true;
      this.showAlertType = "danger";
    },
    handleNewExpenses: function() {
      this.modalTitle = "Add expenses";
      this.intialize();
      this.modalEntity = this.expense;
      this.pageAlertObj.alertVisible = false;
      this.openModal = true;
    },
    handleExpenseEdit: function(expenses) {
      this.modalTitle = "Edit expenses";
      this.expense = expenses.item;
      this.pageAlertObj.alertVisible = false;
      this.openModal = true;
    },
    handleExpenseDelete: function(expenses) {
      if (confirm("Are you sure want to delete this expense ?")) {
        this.expense = expenses.item;
        ExpenseService.deleteExpense(this.expense.Id).then(
          response => {
            var result = response.body;
            if (result.success) {
              this.showAlertDesc = "Expense is deleted successfully.";
              this.showAlertVisible = true;
              this.showAlertType = "success";
              this.loadExpense(this.currentPage);
            } else {
              this.showAlertVisible = true;
            }
          },
          response => {
            this.handleServerError();
          }
        );
      } else {
      }
    },
    handleModalCancel: function() {
      this.openModal = false;
    },
    closeAlert: function() {
      this.showAlertVisible = false;
    }
  }
};
</script>

