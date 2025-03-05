class NguoiDungDto {
  constructor(
    id,
    userName,
    normalizedUserName,
    email,
    normalizedEmail,
    emailConfirmed,
    passwordHash,
    securityStamp,
    concurrencyStamp,
    phoneNumber,
    phoneNumberConfirmed,
    twoFactorEnabled,
    lockoutEnd,
    lockoutEnabled,
    accessFailedCount,
    hoTen,
    gioiTinh,
    soDienThoai,
    diaChi,
    linkAnh,
    ngaySinh,
    vaiTro
  ) {
    this.id = id;
    this.userName = userName;
    this.normalizedUserName = normalizedUserName;
    this.email = email;
    this.normalizedEmail = normalizedEmail;
    this.emailConfirmed = emailConfirmed;
    this.passwordHash = passwordHash;
    this.securityStamp = securityStamp;
    this.concurrencyStamp = concurrencyStamp;
    this.phoneNumber = phoneNumber;
    this.phoneNumberConfirmed = phoneNumberConfirmed;
    this.twoFactorEnabled = twoFactorEnabled;
    this.lockoutEnd = lockoutEnd ? new Date(lockoutEnd) : null; // Convert to Date object if provided
    this.lockoutEnabled = lockoutEnabled;
    this.accessFailedCount = accessFailedCount;
    this.hoTen = hoTen;
    this.gioiTinh = gioiTinh;
    this.soDienThoai = soDienThoai;
    this.diaChi = diaChi;
    this.linkAnh = linkAnh;
    this.ngaySinh = ngaySinh ? new Date(ngaySinh) : null; // Convert to Date object if provided
    this.vaiTro = vaiTro;
  }
}

module.exports = NguoiDungDto; // Export class if using Node.js
