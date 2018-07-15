<template>
    <div>
        <b-modal hide-header-close no-close-on-esc no-close-on-backdrop size="lg" v-model="modalVisible" @ok="saveEntity" :title="title" @cancel="handleModalCancel">
            <loading :active.sync="isLoading"></loading>
            <b-alert :show="alertVisible" dismissible :variant="alertType" @dismissed="closeAlert">
                {{alertDesc}}
            </b-alert>
            <b-row>
                <b-col>
                    <label><b>Category:</b></label>
                </b-col>
                <b-col>
                    <label><b>Expense Date:</b></label>
                </b-col>
            </b-row>
            <b-row>
                <b-col>
                    <b-form-select v-model="categorySelected" @change="handleCategoryChanged" :options="categoryList" />
                </b-col>
                <b-col>
                    <date-picker v-model="expense.ExpenseDate" :config="config"></date-picker>
                </b-col>
            </b-row>
            <br>
            <b-row>
                <b-col>
                    <label><b>Paid by:</b></label>
                </b-col>
                <b-col>
                    <label><b>Amount:</b></label>
                </b-col>
            </b-row>
            <b-row>
                <b-col>
                    <b-form-select v-model="paidBySelected" @change="handlePaidByUserChanged" :options="paidByOptions" />
                </b-col>
                <b-col>
                    <b-form-input v-model="expense.Amount" type="number" placeholder="Enter the amount"></b-form-input>
                </b-col>
            </b-row>
            <br>
            <b-row>
                <b-col>
                    <label><b>Split with</b></label>
                </b-col>
            </b-row>
            <b-row>
                <b-col>
                    <multi-select v-model="divideAmongValue" @input="handleDivideAmong" track-by="text" :options="divideAmong" :multiple="true" :custom-label="handleCustomMultiVue"></multi-select>
                </b-col>
            </b-row>
            <br>
            <b-row>
                <b-col>
                    <label><b>Comment:</b></label>
                </b-col>
            </b-row>
            <b-row>
                <b-col>
                    <b-form-textarea v-model="expense.Comment">

                    </b-form-textarea>
                </b-col>
            </b-row>
        </b-modal>
    </div>
</template>

<script>
import "bootstrap/dist/css/bootstrap.css";
import datePicker from "vue-bootstrap-datetimepicker";
import "eonasdan-bootstrap-datetimepicker/build/css/bootstrap-datetimepicker.css";
import Multiselect from "vue-multiselect";
import CategoryService from "./../../services/categoryservice.js";
import UserService from "./../../services/userservice.js";
import moment from "moment";
import Loading from "vue-loading-overlay";
import "vue-loading-overlay/dist/vue-loading.min.css";

