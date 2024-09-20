var BLOCK_HOME_SESSION_ID = "xblms-blockhome-session-id";
var blockBaseApi = "/api/home/blockhome"; 

var blockApi = axios.create({
  baseURL: blockBaseApi,
  withCredentials: true
});

var bodyHtml = "";

var blockAuthen = function (password) {
  Swal.showLoading();
  blockApi
    .post("", {
      password: password
    })
    .then(function (response) {
      var res = response.data;

      if (res.success) {
        sessionStorage.setItem(BLOCK_HOME_SESSION_ID, res.sessionId);

        location.href = "/home/"
      } else {
        Swal.fire({
          title: "访问密码不正确！",
          type: "error",
          showConfirmButton: false,
          allowOutsideClick: false,
          allowEscapeKey: false
        });
      }
    })
    .catch(function (error) {
      console.log(error);
    });
};

blockApi
  .get("", {
    params: {
      sessionId: sessionStorage.getItem(BLOCK_HOME_SESSION_ID)
    }
  })
  .then(function (response) {
    var res = response.data;

    var isAllowed = res.isAllowed;
    var blockMethod = res.blockMethod;
    var redirectUrl = res.redirectUrl;
    var warning = res.warning;

    if (!isAllowed) {
      if (blockMethod === "RedirectUrl") {
        location.href = redirectUrl;
      } else if (blockMethod === "Warning") {
        bodyHtml = document.body.innerHTML;
        document.title = DOCUMENTTITLE_BLOCK;
        document.body.innerHTML = "";
        document.body.style.display = "block";
        Swal.fire({
          title: "你被限制访问",
          text: warning,
          type: "error",
          showConfirmButton: false,
          allowOutsideClick: false,
          allowEscapeKey: false
        });
      } else if (blockMethod === "Password") {
        bodyHtml = document.body.innerHTML;
        document.title = DOCUMENTTITLE_BLOCK;
        document.body.innerHTML = "";
        document.body.style.display = "block";
        Swal.fire({
          type: "error",
          title: "你被限制访问",
          text: "请输入访问密码",
          input: "password",
          showCancelButton: false,
          confirmButtonText: "验 证",
          confirmButtonColor: '#ff6a00',
          allowOutsideClick: false,
          allowEscapeKey: false,
          inputValidator: function (value) {
            if (!value) {
              return "请输入访问密码！";
            } else {
              blockAuthen(value);
            }
          }
        });
      }
    } else {
      document.body.style.display = "block";
    }
  })
  .catch(function (error) {
    console.log(error);
  });
