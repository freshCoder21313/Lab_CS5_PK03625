using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lab.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class changeValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GioHang");

            migrationBuilder.RenameColumn(
                name: "Gia",
                table: "ChiTietDonHangs",
                newName: "DonGia");

            migrationBuilder.AlterColumn<int>(
                name: "NguoiDungId",
                table: "DonHangs",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DonGia",
                table: "ChiTietDonHangs",
                newName: "Gia");

            migrationBuilder.AlterColumn<string>(
                name: "NguoiDungId",
                table: "DonHangs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "GioHang",
                columns: table => new
                {
                    MaSanPham = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DonHangMaDonHang = table.Column<int>(type: "int", nullable: true),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    TenSanPham = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GioHang", x => x.MaSanPham);
                    table.ForeignKey(
                        name: "FK_GioHang_DonHangs_DonHangMaDonHang",
                        column: x => x.DonHangMaDonHang,
                        principalTable: "DonHangs",
                        principalColumn: "MaDonHang");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GioHang_DonHangMaDonHang",
                table: "GioHang",
                column: "DonHangMaDonHang");
        }
    }
}
