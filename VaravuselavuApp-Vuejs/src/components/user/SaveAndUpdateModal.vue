<template>
    <div>
        <b-modal hide-header-close no-close-on-esc no-close-on-backdrop size="md" v-model="modalShow" @ok="saveEntity" :title="title" @cancel="handleModalCancel">
            <loading :active.sync="isLoading"></loading>
            <b-alert :show="alertVisible" dismissible :variant="alertType" @dismissed="closeAlert">
                {{alertDesc}}
            </b-alert>
            <b-row :no-gutters="false">
                <b-col>
                    <label>
                        Username:
                    </label>
                </b-col>
            </b-row>
            <b-row>
                <b-col>
                    <b-form-input v-model="modalEntity.Username" length="35" type="text" placeholder="User name should not be empty"></b-form-input>
                </b-col>
            </b-row>
            <br>
            <b-row>
                <b-col>
                    <label>
                        Email id:
                    </label>
                </b-col>
            </b-row>
            <b-row>
                <b-col>
                    <b-form-input v-model="modalEntity.EmailId" type="text" placeholder="Email should not be empty"></b-form-input>
                </b-col>
            </b-row>
            <br>
            <b-row>
                <b-col>
                    <label>
                        Contact number
                    </label>
                </b-col>
            </b-row>
            <b-row>
                <b-col>
                    <b-form-input v-model="modalEntity.PhoneNumber" type="number" placeholder="Contact cannot be empty"></b-form-input>
                </b-col>
            </b-row>
            <br>
            <b-row>
                <b-col>
                    <label>
                        Advance amount:
                    </label>
                </b-col>
            </b-row>
            <b-row>
                <b-col>
                    <b-form-input v-model="modalEntity.AdvanceAmount" type="number" placeholder="Enter the amount"></b-form-input>
                </b-col>
            </b-row>
            <br>
            <b-row>
                <b-col>
                    <b-form-checkbox id="isAdmin" v-model="modalEntity.IsAdmin  " @change="handleIsAdminChanged">
                        Is Admin
                    </b-form-checkbox>
                </b-col>
            </b-row>
        </b-modal>
    </div>
</template>

<script>
import UserService from "./../../services/userservice.js";
import Loading from "vue-loading-overlay";
import "vue-loading-overlay/dist/vue-loading.min.css";
export default {
  props: ["openModal", "modalEntity", "title", "alertArgs", "showSpinner"],

  components: {
    Loading
  },

  watch: {
    openModal: function(newVal, oldVal) {
      if (newVal == true) {
        this.modalShow = true;
      } else {
        this.modalShow = false;
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
      modalShow: false,
      user: {
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

  methods: {
    handleIsAdminChanged: function(currentCheckState) {
      this.modalEntity.IsAdmin = currentCheckState;
    },
    handleModalCancel: function() {
      this.modalShow = false;
      this.alertVisible = false;
      this.$emit("handleCancelModal");
    },
    closeAlert: function() {
      this.alertVisible = false;
    },
    saveEntity: function(evt) {
      evt.preventDefault();
      this.$emit("saveEntity", this.modalEntity);
    }
  }
};
</script>

