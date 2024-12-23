var $url = '/login';
var $urlCaptcha = '/login/captcha';

if (window.top != self) {
  window.top.location = self.location;
}

var data = utils.init({
  status: utils.getQueryInt('status'),
  pageSubmit: false,
  pageAlert: null,
  account: null,
  password: null,
  isPersistent: false,
  captchaToken: null,
  captchaValue: null,
  captchaUrl: null,
  version:null,
  isAdminCaptchaDisabled: false,
  loginTitle: DOCUMENTTITLE_ADMIN
});

var methods = {
  apiGet: function () {
    var $this = this;

    if (this.status === 401) {
      this.pageAlert = {
        type: 'danger',
        html: '账号登录已过期或失效，请重新登录'
      };
    }

    utils.loading(this, true);
    $api.get($url).then(function (response) {
      var res = response.data;
      if (res.success) {
        $this.version = res.version;
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
      $this.captchaValue = '';
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

    utils.loading(this, true);
    $api.post($url, {
      account: this.account,
      password: md5(this.password),
      isPersistent: this.isPersistent,
      isForceLogoutAndLogin: isForceLogoutAndLogin,
      token: this.captchaToken,
      value: this.captchaValue
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

  btnSubmitClick: function (e) {
    e && e.preventDefault();
    this.pageSubmit = true;
    this.pageAlert = null;

    if (!this.account || !this.password) return;
    if (!this.isAdminCaptchaDisabled && !this.captchaValue) return;
    this.apiSubmit(false);
  }
};

var $vue = new Vue({
  el: '#main',
  data: data,
  directives: {
    focus: {
      inserted: function (el) {
        el.focus()
      }
    }
  },
  methods: methods,
  created: function () {
    document.title = DOCUMENTTITLE_ADMIN + '-登录';
    this.apiGet();
  }
});
