if (window.top != self) {
  window.top.location = self.location;
}

var $url = '/index';
var $sidebarWidth = 238;
var $collapseWidth = 60;

var data = utils.init({
  sessionId: localStorage.getItem('sessionId'),
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
  contextTop: 0
});

var methods = {
  apiGet: function () {
    var $this = this;

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

          $this.local = res.local;
          $this.menus = res.menus;
          $this.getLevelMenus($this.menus);

          var home = $this.menus[0];
          $this.defaultActive = home.id;
          $this.defaultOpeneds.push(home.id);
          $this.btnMenuClick(home);

          setTimeout($this.ready, 100);
        }


      } else {
        location.href = res.redirectUrl;
      }
    }).catch(function (error) {
      utils.loading($this, false);
      utils.error(error);
    });
  },
    redirectPassword: function(userName) {
        var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getSettingsUrl('administratorsLayerPassword', { userName: userName }),
      width: "38%",
      height: "58%",
      end: function() { $this.apiGet() }
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

  closeContextMenu: function () {
    this.contextMenuVisible = false;
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
    this.closeContextMenu();
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
      utils.addTab(menu.text, menu.link + "?menuId=" + menu.id);
    }
  },

  btnMobileMenuClick: function () {
    this.isCollapse = false;
    this.isMobileMenu = !this.isMobileMenu;
  }

};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    document.title = DOCUMENTTITLE_ADMIN;
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
  watch: {
    contextMenuVisible(value) {
      if (this.contextMenuVisible) {
        document.body.addEventListener("click", this.closeContextMenu);
      } else {
        document.body.removeEventListener("click", this.closeContextMenu);
      }
    }
  }
});
