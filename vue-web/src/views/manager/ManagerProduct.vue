<template>
  <div>
    <h1 class="title has-text-centered">Danh sách sản phẩm</h1>
    <b-button
      variant="success"
      class="btn btn-success"
      @click="toggleForm(null)"
    >
      <i class="material-icons center">add</i> Thêm
    </b-button>

    <div class="row justify-content-between mt-4">
      <div class="col-6">
        <label for="" class="label-form">Nhập giá lọc nhỏ nhất:</label>
        <b-form-input
          v-model.number="giaNhoNhat"
          @input="loadData"
          type="number"
          min="1"
          placeholder="Nhập giá nhỏ nhất..."
        />
      </div>
      <div class="col-6">
        <label for="" class="label-form">Nhập giá lọc cao nhất:</label>
        <b-form-input
          v-model.number="giaLonNhat"
          @input="loadData"
          type="number"
          :min="giaNhoNhat || 1"
          placeholder="Nhập giá lớn nhất..."
        />
      </div>
    </div>

    <!-- DataTable -->
    <table m-table class="table is-striped is-hoverable" id="dt-mP"></table>

    <!-- Modal Form -->
    <b-modal v-model="showForm" size="lg" title="Quản lý sản phẩm" hide-footer>
      <template #modal-title>
        <h5>{{ isEditing ? "Sửa sản phẩm" : "Thêm sản phẩm" }}</h5>
      </template>

      <form @submit.prevent="submitForm">
        <div class="mb-3">
          <label for="maSanPham" class="form-label">Mã sản phẩm</label>
          <b-form-input
            v-model="formData.maSanPham"
            id="maSanPham"
            placeholder="Nhập mã sản phẩm"
            disabled
          />
        </div>
        <div class="mb-3">
          <label for="tenSanPham" class="form-label">Tên sản phẩm</label>
          <b-form-input
            v-model="formData.tenSanPham"
            id="tenSanPham"
            placeholder="Nhập tên sản phẩm"
          />
        </div>
        <div class="mb-3">
          <label for="soLuong" class="form-label">Số lượng</label>
          <b-form-input
            v-model.number="formData.soLuong"
            id="soLuong"
            type="number"
            placeholder="Nhập số lượng"
          />
        </div>
        <div class="mb-3">
          <label for="donGia" class="form-label">Đơn giá</label>
          <b-form-input
            v-model.number="formData.donGia"
            id="donGia"
            type="number"
            placeholder="Nhập đơn giá"
          />
        </div>

        <div class="modal-footer text-right">
          <b-button variant="secondary" @click="showForm = false">Hủy</b-button>
          <b-button variant="primary" type="submit">Lưu</b-button>
        </div>
      </form>
    </b-modal>
  </div>
</template>

<script>
import * as configsDt from "@/constants/configsDatatable.js";
import * as axiosClient from "@/api/axiosClient";
import * as SwalPlugins from "@/plugins/notification";
import $ from "jquery";
import "datatables.net";
import "datatables.net-dt";
import SanPhamDto from "@/models/dtos/SanPhamDto";

export default {
  name: "ManagerProduct",
  data() {
    return {
      tableData: [],
      giaNhoNhat: 1,
      giaLonNhat: 9999999,
      isEditing: false,
      formData: new SanPhamDto(),
      showForm: false,
    };
  },
  mounted() {
    this.loadData();
  },
  methods: {
    // Tải dữ liệu datatable
    loadData() {
      axiosClient
        .getFromApi(
          `/Manager/SanPham/HandFilterByPriceAndManualSortByPrice?giaNhoNhat=${this.giaNhoNhat}&giaLonNhat=${this.giaLonNhat}`
        )
        .then((response) => {
          this.tableData = response.data;
          this.$nextTick(this.initDataTable);
        })
        .catch((error) => {
          console.error("Lỗi tải danh sách sản phẩm:", error);
        });
    },
    initDataTable() {
      const tableId = "#dt-mP";

      if ($.fn.DataTable.isDataTable(tableId)) {
        $(tableId).DataTable().destroy();
        $(tableId).empty();
      }

      $(tableId).DataTable({
        data: this.tableData,
        columns: [
          {
            data: "maSanPham",
            title: "ID",
            className: "text-center",
            width: "15%",
          },
          {
            data: "tenSanPham",
            title: "Tên sản phẩm",
            className: "text-left",
            width: "35%",
          },
          {
            data: "soLuong",
            title: "Số lượng",
            className: "text-center",
            width: "10%",
          },
          {
            data: "donGia",
            title: "Đơn giá",
            className: "text-right",
            width: "20%",
            render: (data) =>
              typeof data === "number"
                ? data.toLocaleString("vi-VN", {
                    style: "currency",
                    currency: "VND",
                  })
                : "N/A",
          },
          {
            data: "maSanPham",
            title: "Hành động",
            width: "20%",
            render: (data) => `
              <button class="btn btn-danger btn-delete" data-id="${data}"><i class="material-icons">delete</i></button>
              <button class="btn btn-warning btn-edit" data-id="${data}"><i class="material-icons">edit</i></button>
            `,
          },
        ],
        order: [[0, "asc"]],
        language: configsDt.defaultLanguageDatatable,
      });

      this.attachEventHandlers();
    },
    // Gắn sự kiện delete/edit
    attachEventHandlers() {
      const self = this;
      $("#dt-mP").on("click", ".btn-delete", function () {
        const maSanPham = $(this).data("id");
        self.deleteProductRecord(maSanPham);
      });

      $("#dt-mP").on("click", ".btn-edit", function () {
        const maSanPham = $(this).data("id");
        self.loadViewUpsertProduct(maSanPham);
      });
    },
    // Load view insert
    toggleForm(product) {
      if (product) {
        this.isEditing = true;
        this.formData = { ...product };
      } else {
        this.isEditing = false;
        this.formData = new SanPhamDto();
      }
      this.showForm = true;
    },
    closeModal() {
      this.showForm = false;
    },
    resetForm() {
      this.formData = new SanPhamDto();
      this.isEditing = false;
    },
    submitForm() {
      const apiCall = this.isEditing
        ? axiosClient.putToApi(`/Manager/SanPham/put/`, this.formData)
        : axiosClient.postToApi(`/Manager/SanPham/post/`, this.formData);

      apiCall
        .then((response) => {
          if (response.success) {
            this.$toast.success(response.message);
            this.loadData();
            this.closeModal();
          }
        })
        .catch((error) => {
          console.error("Lỗi xử lý form sản phẩm:", error);
        });
    },
    deleteProductRecord(maSanPham) {
      SwalPlugins.confirmAction(
        `Bạn có chắc muốn xóa sản phẩm ID: ${maSanPham}?`,
        () => {
          axiosClient
            .deleteFromApi(`/Manager/SanPham/delete/${maSanPham}`)
            .then((response) => {
              if (response.success) {
                this.$toast.success(response.message);
                this.loadData();
              }
            })
            .catch((error) => {
              console.error("Lỗi xóa sản phẩm:", error);
            });
        }
      );
    },
    loadViewUpsertProduct(maSanPham) {
      const product = this.tableData.find(
        (item) => item.maSanPham === maSanPham
      );
      if (product) {
        this.toggleForm(product);
      }
    },
  },
};
</script>
