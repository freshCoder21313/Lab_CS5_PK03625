<template>
  <div>
    <h1 class="text-center">Danh sách tài khoản</h1>
    <table class="table" id="dt-mU"></table>
  </div>
</template>

<script>
//#region Import range
import * as configsDt from "@/constants/configsDatatable.js";
import * as axiosClient from "@/api/axiosClient";
import $ from "jquery";
import "datatables.net";
import "datatables.net-dt/css/dataTables.dataTables.css";

//#endregion

export default {
  name: "ManagerUser",
  components: {},
  props: {},
  data() {
    return { tableData: [] };
  },
  computed: {},
  watch: {},
  mounted() {
    this.loadData();
  },
  methods: {
    loadData() {
      axiosClient
        .getFromApi("/manager/nhanvien/get")
        .then((response) => {
          this.tableData = response.data;
          this.$nextTick(() => {
            // Chờ DOM cập nhật
            this.initDataTable();
          });
        })
        .catch((error) => {
          console.error("Error loading data:", error);
          // Xử lý lỗi ở đây, ví dụ: hiển thị thông báo lỗi cho người dùng
        });
    },
    initDataTable() {
      this.$nextTick(() => {
        $("#dt-mU").DataTable({
          data: this.tableData,
          columns: [
            this.defaultTdToShowDetail,
            { data: "Id", width: "15%", title: "ID" },
            { data: "hoTen", width: "35%", title: "Họ tên" },
            { data: "soDienThoai", width: "20%", title: "Số điện thoại" },
            {
              data: "ngaySinh",
              width: "15%",
              title: "Ngày sinh",
              render: function (data) {
                return new Date(data).toLocaleDateString("vi-VN");
              },
            },
            {
              data: "Id",
              width: "20%",
              title: "Hành động",
              render: (data) => `
                    <button @click="deleteStaffRecord(${data})" class="btn btn-danger">🗑️</button>
                    <button @click="loadViewUpsertStaff(${data})" data-bs-toggle="modal" data-bs-target="#upsertModal" class="btn btn-warning">📝</button>
                  `,
            },
          ],
          order: [[1, "asc"]],
          language: configsDt.defaultLanguageDatatable,
          // serverSide: true, //nếu api hỗ trợ serverSide
        });
      });
    },
    deleteStaffRecord(staffId) {
      // Thêm code xử lý xóa nhân viên ở đây
      console.log(`Xóa nhân viên có ID: ${staffId}`);
    },
    loadViewUpsertStaff(staffId) {
      // Thêm code xử lý load form cập nhật/thêm nhân viên ở đây
      console.log(`Load form cho nhân viên có ID: ${staffId}`);
    },
  },
};
</script>

<style scoped></style>
