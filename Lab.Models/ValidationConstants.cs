using System.ComponentModel.DataAnnotations;

namespace Lab.Models
{
    // Validate tiêu chuẩn
    public static class ValidationConstants
    {
        // Hạn chế kí tự đặc biệt: Dùng cho tên đăng nhập, mật khẩu,...
        public const string ValidateNoSpecialChar = @"^[a-zA-Z0-9]+$"; 
        public const string InvalidNoSpecialCharMessage = "Tên đăng nhập hoặc mật khẩu không được chứa kí tự đặc biệt.";

        // Cho phép tiếng Việt và kí tự đặc biệt: Dùng cho mô tả,..
        public const string ValidateString = @"^[a-zA-Z0-9àáạảãâầấậẩẫăđèéẹẻẽêềếệểễìíịỉĩòóọỏõôồốộổỗơờớợởỡùúụủũưừứựửữýỳỹỵÀÁẠẢÃÂẦẤẬẨẪĂĐÈÉẸẺẼÊỀẾỆỂỄÌÍỊỈĨÒÓỌỎÕÔỒỐỘỔỖƠỜỚỢỞỠÙÚỤỦŨƯỪỨỰỬỮÝỲỸỴ ,~!@#$%^&*()_+{}|:<>?`[\];',./\\-]*$";
        public const string InvalidStringMessage = "Mô tả không hợp lệ. Chỉ cho phép các ký tự chữ, số, và các ký tự đặc biệt được phép.";

        public const string ValidateStringName = @"^[a-zA-ZàáạảãâầấậẩẫăđèéẹẻẽêềếệểễìíịỉĩòóọỏõôồốộổỗơờớợởỡùúụủũưừứựửữýỳỹỵÀÁẠẢÃÂẦẤẬẨẪĂĐÈÉẸẺẼÊỀẾỆỂỄÌÍỊỈĨÒÓỌỎÕÔỒỐỘỔỖƠỜỚỢỞỠÙÚỤỦŨƯỪỨỰỬỮÝỲỸỴ ]*$"; // Cho phép dùng tiếng Việt và dấu cách ' ': Dùng để đặt tên
        public const string InvalidStringNameMessage = "Tên không hợp lệ. Chỉ cho phép ký tự chữ, số và khoảng trắng.";

        // Định dạng địa chỉ email hợp lệ
        public const string ValidateEmail = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
        public const string InvalidEmailMessage = "Địa chỉ email không hợp lệ.";

        // Hạn chế số điện thoại 10 chữ số
        public const string ValidatePhoneNumber = @"^(0\d{9})$";
        public const string InvalidPhoneNumberMessage = "Số điện thoại không hợp lệ. Vui lòng nhập số điện thoại có 10 chữ số.";

        // Mã bưu chính 5 hoặc 6 chữ số
        public const string ValidatePostalCode = @"^\d{5,6}$";
        public const string InvalidPostalCodeMessage = "Mã bưu chính không hợp lệ. Vui lòng nhập mã bưu chính có 5 hoặc 6 chữ số.";

        // Mật khẩu mạnh
        public const string ValidateStrongPassword = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$";
        public const string InvalidStrongPasswordMessage = "Mật khẩu không đủ mạnh. Vui lòng nhập mật khẩu ít nhất 8 ký tự, bao gồm chữ hoa, chữ thường, số và ký tự đặc biệt.";

        // Chỉ cho phép số nguyên dương
        public const string ValidatePositiveInteger = @"^\d+$";
        public const string InvalidPositiveIntegerMessage = "Giá trị không hợp lệ. Vui lòng nhập số nguyên dương.";

        // Tên người dùng
        public const string ValidateUsername = @"^[a-zA-Z0-9]{3,20}$";
        public const string InvalidUsernameMessage = "Tên người dùng không hợp lệ. Vui lòng nhập từ 3 đến 20 ký tự chữ hoặc số.";


        public static ValidationResult? ValidateBirthDate(DateTime date, ValidationContext context)
        {
            if (date > DateTime.Now)
            {
                return new ValidationResult("Ngày sinh không thể trong tương lai.");
            }
            return ValidationResult.Success;
        }
    }
}
