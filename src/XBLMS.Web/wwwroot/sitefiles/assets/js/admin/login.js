var $url = '/login';
var $urlCaptcha = '/login/captcha';

if (window.top != self) {
  window.top.location = self.location;
}

var data = utils.init({
  status: utils.getQueryInt('status'),
  form: {
    account: null,
    password: null,
    captchaValue: null,
  },
  isPersistent: false,
  pageSubmit: false,
  pageAlert: null,
  captchaToken: null,
  captchaUrl: null,
  version: null,
  versionName: null,
  isAdminCaptchaDisabled: false,
  systemCodeName: null,
});

var methods = {
  apiGet: function () {
    var $this = this;

    if (this.status === 401) {
      this.pageAlert = {
        type: 'error',
        title: '账号登录已过期或失效，请重新登录'
      };
    }

    utils.loading(this, true);
    $api.get($url).then(function (response) {
      var res = response.data;
      if (res.success) {
        $this.systemCodeName = res.systemCodeName;
        document.title = res.systemCodeName;
        $this.version = res.version;
        $this.versionName = res.versionName;
        $this.isAdminCaptchaDisabled = res.isAdminCaptchaDisabled;
        $this.apiCaptcha();
      } else {
        location.href = res.redirectUrl;
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  apiCaptcha: function () {
    var $this = this;

    utils.loading(this, true);
    $api.post($urlCaptcha).then(function (response) {
      var res = response.data;

      $this.captchaToken = res.value;
      $this.form.captchaValue = '';
      $this.pageSubmit = false;
      $this.captchaUrl = $apiUrl + $urlCaptcha + '?token=' + $this.captchaToken;
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },


  apiSubmit: function (isForceLogoutAndLogin) {
    var $this = this;

    utils.loading(this, true, "登录...");
    $api.post($url, {
      account: this.form.account,
      password: md5(this.form.password),
      isPersistent: this.isPersistent,
      isForceLogoutAndLogin: isForceLogoutAndLogin,
      token: this.captchaToken,
      value: this.form.captchaValue
    }).then(function (response) {
      var res = response.data;

      if (res.isLoginExists) {
        $this.$confirm('该用户正在登录状态，可能是其他人正在使用或您上一次登录没有正常退出，是否强制注销并登录？', '强制登录提示', {
          confirmButtonText: '强制注销并登录',
          cancelButtonText: '取消',
          type: 'warning'
        }).then(() => {
          $this.apiSubmit(true);
        }).catch(() => {
          $this.$message({
            type: 'success',
            message: '已取消登录'
          });
          utils.loading($this, false);
        });
      } else {
        localStorage.setItem(SESSION_ID_NAME, res.sessionId);
        localStorage.removeItem(ACCESS_TOKEN_NAME);
        sessionStorage.removeItem(ACCESS_TOKEN_NAME);
        if ($this.isPersistent) {
          localStorage.setItem(ACCESS_TOKEN_NAME, res.token);
        } else {
          sessionStorage.setItem(ACCESS_TOKEN_NAME, res.token);
        }
        $this.redirectIndex();
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      $this.apiCaptcha();
      utils.loading($this, false);
    });
  },

  redirectIndex: function () {
    location.href = utils.getIndexUrl();
  },


  btnCaptchaClick: function () {
    this.apiCaptcha();
  },

  btnSubmitClick: function () {
    var $this = this;
    this.$refs.form.validate(function (valid) {
      if (valid) {
        $this.pageSubmit = true;
        $this.pageAlert = null;
        $this.apiSubmit(false);
      }
    });
  }
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    var $this = this;
    this.apiGet();
    setTimeout(function () {
      $this.$refs.account.focus();
    }, 100);
  }
});
