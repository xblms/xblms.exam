var $url = "/dashboard";

var data = utils.init({
  homepage: '/',
  admin: null
});

var methods = {
  apiGet: function() {
    var $this = this;

    utils.loading(this, true);
    $api.get($url).then(function (response) {
      var res = response.data;
      $this.admin = res.administrator;
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnUserMenuClick: function (command) {
    var $this = this;
    if (command === 'view') {
      utils.openAdminView(this.admin.id);
    } else if (command === 'profile') {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getSettingsUrl('administratorsLayerProfile', { userName: this.admin.userName }),
        width: "60%",
        height: "88%",
        end: function () {
          $this.apiGet();
        }
      });
    } else if (command === 'password') {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getSettingsUrl('administratorsLayerPassword', { userName: this.admin.userName }),
        width: "38%",
        height: "58%",
      });
    } else if (command === 'logout') {
      top.utils.alertInfo({
        title: '安全退出',
        text: '确定要退出系统吗？',
        button: '确 定',
        callback: function () {
          top.location.href = utils.getRootUrl('logout')
        }
      });
    }
  },

};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    location.href = utils.getRootUrl('dashboardAdmin')
  },
});
