var $url = '/settings/administratorsLayerProfile';
var $urlAuth = '/settings/administratorsLayerProfile/actions/auth';
var $urlRoles = '/settings/administratorsLayerProfile/actions/roles';

var data = utils.init({
  userName: utils.getQueryString('userName'),
  organId: utils.getQueryInt('organId'),
  organName: utils.getQueryString('organName'),
  organType: utils.getQueryString('organType'),
  userId: 0,
  uploadUrl: null,
  auths: null,
  authDatas: null,
  roles: null,
  organs: null,
  selectOrgans: [],
  form: {
    userId: 0,
    userName: null,
    displayName: null,
    password: null,
    confirmPassword: null,
    avatarUrl: null,
    mobile: null,
    email: null,
    auth: null,
    authData: null,
    organId: 0,
    organName: '',
    organType: ''
  },
  isSelf: false,
  adminId: 0,
  creatorId: 0,
  roleChecked: []
});

var methods = {
  apiGet: function () {
    var $this = this;
    $this.form.organId = $this.organId;
    $this.form.organType = $this.organType;
    $this.form.organName = $this.organName;

    $api.get($url, {
      params: {
        userName: this.userName
      }
    }).then(function (response) {
      var res = response.data;
      $this.auths = res.auths;
      $this.authDatas = res.authDatas;
      $this.isSelf = res.isSelf;
      $this.roleChecked = res.rolesIds;
      $this.adminId = res.adminId;
      $this.creatorId = res.creatorId;

      if ($this.userName) {
        $this.userId = res.userId;
        $this.form.userName = res.userName;
        $this.form.displayName = res.displayName;
        $this.form.avatarUrl = res.avatarUrl;
        $this.form.mobile = res.mobile;
        $this.form.email = res.email;
        $this.form.auth = res.auth;
        $this.form.authData = res.authData;
        $this.form.organId = res.organId;
        $this.form.organName = res.organName;
        $this.form.organType = res.organType;
      }

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },

  apiGetRoles: function () {
    var $this = this;
    $api.get($urlRoles, {
    }).then(function (response) {
      var res = response.data;
      $this.roles = res.list;

    }).catch(function (error) {
    }).then(function () {
    });
  },
  btnAddRoleClick: function () {

    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getSettingsUrl('administratorsRoleAdd'),
      width: "68%",
      height: "78%",
      end: function () {
        $this.apiGetRoles();
      }
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
      auth: this.form.auth,
      authData: this.form.authData,
      organId: this.form.organId,
      organType: this.form.organType,
      rolesIds: this.roleChecked
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

    if (this.form.auth === "AdminNormal" && this.roleChecked.length === 0) {
      utils.error('请至少选择一个角色', { layer: true });
      return;
    }

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
    this.apiGetRoles();
    this.uploadUrl = $apiUrl + $url + '/actions/upload?userName=' + this.userName;
    this.apiGet();
  }
});