export default {
  components: {
    "date-picker": datePicker,
    "multi-select": Multiselect,
    Loading
  },

  props: ["openModal", "modalEntity", "title", "alertArgs", "showSpinner"],

  watch: {
    openModal: function(newVal, oldVal) {
      if (newVal == true) {
        this.modalVisible = true;
        this.loadModal(this.modalEntity);
        this.loadDivideAmong();
      } else {
        this.modalVisible = false;
      }
    },
    showSpinner: function(newVal, oldVal) {
      this.isLoading = this.showSpinner;
    },
    alertArgs: {
      handler: function(newVal) {
        (this.alertDesc = this.alertArgs.alertDesc),
          (this.alertTitle = this.alertArgs.alertTitle),
          (this.alertVisible = this.alertArgs.alertVisible),
          (this.alertType = this.alertArgs.alertType);
        this.inputValue = this.modalEntity.Name;
      },
      deep: true
    }
  },
  data: function() {
    return {
      isLoading: false,
      alertDesc: "",
      alertTitle: "",
      alertVisible: false,
      alertType: "",
      categorySelected: null,
      paidBySelected: null,
      tempPaidBySelected: null,
      expense: {
        Id: 0,
        ExpenseDate: "",
        PaidBy: 0,
        Comment: "",
        DivideAmong: [],
        Amount: 0,
        Category: 0
      },
      paidByOptions: [{ value: null, text: "Select the user" }],
      dummyPaidByOptions: [{ value: null, text: "Select the user" }],
      splitWithSelected: [],
      categoryList: [{ value: null, text: "Select the category" }],
      modalVisible: false,
      date: new Date(),
      config: {
        format: "DD/MM/YYYY",
        useCurrent: true
      },
      divideAmongValue: [],
      divideAmong: []
    };
  },
  created: function() {
    this.loadCategory();
    this.loadUsers();
    this.loadDivideAmong();
  },
  methods: {
    loadDivideAmong: function() {
      UserService.getUsers().then(
        response => {
          var result = response.body;
          if (result.success) {
            this.dummyPaidByOptions = response.body.data.map(item => ({
              value: item.Id,
              text: item.Username
            }));
            this.divideAmong = this.dummyPaidByOptions;
            this.divideAmongValue = this.dummyPaidByOptions;
            if (this.modalEntity.Id > 0) {
              for (
                var i = 0;
                i < this.modalEntity.DivideAmong.length - 1;
                i++
              ) {
                if (
                  this.modalEntity.DivideAmong[i] !=
                  this.divideAmongValue[i].value
                ) {
                  this.divideAmongValue.splice(i, 1);
                }
              }
            }
          } else {
            this.alertDesc = response.data.error;
            this.alertVisible = true;
            this.alertType = "danger";
          }
        },
        response => {
          {
            this.handleServerError();
          }
        }
      );
    },
    moment: function(date) {
      return moment(date).format("DD/MM/YYYY");
    },
    handleModalCancel: function() {
      this.modalShow = false;
      this.alertVisible = false;
      this.inputValue = "";
      this.$emit("handleCancelModal");
    },
    loadModal: function() {
      if (this.modalEntity.Id == 0) {
        this.expense.Id = this.modalEntity.Id;
        this.expense.ExpenseDate = this.modalEntity.ExpenseDate;
        this.expense.PaidBy = this.modalEntity.PaidBy;
        this.expense.Comment = this.modalEntity.Comment;
        this.expense.DivideAmong = this.modalEntity.DivideAmong;
        this.expense.Amount = this.modalEntity.Amount;
        this.expense.Category = this.modalEntity.Category;
        this.categorySelected = null;
        this.paidBySelected = this.tempPaidBySelected;
      } else {
        this.expense.Id = this.modalEntity.Id;
        this.expense.ExpenseDate = this.moment(this.modalEntity.ExpenseDate);
        this.expense.PaidBy = this.modalEntity.PaidBy;
        this.expense.Comment = this.modalEntity.Comment;
        this.expense.DivideAmong = this.modalEntity.DivideAmong;
        this.expense.Amount = this.modalEntity.Amount;
        this.expense.Category = this.modalEntity.CategoryId;
        this.categorySelected = this.modalEntity.CategoryId;
        this.paidBySelected = this.modalEntity.PaidBy;
      }
    },
    handleModalCancel: function() {
      this.modalVisible = false;
      this.$emit("handleCancelModal");
    },
    handleCustomMultiVue: function(option) {
      return `${option.text}`;
    },
    loadCategory: function() {
      CategoryService.getCategories().then(
        response => {
          var result = response.body;
          if (result.success) {
            this.categoryList = response.body.data.map(item => ({
              value: item.Id,
              text: item.Name
            }));
            this.categoryList.push({
              value: null,
              text: "Select the category"
            });
            this.categorySelected = null;
          } else {
            this.alertDesc = response.data.error;
            this.alertVisible = true;
            this.alertType = "danger";
          }
        },
        response => {
          {
            this.handleServerError();
          }
        }
      );
    },
    loadUsers: function() {
      UserService.getUsers().then(
        response => {
          var result = response.body;
          if (result.success) {
            this.paidByOptions = response.body.data.map(item => ({
              value: item.Id,
              text: item.Username
            }));
            this.tempPaidBySelected = response.body.loggedInId;
            this.paidBySelected = this.tempPaidBySelected;
          } else {
            this.alertDesc = response.data.error;
            this.alertVisible = true;
            this.alertType = "danger";
          }
        },
        response => {
          {
            this.handleServerError();
          }
        }
      );
    },
    handleServerError: function() {
      this.alertDesc = "Server issue,Please contact the administrator";
      this.alertTitle = "Errors";
      this.alertVisible = true;
      this.alertType = "danger";
    },
    closeAlert: function() {
      this.alertVisible = false;
    },
    saveEntity: function(evt) {
      evt.preventDefault();
      this.expense.PaidBy = this.paidBySelected;
      if (this.modalEntity.Id == 0) this.handleDivideAmong(this.paidByOptions);
      this.$emit("saveEntity", this.expense);
    },
    handleCategoryChanged: function(options) {
      this.expense.Category = options;
    },
    handlePaidByUserChanged: function() {
      this.expense.PaidBy = this.paidBySelected;
    },
    handleDivideAmong: function(options) {
      this.expense.DivideAmong = [];
      for (var i = 0; i < options.length; i++) {
        this.expense.DivideAmong.push(options[i].value);
      }
    }
  }
};
</script>
<style src="vue-multiselect/dist/vue-multiselect.min.css"></style>


