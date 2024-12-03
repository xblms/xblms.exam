var $url = '/index';

var $sidebarWidth = 238;
var $collapseWidth = 60;

var data = utils.init({
  menus: [],
  user: null,

  menu: null,
  levelMenus: [],

  defaultOpeneds: [],
  defaultActive: '',
  defaultActiveIndex:'',
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
  paperTotal: 0,
  qPaperTotal: 0,
  assTotal:0,
  taskTotal:0
});

var methods = {
  apiGet: function () {
    var $this = this;

    $api.get($url).then(function (response) {
      var res = response.data;
      if (res.user) {

        $this.paperTotal = res.paperTotal;
        $this.qPaperTotal = res.qPaperTotal;
        $this.assTotal = res.assTotal;
        $this.taskTotal = $this.paperTotal + $this.qPaperTotal + $this.assTotal;

        $this.user = res.user;
        $this.menus = res.menus;
        $this.getLevelMenus($this.menus);

        $this.btnTopMenuClick('index');

        setTimeout($this.ready, 100);
      } else {
        location.href = utils.getRootUrl('login');
      }
    }).catch(function (error) {
      utils.error(error);
    });
  },
  apiGetTask: function () {
    var $this = this;

    $api.get($url).then(function (response) {
      var res = response.data;
      $this.paperTotal = res.paperTotal;
      $this.qPaperTotal = res.qPaperTotal;
      $this.assTotal = res.assTotal;
      $this.taskTotal = $this.paperTotal + $this.qPaperTotal + $this.assTotal;
    }).catch(function (error) {
      utils.error(error);
    });
  },
  ready: function () {
    window.onresize = this.winResize;
    window.onresize();

    utils.loading(this, false);
  },


  getLevelMenus: function (menus) {

    menus.forEach(m => {
      this.levelMenus.push(m);
      if (m.children && m.children.length > 0) {
        this.getLevelMenus(m.children)
      }
    })

  },
  openContextMenu: function (e) {
    if (e.srcElement.id && _.startsWith(e.srcElement.id, 'tab-')) {
      this.contextTabName = _.trimStart(e.srcElement.id, 'tab-');
      this.contextMenuVisible = true;
      this.contextLeft = e.clientX;
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
    } else if (command === 'others') {
      this.tabs = this.tabs.filter(function (tab) {
        return tab.name === $this.contextTabName;
      });
      utils.openTab($this.contextTabName);
    } else if (command === 'left') {
      var isTab = false;
      this.tabs = this.tabs.filter(function (tab) {
        if (tab.name === $this.contextTabName) {
          isTab = true;
        }
        return tab.name === $this.contextTabName || isTab;
      });
    } else if (command === 'right') {
      var isTab = false;
      this.tabs = this.tabs.filter(function (tab) {
        if (tab.name === $this.contextTabName) {
          isTab = true;
        }
        return tab.name === $this.contextTabName || !isTab;
      });
    } else if (command === 'all') {
      this.tabs = [];
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
      this.defaultActiveIndex = this.getIndex(curMenu);
      this.btnMenuClick(curMenu);
    }
  },
  btnTopMenuClick: function (command) {
    var lcurMenu = _.find(this.levelMenus, function (x) {
      return x.name == command;
    });
    if (lcurMenu)
    {
      this.defaultActiveIndex = this.getIndex(lcurMenu);
      this.btnMenuClick(lcurMenu)
    }
  },
  btnMenuClick: function (menu) {
    if (menu.target == "_blank") {
      top.location.href = menu.link;
    }
    else {
      if (this.tabs && this.tabs.length > 0) {
        this.tabs = [];
      }
      var newText = '<i class="' + menu.iconClass + '"><span class="me-2"></span>' + menu.text;
      utils.addTab(newText, menu.link);
    }
  },

  btnMobileMenuClick: function () {
    this.isCollapse = false;
    this.isMobileMenu = !this.isMobileMenu;
  },

  btnUserMenuClick: function (command) {

    this.tabs = [];
    var newTitle = "";
    var newLink = "";


    if (command === 'profile') {

      newTitle = '<i class="el-icon-edit-outline"><span class="me-2"></span>修改资料';
      newLink = utils.getPageUrl(null, 'profile');

    }
    else if (command === 'password') {

      newTitle = '<i class="el-icon-key"><span class="me-2"></span>更改密码';
      newLink = utils.getPageUrl(null, 'password');

    }
    else if (command === 'logout') {

      newTitle = '<i class="el-icon-switch-button"><span class="me-2"></span>退出系统';
      newLink = utils.getPageUrl(null, 'logout');

    }

    utils.addTab(newTitle, newLink);
  }
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    document.title = DOCUMENTTITLE_HOME;
    this.apiGet();
  },
  computed: {
    leftWidth: function () {
      if (this.isDesktop) {
        return this.isCollapse ? $collapseWidth : $sidebarWidth;
      }
      return this.isMobileMenu ? this.winWidth : 0;
    }
  }
});
