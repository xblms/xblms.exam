var $url = '/settings/usersConfig';

var data = utils.init({
  form: {
    userPasswordMinLength: null,
    userPasswordRestriction: null,
    isUserLockLogin: null,
    userLockLoginCount: null,
    userLockLoginType: null,
    userLockLoginHours: null,
    isUserCaptchaDisabled: null,
    isHomeClosed:null
  }
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url).then(function (response) {
      var res = response.data;

      $this.form.isHomeClosed = res.config.isHomeClosed;
      $this.form.userPasswordMinLength = res.config.userPasswordMinLength;
      $this.form.userPasswordRestriction = res.config.userPasswordRestriction;
      $this.form.isUserLockLogin = res.config.isUserLockLogin;
      $this.form.userLockLoginCount = res.config.userLockLoginCount;
      $this.form.userLockLoginType = res.config.userLockLoginType;
      $this.form.userLockLoginHours = res.config.userLockLoginHours;
      $this.form.isUserCaptchaDisabled = res.config.isUserCaptchaDisabled;
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  apiSubmit: function () {
    var $this = this;

    utils.loading(this, true);
    $api.post($url, {
      isHomeClosed: this.form.isHomeClosed,
      userPasswordMinLength: this.form.userPasswordMinLength,
      userPasswordRestriction: this.form.userPasswordRestriction,
      isUserLockLogin: this.form.isUserLockLogin,
      userLockLoginCount: this.form.userLockLoginCount,
      userLockLoginType: this.form.userLockLoginType,
      userLockLoginHours: this.form.userLockLoginHours,
      isUserCaptchaDisabled: this.form.isUserCaptchaDisabled,
    }).then(function (response) {
      var res = response.data;

      utils.success('用户设置保存成功！');
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  getPasswordRestrictionText: function (val) {
    if (val === 'LetterAndDigit') return '字母和数字组合';
    else if (val === 'LetterAndDigitAndSymbol') return '字母、数字以及符号组合';
    else return '不限制';
  },

  btnSubmitClick: function () {
    var $this = this;

    this.$refs.form.validate(function(valid) {
      if (valid) {
        $this.apiSubmit();
      }
    });
  },

  btnCloseClick: function() {
    utils.removeTab();
  },
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  }
});
