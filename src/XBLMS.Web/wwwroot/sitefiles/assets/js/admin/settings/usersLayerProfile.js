var $url = '/settings/usersLayerProfile';

var data = utils.init({
  userId: utils.getQueryInt('userId'),
  organId: utils.getQueryString("organId"),
  organType: utils.getQueryString("organType"),
  organName: utils.getQueryString('organName'),
  uploadUrl: null,
  form: {
    userName: null,
    displayName: null,
    password: null,
    confirmPassword: null,
    avatarUrl: null,
    mobile: null,
    email: null,
    dutyName: '',
    locked: false,
    organId: 0,
    organType: '',
    organName:''
  }
});

var methods = {
  apiGet: function () {
    var $this = this;

    $this.form.organId = $this.organId;
    $this.form.organType = $this.organType;
    $this.form.organName = $this.organName;


    $api.get($url, {
      params: {
        userId: this.userId
      }
    }).then(function (response) {
      var res = response.data;
      $this.organs = res.organs;

      if ($this.userId > 0) {
        $this.userId = res.userId;
        $this.form.userName = res.userName;
        $this.form.displayName = res.displayName;
        $this.form.avatarUrl = res.avatarUrl;
        $this.form.mobile = res.mobile;
        $this.form.email = res.email;
        $this.form.organId = res.organId;
        $this.form.organType = res.organType;
        $this.form.organName = res.organName;
        $this.form.locked = res.locked;
        $this.form.dutyName = res.dutyName;
      }


    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },

  apiSubmit: function () {
    var $this = this;

    utils.loading(this, true);
    $api.post($url, {
      userId: this.userId,
      userName: this.form.userName,
      displayName: this.form.displayName,
      password: this.form.password,
      avatarUrl: this.form.avatarUrl,
      mobile: this.form.mobile,
      email: this.form.email,
      organId: this.form.organId,
      locked: this.form.locked,
      dutyName: this.form.dutyName,
      organId: this.form.organId,
      organType: this.form.organType,
    }).then(function (response) {
      utils.success('操作成功！');
      utils.closeLayer(false);
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },

  validatePass: function (rule, value, callback) {
    if (value === '') {
      callback(new Error('请再次输入密码'));
    } else if (value !== this.form.password) {
      callback(new Error('两次输入密码不一致!'));
    } else {
      callback();
    }
  },

  btnSubmitClick: function () {
    var $this = this;

    this.$refs.form.validate(function (valid) {
      if (valid) {
        $this.apiSubmit();
      }
    });
  },

  uploadBefore(file) {
    var re = /(\.jpg|\.jpeg|\.bmp|\.gif|\.png|\.webp)$/i;
    if (!re.exec(file.name)) {
      utils.error('头像只能是图片格式，请选择有效的文件上传!', { layer: true });
      return false;
    }

    var isLt10M = file.size / 1024 / 1024 < 10;
    if (!isLt10M) {
      utils.error('头像图片大小不能超过 10MB!', { layer: true });
      return false;
    }
    return true;
  },

  uploadProgress: function () {
    utils.loading(this, true);
  },

  uploadSuccess: function (res, file, fileList) {
    this.form.avatarUrl = res.value;
    utils.loading(this, false);
    if (fileList.length > 1) fileList.splice(0, 1);
  },

  uploadError: function (err) {
    utils.loading(this, false);
    var error = JSON.parse(err.message);
    utils.error(error.message, { layer: true });
  },

  uploadRemove(file) {
    this.form.avatarUrl = null;
  },

  btnCancelClick: function () {
    utils.closeLayer(false);
  },
  btnSelectOrganClick: function () {
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getCommonUrl('selectOrgan', { windowName: window.name, selectOne: true }),
      width: "60%",
      height: "88%"
    });
  },
  selectOrganCallback: function (selectCallbackList) {
    var selectOrgan = selectCallbackList[0];
    this.form.organId = selectOrgan.id;
    this.form.organName = selectOrgan.name;
    this.form.organType = selectOrgan.type;
  },
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.uploadUrl = $apiUrl + $url + '/actions/upload?userId=' + this.userId;
    this.apiGet();
  }
});
