if (window.top != self) {
  window.top.location = self.location;
}

var $url = '/index';
var $urlChangeShowall = $url + '/changeShowall';
var $urlChangeOrgan = $url + '/changeOrgan';

var $sidebarWidth = 238;
var $collapseWidth = 60;

var data = utils.init({
  version: null,
  versionName: null,
  sessionId: localStorage.getItem(SESSION_ID_NAME),
  menus: null,
  levelMenus: [],
  local: null,
  menu: null,
  keyword: null,

  defaultOpeneds: [],
  defaultActive: "",
  tabName: null,
  tabs: [],
  winHeight: 0,
  winWidth: 0,
  isCollapse: false,
  isDesktop: true,
  isMobileMenu: false,

  contextMenuVisible: false,
  contextTabName: null,
  contextLeft: 0,
  contextTop: 0,

  topFrameDrawer: false,
  topFrameTitle: null,
  topFrameSrc: '',
  topFrameWidth: 88,

  topRightFrameDrawer: false,
  topRightFrameTitle: null,
  topRightFrameSrc: '',
  topRightFrameWidth: 88,

  topTopFrameDrawer: false,
  topTopFrameTitle: null,
  topTopFrameSrc: '',
  topTopFrameWidth: 50,

  isSafeMode: false,
  adminEnforceLogoutMinutes: 0,
  adminEnforceLogoutMinutesTimer: 0
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);

    $api.get($url, {
      params: {
        sessionId: this.sessionId
      }
    }).then(function (response) {
      var res = response.data;

      if (res.value) {
        if (res.isEnforcePasswordChange) {
          utils.error('账号密码已过期，请尽快修改密码');
          $this.redirectPassword(res.local.userName);
        } else {

          document.title = res.systemCodeName;
          $this.version = res.version;
          $this.versionName = res.versionName;
          $this.isSafeMode = res.isSafeMode;
          $this.local = res.local;
          $this.menus = res.menus;
          $this.getLevelMenus($this.menus);

          var home = $this.menus[0];
          $this.defaultActive = home.id;
          $this.defaultOpeneds.push(home.id);
          $this.btnMenuClick(home);

          $this.adminEnforceLogoutMinutes = res.adminEnforceLogoutMinutes;
          if ($this.adminEnforceLogoutMinutes > 0) {
            window.setInterval($this.adminEnforceLogoutMinutesTimerOut, 1000)
          }

          setTimeout($this.ready, 100);
        }
      } else {
        location.href = res.redirectUrl;
      }
    }).catch(function (error) {
      utils.error(error);
      utils.loading($this, false);
    });
  },
  changeShowall: function (val) {
    var $this = this;

    utils.loading(this, true);

    $api.post($urlChangeShowall, {
      authDataShowAll: val
    }).then(function (response) {
      var res = response.data;
      if (res.value) {
        $this.changeOrganReload();
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  changeOrganReload: function () {

    this.tabs.forEach(tab => {
      var url = tab.url;
      var iframe = top.document.getElementById('frm-' + tab.name).contentWindow;
      iframe.location.href = url;
    });

  },
  redirectPassword: function (userName) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getSettingsUrl('administratorsLayerPassword', { userName: userName }),
      width: "38%",
      height: "58%",
      end: function () { $this.apiGet() }
    });
  },

  getLevelMenus: function (menus) {
    menus.forEach(m => {
      this.levelMenus.push(m);
      if (m.children && m.children.length > 0) {
        this.getLevelMenus(m.children)
      }
    })
  },
  ready: function () {
    window.onresize = this.winResize;
    window.onresize();
    utils.loading(this, false);
  },
  openContextMenu: function (e) {
    if (e.srcElement.id && _.startsWith(e.srcElement.id, 'tab-')) {
      this.contextTabName = _.trimStart(e.srcElement.id, 'tab-');
      this.contextMenuVisible = true;
      this.contextLeft = e.clientX;
      if (e.clientX + 130 > this.winWidth) {
        this.contextLeft = this.winWidth - 130;
      } else {
        this.contextLeft = e.clientX;
      }
      this.contextTop = e.clientY;
    }
  },
  tabClick: function (e) {
    var $this = this;

    this.contextTabName = e.name;
    var index = $this.tabs.findIndex(function (tab) {
      return tab.name === e.name;
    });
    if (index === 0) {
      var tab = $this.tabs[index];
      var url = tab.url;
      var iframe = top.document.getElementById('frm-' + tab.name).contentWindow;
      iframe.location.href = url;
    }
  },
  btnContextClick: function (command) {
    var $this = this;

    if (command === 'this') {
      this.tabs = this.tabs.filter(function (tab) {
        return tab.name !== $this.contextTabName;
      });
    }
    else if (command === 'reload') {
      var index = $this.tabs.findIndex(function (tab) {
        return tab.name === $this.contextTabName;
      });
      var tab = $this.tabs[index];

      var url = tab.url;
      var iframe = top.document.getElementById('frm-' + tab.name).contentWindow;
      iframe.location.href = url;
    }
    else if (command === 'others') {
      this.tabs = this.tabs.filter(function (tab) {
        return tab.name === $this.contextTabName || tab.title === '首页';
      });

      var index = $this.tabs.findIndex(function (tab) {
        return tab.name === $this.contextTabName;
      });
      var tab = $this.tabs[index];
      this.tabName = tab.name;
      //utils.openTab($this.contextTabName);
    }
  },
  winResize: function () {
    this.winHeight = $(window).height();
    this.winWidth = $(window).width();
    this.isDesktop = this.winWidth > 992;
  },

  getIndex: function (level1, level2, level3) {
    if (level3) return level1.id + '/' + level2.id + '/' + level3.id;
    else if (level2) return level1.id + '/' + level2.id;
    else if (level1) return level1.id;
    return '';
  },

  btnSideMenuClick: function (sideMenuIds) {

    if (this.tabs.length > 3) {
      var newTbas = [];
      for (var ti = 0; ti < this.tabs.length; ti++) {
        if (ti != 1) {
          newTbas.push(this.tabs[ti]);
        }
      }
      this.tabs = newTbas;
    }

    var ids = sideMenuIds.split('/');
    var defaultOpeneds = [];


    var curMenu = null;
    for (var i = 0; i < ids.length; i++) {
      if (i === ids.length - 1) {
        curMenu = _.find(this.levelMenus, function (x) {
          return x.id == ids[i];
        });
        defaultOpeneds.push(curMenu.id);
      }
      else {
        var otherMenu = _.find(this.levelMenus, function (x) {
          return x.id == ids[i];
        });
        defaultOpeneds.push(otherMenu.id);
      }
    }
    this.defaultOpeneds = defaultOpeneds;
    if (curMenu) {
      this.defaultActive = curMenu.id;
      this.btnMenuClick(curMenu);
    }
  },

  btnMenuClick: function (menu) {
    if (menu.target == "_blank") {
      top.location.href = menu.link;
    }
    else {
      this.contextTabName = menu.name;
      utils.addTab(menu.text, menu.link + "?menuId=" + menu.id);
    }
  },

  btnUserMenuClick: function (command) {
    var $this = this;
    if (command === 'view') {
      utils.openAdminView(this.local.userId);
    } else if (command === 'profile') {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getSettingsUrl('administratorsLayerProfile', { userName: this.local.userName }),
        width: "60%",
        height: "88%"
      });
    } else if (command === 'password') {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getSettingsUrl('administratorsLayerPassword', { userName: this.local.userName }),
        width: "38%",
        height: "58%",
      });
    } else if (command === 'logout') {
      top.utils.alertWarning({
        title: '安全退出',
        text: '确定要退出系统吗？',
        callback: function () {
          top.location.href = utils.getRootUrl('logout')
        }
      });
    }
    else if (command === 'docs') {
      window.open('/docs/')
    }
  },
  btnSelectOrganClick: function () {
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getCommonUrl('selectOrganChange'),
      width: "60%",
      height: "88%"
    });
  },
  selectOrganCallback: function (selectCallbackList) {
    var selectOrgan = selectCallbackList[0];

    var $this = this;

    utils.loading(this, true);

    $api.post($urlChangeOrgan, {
      organId: selectOrgan.id
    }).then(function (response) {
      var res = response.data;
      if (res.value) {
        $this.local.authCurrentOrganName = selectOrgan.name;
        $this.changeOrganReload();
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  adminEnforceLogoutMinutesTimerOut: function () {
    this.adminEnforceLogoutMinutesTimer++;
    if (this.adminEnforceLogoutMinutesTimer >= this.adminEnforceLogoutMinutes * 60) {
      top.location.href = utils.getRootUrl('logout')
    }
  },
  adminEnforceLogoutMinutesTimerFun: function () {
    this.adminEnforceLogoutMinutesTimer = 0;
  }

};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  },
  computed: {
    leftWidth: function () {
      if (this.isDesktop) {
        return this.isCollapse ? $collapseWidth : $sidebarWidth;
      }
      return this.isMobileMenu ? this.winWidth : 0;
    }
  },
  mounted: function () {
    document.body.addEventListener('mousemove', this.adminEnforceLogoutMinutesTimerFun);
  }
});
