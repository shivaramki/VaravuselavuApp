<template>
    <div>
        <b-modal hide-header-close no-close-on-esc no-close-on-backdrop size="md" v-model="modalShow" :title="title" @cancel="handleModalCancel" @ok="saveEntity">
            <loading :active.sync="isLoading"></loading>
            <b-alert :show="alertVisible" dismissible :variant="alertType" @dismissed="closeAlert">
                {{alertDesc}}
            </b-alert>
            <b-input-group prepend="Name">
                <b-form-input v-model="inputValue">
                </b-form-input>
            </b-input-group>
        </b-modal>
    </div>
</template>

<script>
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
        this.inputValue = this.modalEntity.Name;
        this.entityTitle = this.title;
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
      modalShow: false,
      inputValue: "",
      entityTitle: "",
      alertDesc: "",
      alertTitle: "",
      alertVisible: false,
      alertType: ""
    };
  },
  methods: {
    handleModalCancel: function() {
      this.modalShow = false;
      this.alertVisible = false;
      this.inputValue = "";
      this.$emit("handleCancelModal");
    },

    saveEntity: function(evt) {
      evt.preventDefault();
      if (this.inputValue.trim().length > 0) {
        this.modalEntity.Name = this.inputValue;
        this.$emit("saveEntity", this.modalEntity);
      } else {
        this.alertDesc = "Name shouldn't be empty";
        this.alertTitle = "Error";
        this.alertVisible = true;
        this.alertType = "danger";
        this.modalShow = true;
      }
    },
    closeAlert: function() {
      this.alertVisible = false;
    }
  }
};
</script>

