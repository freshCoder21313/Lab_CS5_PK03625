<template>
  <b-modal v-model="showModal" title="Truy Cập" hide-footer>
    <div class="text-center mb-3">
      <b-button-group>
        <b-button
          :variant="isLogin ? 'primary' : 'outline-primary'"
          @click="isLogin = true"
          >Đăng nhập</b-button
        >
        <b-button
          :variant="!isLogin ? 'primary' : 'outline-primary'"
          @click="isLogin = false"
          >Đăng ký</b-button
        >
      </b-button-group>
    </div>

    <b-form @submit.prevent="handleSubmit">
      <b-form-group
        v-for="(field, key) in filteredFields"
        :key="key"
        :label="field.label"
        :label-for="key"
      >
        <b-form-input
          :id="key"
          v-model.trim="formData[key]"
          :type="field.type"
          :placeholder="field.placeholder"
          :state="validation[key]"
          required
        ></b-form-input>
        <b-form-invalid-feedback v-if="!validation[key]">
          {{ field.invalidMessage }}
        </b-form-invalid-feedback>
      </b-form-group>

      <b-button type="submit" variant="primary" block>
        {{ isLogin ? "Đăng nhập" : "Đăng ký" }}
      </b-button>
    </b-form>
  </b-modal>
</template>

<script>
import { postToApi } from "@/api/axiosClient";
import DangKy from "@/models/requests/DangKy";
import { StaticValidates } from "@/constants/staticValidates";
import * as SwalPlugins from "@/plugins/notification";

export default {
  name: "AuthModal",
  data() {
    return {
      showModal: false,
      isLogin: true,
      formData: new DangKy(),
      validation: {},
      formFields: {
        userName: {
          ...StaticValidates.userName,
          label: "Tên đăng nhập",
          isLogin: true,
        },
        password: {
          ...StaticValidates.password,
          label: "Mật khẩu",
          type: "password",
          isLogin: true,
        },
        confirmPassword: {
          ...StaticValidates.confirmPassword,
          label: "Xác nhận mật khẩu",
          type: "password",
          isLogin: false,
        },
        email: {
          ...StaticValidates.email,
          label: "Email",
          type: "email",
          isLogin: false,
        },
        hoTen: { ...StaticValidates.hoTen, label: "Họ và Tên", isLogin: false },
        soDienThoai: {
          ...StaticValidates.soDienThoai,
          label: "Số điện thoại",
          type: "tel",
          isLogin: false,
        },
        diaChi: { ...StaticValidates.diaChi, label: "Địa chỉ", isLogin: false },
        ngaySinh: {
          ...StaticValidates.ngaySinh,
          label: "Ngày sinh",
          type: "date",
          isLogin: false,
        },
      },
    };
  },
  computed: {
    filteredFields() {
      return Object.fromEntries(
        // eslint-disable-next-line
        Object.entries(this.formFields).filter(([_, field]) =>
          this.isLogin ? field.isLogin : true
        )
      );
    },
  },
  methods: {
    async handleSubmit() {
      if (!this.validateForm()) return;
      try {
        const url = this.isLogin ? "/TruyCap/DangNhap" : "/TruyCap/DangKy";
        const data = this.isLogin
          ? {
              userName: this.formData.userName,
              password: this.formData.password,
            }
          : { ...this.formData };

        const response = await postToApi(url, data);
        if (response.success) {
          if (this.isLogin) {
            localStorage.setItem("accessToken", response.data.accessToken);
            localStorage.setItem("refreshToken", response.data.refreshToken);

            SwalPlugins.successNotification(response.message);
          }
          this.showModal = false;
        } else {
          SwalPlugins.errorNotification(response.message);
          throw new Error(response.message);
        }
      } catch (error) {
        SwalPlugins.errorNotification(error.message);
      }
    },
    validateForm() {
      this.validation = Object.keys(this.filteredFields).reduce((acc, key) => {
        const field = this.formFields[key];
        const value = this.formData[key] || "";

        if (typeof field.validate === "function") {
          acc[key] =
            field.validate.length === 2
              ? field.validate(this.formData.password, value)
              : field.validate(value);
        } else if (field.validate instanceof RegExp) {
          acc[key] = field.validate.test(value);
        } else {
          acc[key] = true;
        }

        return acc;
      }, {});

      return Object.values(this.validation).every(Boolean);
    },
  },
};
</script>

<style scoped></style>
