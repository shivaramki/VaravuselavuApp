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
                            <b-col :md="2">
                                <h3>Users</h3>
                            </b-col>
                            <b-col :offset=8>
                                <b-button variant="primary" @click="handleAddUser">Add user</b-button>
                            </b-col>
                        </b-row>
                    </div>
                    <b-row>
                        <b-col>
                            <b-table striped hover bordered :items="userList" :fields="fields">
                                <template slot="actions" slot-scope="row">
                                    <b-button size="md" @click="handleEditUser(row)">
                                        Edit
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
                <b-pagination align="center" :total-rows="totalRows" v-model="currentPage" :per-page="5">
                </b-pagination>
            </b-col>
        </b-row>
        <user-modal :open-modal="showModal" :modal-entity="userEntity" :alert-args="pageAlertObj" :title="modalTitle" :show-spinner="isLoading" @saveEntity="saveEntity" @handleCancelModal="handleModalCancel"></user-modal>
    </div>
</template>

<script>
import UserModal from "../../src/components/user/SaveAndUpdateModal.vue";
import UserService from "../services/userservice.js";
export default {
  components: {
    UserModal
  },
  data: function() {
    return {
      fields: {
        Username: {
          label: "Name"
        },
        EmailId: {
          label: "Email"
        },
        AdvanceAmount: {
          label: "Advance Payed"
        },
        PhoneNumber: {
          label: "Contact No"
        },
        actions: {
          label: "Actions",
          thStyle: {
            width: "10%"
          }
        }
      },
      userList: [
        {
          Id: 0,
          Username: "",
          EmailId: "",
          AdvanceAmount: 0,
          PhoneNumber: "",
          AdvanceAmount: 0,
          IsAdmin: false
        }
      ],
      totalRows: 0,
      currentPage: 1,
      showModal: false,
      modalTitle: "",
      showAlertDesc: "Server issue,Please contact the administrator",
      showAlertTitle: "Errors",
      showAlertVisible: false,
      showAlertType: "danger",
      pageAlertObj: {
        alertDesc: [],
        alertTitle: "",
        alertVisible: false,
        alertType: ""
      },
      isLoading: false,
      userEntity: {
        Id: 0,
        Username: "",
        Password: "",
        EmailId: "",
        PhoneNumber: 0,
        AdvanceAmount: 0,
        IsAdmin: false
      }
    };
  },
  created: function() {
    this.loadUsers();
  },
  methods: {
    loadUsers: function() {
      UserService.getUsers().then(
        response => {
          if (response.body.success) {
            this.userList = response.body.data;
          } else {
            this.showAlertVisible = true;
          }
        },
        response => {
          this.handleServerError();
        }
      );
    },
    handleAddUser: function() {
      this.initialize();
      this.showModal = true;
      this.modalTitle = "New User";
    },
    initialize: function() {
      this.userEntity.Id = 0;
      this.userEntity.Username = "";
      this.userEntity.Password = "";
      this.userEntity.EmailId = "";
      this.userEntity.PhoneNumber = 0;
      this.userEntity.AdvanceAmount = 0;
      this.userEntity.IsAdmin = false;
    },
    handleEditUser: function(user) {
      this.modalTitle = "Edit user";
      this.userEntity.Id = user.item.Id;
      this.userEntity.Username = user.item.Username;
      this.userEntity.Password = user.item.Password;
      this.userEntity.EmailId = user.item.EmailId;
      this.userEntity.PhoneNumber = user.item.PhoneNumber;
      this.userEntity.AdvanceAmount = user.item.AdvanceAmount;
      this.userEntity.IsAdmin = user.item.IsAdmin;
      this.showModal = true;
    },
    handleServerError: function() {
      this.showAlertDesc = "Server issue,Please contact the administrator";
      this.showAlertTitle = "Errors";
      this.showAlertVisible = true;
      this.showAlertType = "danger";
    },
    saveEntity: function(user) {
      this.pageAlertObj.alertVisible = false;
      this.isLoading = true;
      if (user.Id == 0) {
        UserService.addUser(user).then(
          response => {
            if (response.body.success) {
              this.loadUsers(this.currentPage);
              this.pageAlertObj.Visible = false;
              this.isLoading = false;
              this.showModal = false;
              this.showAlertDesc = "User saved successfully.";
              this.showAlertTitle = "Alert";
              this.showAlertVisible = true;
              this.showAlertType = "success";
            } else {
              this.userEntity = user;
              this.pageAlertObj.alertDesc = response.body.error.join(", ");
              this.pageAlertObj.alertTitle = "Errors";
              this.pageAlertObj.alertVisible = true;
              this.pageAlertObj.alertType = "danger";
              this.isLoading = false;
            }
          },
          response => {
            this.handleServerError();
            this.isLoading = false;
          }
        );
      } else {
        UserService.editUser(user).then(
          response => {
            if (response.body.success) {
              this.loadUsers(this.currentPage);
              this.pageAlertObj.Visible = false;
              this.isLoading = false;
              this.showModal = false;
              this.showAlertDesc = "User saved successfully.";
              this.showAlertTitle = "Alert";
              this.showAlertVisible = true;
              this.showAlertType = "success";
            } else {
              this.userEntity = user;
              this.pageAlertObj.alertDesc = response.body.error.join(", ");
              this.pageAlertObj.alertTitle = "Errors";
              this.pageAlertObj.alertVisible = true;
              this.pageAlertObj.alertType = "danger";
              this.isLoading = false;
            }
          },
          response => {
            this.handleServerError();
            this.isLoading = false;
          }
        );
      }
    },
    handleModalCancel: function() {
      this.showModal = false;
    },
    closeAlert: function() {
      this.showAlertVisible = false;
    }
  }
};
</script>

