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
      <b-form-group label="Tên đăng nhập" label-for="username">
        <b-form-input
          id="username"
          v-model.trim="formData.userName"
          :state="validation.userName"
          required
        ></b-form-input>
        <b-form-invalid-feedback v-if="!validation.userName"
          >Vui lòng nhập tên đăng nhập.</b-form-invalid-feedback
        >
      </b-form-group>

      <b-form-group label="Mật khẩu" label-for="password">
        <b-form-input
          type="password"
          id="password"
          v-model.trim="formData.password"
          :state="validation.password"
          required
        ></b-form-input>
        <b-form-invalid-feedback v-if="!validation.password"
          >Mật khẩu tối thiểu 6 ký tự.</b-form-invalid-feedback
        >
      </b-form-group>

      <b-form-group v-if="!isLogin" label="Email" label-for="email">
        <b-form-input
          type="email"
          id="email"
          v-model.trim="formData.email"
          :state="validation.email"
          required
        ></b-form-input>
        <b-form-invalid-feedback v-if="!validation.email"
          >Vui lòng nhập email hợp lệ.</b-form-invalid-feedback
        >
      </b-form-group>

      <b-form-group
        v-if="!isLogin"
        label="Xác nhận mật khẩu"
        label-for="confirmPassword"
      >
        <b-form-input
          type="password"
          id="confirmPassword"
          v-model.trim="formData.confirmPassword"
          :state="validation.confirmPassword"
          required
        ></b-form-input>
        <b-form-invalid-feedback v-if="!validation.confirmPassword"
          >Mật khẩu xác nhận không khớp.</b-form-invalid-feedback
        >
      </b-form-group>

      <b-button type="submit" variant="primary" block
        >{ isLogin ? "Đăng nhập" : "Đăng ký" }</b-button
      >
    </b-form>
  </b-modal>
</template>

<script>
import { postToApi } from "@/api/axiosClient";
import Swal from "sweetalert2";
export default {
  name: "AuthModal",
  data() {
    return {
      showModal: false,
      isLogin: true,
      formData: { userName: "", password: "", confirmPassword: "", email: "" },
      validation: {
        userName: null,
        password: null,
        confirmPassword: null,
        email: null,
      },
    };
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
          : {
              userName: this.formData.userName,
              password: this.formData.password,
              email: this.formData.email,
            };
        const response = await postToApi(url, data);
        if (response.success) {
          if (this.isLogin) {
            localStorage.setItem("accessToken", response.data.accessToken);
            localStorage.setItem("refreshToken", response.data.refreshToken);
          }
          this.showModal = false;
        } else {
          throw new Error(response.message);
        }
      } catch (error) {
        Swal.fire("Lỗi", error.message, "error");
      }
    },
    validateForm() {
      this.validation.userName = this.formData.userName.length > 0;
      this.validation.password = this.formData.password.length >= 6;
      this.validation.email =
        this.isLogin || /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(this.formData.email);
      this.validation.confirmPassword =
        this.isLogin ||
        this.formData.password === this.formData.confirmPassword;
      return (
        this.validation.userName &&
        this.validation.password &&
        this.validation.email &&
        this.validation.confirmPassword
      );
    },
  },
};
</script>

<style scoped></style>
