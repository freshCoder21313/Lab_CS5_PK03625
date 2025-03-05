module.exports = {
  root: true,
  env: {
    node: true,
  },
  extends: ['eslint:recommended', 'plugin:vue/vue3-essential'],
  rules: {
    indent: 'off', // Tắt quy tắc liên quan đến indent
    quotes: 'off', // Tắt quy tắc liên quan đến dấu nháy
    semi: 'off', // Tắt quy tắc liên quan đến dấu chấm phẩy
    // Tắt thêm các quy tắc khác nếu cần
  },
};
