var utils = {
  init: function (data) {
    return _.assign(
      {
        pageLoad: false,
        loading: null,
        euiSize: 'small',
        menuId: utils.getQueryString("menuId"),
        optionsABC: ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'],
      },
      data
    );
  },

  getQueryString: function (name, defaultValue) {
    var result = location.search.match(
      new RegExp("[?&]" + name + "=([^&]+)", "i")
    );
    if (!result || result.length < 1) {
      return defaultValue || "";
    }
    return decodeURIComponent(result[1]);
  },

  getQueryStringList: function (name) {
    var value = utils.getQueryString(name);
    if (value) {
      return value.split(",");
    }
    return [];
  },

  getQueryBoolean: function (name) {
    var result = location.search.match(
      new RegExp("[?&]" + name + "=([^&]+)", "i")
    );
    if (!result || result.length < 1) {
      return false;
    }
    return result[1] === "true" || result[1] === "True";
  },

  getQueryInt: function (name, defaultValue) {
    var result = location.search.match(
      new RegExp("[?&]" + name + "=([^&]+)", "i")
    );
    if (!result || result.length < 1) {
      return defaultValue || 0;
    }
    return utils.toInt(result[1]);
  },

  getQueryIntList: function (name) {
    var result = location.search.match(
      new RegExp("[?&]" + name + "=([^&]+)", "i")
    );
    if (!result || result.length < 1) {
      return [];
    }
    return _.map(result[1].split(","), function (x) {
      return utils.toInt(x);
    });
  },
  getQueryIntList: function (name) {
    var value = utils.getQueryString(name);
    if (value) {
      return _.map(value.split(","), function (item) {
        return parseInt(item, 10);
      });
    }
    return [];
  },
  loadExternals: function (cssUrls, jsUrls) {
    if (cssUrls) {
      var head = document.getElementsByTagName('head')[0];
      for (var i = 0; i < cssUrls.length; i++) {
        var url = cssUrls[i];
        var link = document.createElement('link');
        link.href = url;
        link.rel = 'stylesheet';
        link.type = 'text/css';
        head.appendChild(link);
      }
    }
    if (jsUrls) {
      var head = document.getElementsByTagName('head')[0];
      for (var i = 0; i < jsUrls.length; i++) {
        var url = jsUrls[i];
        var script = document.createElement('script');
        script.src = url;
        script.type = 'text/javascript';
        head.appendChild(script);
      }
    }
  },

  getEditor: function (attributeName, height) {
    return UE.getEditor(attributeName, {
      allowDivTransToP: false,
      maximumWords: 99999999,
      initialFrameWidth: null,
      initialFrameHeight: height && height > 0 ? height : 320,
      autoHeightEnabled: false,
      autoFloatEnabled: false,
      zIndex: 2000,
    });
  },
  formatFileSize: function (size) {
    var kb = 1024;
    var mb = kb * 1024;
    var gb = mb * 1024;

    if (size >= gb) {
      return (size / mb).toFixed(2) + 'MB';
    } else if (size >= mb) {
      return (size / mb).toFixed(2) + 'MB';
    } else if (size >= kb) {
      return (size / kb).toFixed(2) + 'KB';
    } else if (size > 0) {
      return size + 'B';
    }
    else {
      return '0B';
    }
  },
  formatPercentFloat: function (count, total) {
    if (count > 0 && total > 0) {
      if (count >= total) {
        return 100;
      }
      return (Math.round(count / total * 10000) / 100.00);
    }
    return 0.00
  },
  formatAvg: function (count, total) {
    if (count > 0 && total > 0) {
      return ((count / total).toFixed(2));
    }
    return 0.00
  },
  formatDuration: function (duration) {
    var fen = "00";
    var miao = "00";
    if (duration < 60) {
      fen = "00";
      if (duration < 10) {
        miao = "0" + duration;
      }
      else {
        miao = duration;
      }

    }
    if (duration === 60) {
      fen = "00";
      miao = "60";
    }
    if (duration > 60) {
      fen = Math.trunc(duration / 60);
      if (fen < 10) {
        fen = "0" + fen;
      }
      miao = Math.trunc(duration % 60);
      if (miao < 10) {
        miao = "0" + miao;
      }
    }

    return fen + ":" + miao;
  },
  formatDurationCN: function (duration) {

    if (duration > 3600) {
      var shi = Math.trunc(duration / 3600);
      var fen = Math.trunc(duration / 60);
      return shi + "时" + miao + "分";
    }
    else {
      if (duration <= 60) {
        return duration + "秒";
      }
      else {
        fen = Math.trunc(duration / 60);
        miao = Math.trunc(duration % 60);
        return fen + "分" + miao + "秒";
      }

    }

  },
  toCamelCase: function (s) {
    if (!s || s[0] !== s[0].toUpperCase()) {
      return s;
    }
    var chars = s.split('');
    var values = s.split('');
    for (var i = 0; i < chars.length; i++) {
      if (i == 1 && chars[i] !== chars[i].toUpperCase()) {
        return values.join('');
      }
      var hasNext = (i + 1) < chars.length;
      if (i > 0 && hasNext && chars[i + 1] !== chars[i + 1].toUpperCase()) {
        return values.join('');
      }
      if (utils.isNumeric(chars[i])) {
        return values.join('');
      }
      values[i] = _.toLower(chars[i]);
    }
    return values.join('');
  },

  toInt: function (val) {
    if (!val) return 0;
    if (typeof val === 'number') return val;
    return parseInt(val, 10) || 0;
  },

  toArray: function (val) {
    return (val || '').split(',');
  },

  formatDate: function (date) {
    var d = new Date(date),
      month = '' + (d.getMonth() + 1),
      day = '' + d.getDate(),
      year = d.getFullYear();

    if (month.length < 2)
      month = '0' + month;
    if (day.length < 2)
      day = '0' + day;

    return [year, month, day].join('-');
  },

  isNumeric: function (str) {
    return /^\d+$/.test(str);
  },


  getIndexUrl: function (query) {
    var url = $rootUrl + "/";
    if (query) {
      url += "?";
      _.forOwn(query, function (value, key) {
        url += key + "=" + encodeURIComponent(value) + "&";
      });
      url = url.substr(0, url.length - 1);
    }
    return url;
  },
  getRootUrl: function (name, query) {
    return utils.getPageUrl(null, name, query);
  },

  getAssetsUrl: function (url) {
    return "/sitefiles/assets/" + url;
  },

  getXblUrl: function (name, query) {
    return utils.getPageUrl("xbl", name, query);
  },

  getExamUrl: function (name, query) {
    return utils.getPageUrl("exam", name, query);
  },

  getStudyUrl: function (name, query) {
    return utils.getPageUrl("study", name, query);
  },
  getKnowledgesUrl: function (name, query) {
    return utils.getPageUrl("knowledges", name, query);
  },

  getSettingsUrl: function (name, query) {
    return utils.getPageUrl("settings", name, query);
  },
  getPointsUrl: function (name, query) {
    return utils.getPageUrl("points", name, query);
  },
  getGiftUrl: function (name, query) {
    return utils.getPageUrl("gift", name, query);
  },

  getCommonUrl: function (name, query) {
    return utils.getPageUrl("common", name, query);
  },

  getPageUrl: function (prefix, name, query) {
    var url = $rootUrl + "/";
    if (prefix) {
      url += prefix + "/" + name + "/";
    } else {
      url += name + "/";
    }
    if (this.contains(url, 'xblms-admin/login') || this.contains(url, 'home/login') || this.contains(url, 'app/login')) {
      url += "?par=" + this.uuid();
    }
    else {
      url += "?par=" + this.uuid() + "&menuId=" + utils.getQueryString("menuId");
    }

    if (query) {
      url += "&";
      _.forOwn(query, function (value, key) {
        url += key + "=" + encodeURIComponent(value) + "&";
      });
      url = url.substr(0, url.length - 1);
    }
    return url;
  },

  getCountName: function (attributeName) {
    return utils.toCamelCase(attributeName + "Count");
  },

  getExtendName: function (attributeName, n) {
    return utils.toCamelCase(n ? attributeName + n : attributeName);
  },
  openAdminView(id) {
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getCommonUrl('adminLayerView', { id: id }),
      width: "68%",
      height: "88%",
    });
  },
  openUserView(id) {
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getCommonUrl('userLayerView', { id: id }),
      width: "68%",
      height: "88%",
    });
  },
  pad: function (num) {
    var s = num + "";
    while (s.length < 2) s = "0" + s;
    return s;
  },

  getUrl: function (siteUrl, url) {
    if (url && (url.startsWith('/') || url.indexOf('://') != -1)) return url;
    siteUrl = _.trimEnd(siteUrl, '/');
    return siteUrl + '/' + _.trimStart(_.trimStart(_.trimStart(url, '~'), '@'), '/');
  },

  getFriendlyDate: function (date) {
    if (Object.prototype.toString.call(date) !== '[object Date]') {
      date = new Date(date);
    }
    var now = new Date();
    var delta = Math.round((now - date) / 1000);
    if (delta > 0) {
      var minute = 60, hour = minute * 60, day = hour * 24;
      if (delta < day) {
        return '今天';
      }
      if (delta < day * 2) {
        return '昨天';
      }
      if (date.getFullYear() === now.getFullYear()) {
        return utils.pad(date.getMonth() + 1) + '月' + utils.pad(date.getDate()) + '日';
      }
    }
    return date.getFullYear() + '-' + utils.pad(date.getMonth() + 1) + '-' + utils.pad(date.getDate());
  },

  getFriendlyDateTime: function (date) {
    if (Object.prototype.toString.call(date) !== '[object Date]') {
      date = new Date(date);
    }
    var now = new Date();
    var delta = Math.round((now - date) / 1000);
    if (delta > 0) {
      var minute = 60, hour = minute * 60, day = hour * 24;
      if (delta < day) {
        return '今天 ' + utils.pad(date.getHours()) + ':' + utils.pad(date.getMinutes()) + ':' + utils.pad(date.getSeconds());
      }
      if (delta < day * 2) {
        return '昨天 ' + utils.pad(date.getHours()) + ':' + utils.pad(date.getMinutes()) + ':' + utils.pad(date.getSeconds());
      }
      if (date.getFullYear() === now.getFullYear()) {
        return utils.pad(date.getMonth() + 1) + '月' + utils.pad(date.getDate()) + '日 ' + utils.pad(date.getHours()) + ':' + utils.pad(date.getMinutes()) + ':' + utils.pad(date.getSeconds());
      }
    }
    return date.getFullYear() + '-' + utils.pad(date.getMonth() + 1) + '-' + utils.pad(date.getDate()) + ' ' + utils.pad(date.getHours()) + ':' + utils.pad(date.getMinutes()) + ':' + utils.pad(date.getSeconds());
  },

  getRootVue: function () {
    return top.$vue || window.$vue;
  },

  getTabVue: function (name) {
    if (!name) {
      return window.$vue;
    }
    var $this = utils.getRootVue();
    var tab = $this.tabs.find(function (tab) {
      return tab.name == name;
    });
    if (tab) {
      var iframe = top.document.getElementById('frm-' + tab.name).contentWindow;
      return iframe.$vue;
    }
    return null;
  },

  getTabName: function () {
    var $this = utils.getRootVue();
    return $this.tabName;
  },

  openTab: function (name) {
    var $this = utils.getRootVue();
    var index = $this.tabs.findIndex(function (tab) {
      return tab.name == name;
    });
    if (index !== -1) {
      $this.tabName = name;
    }
  },

  focusTab: function (name) {

    if (!name) {
      name = utils.getTabName();
    }
    setTimeout(function () {
      var ele = top.document.getElementById('frm-' + name);
      if (ele) {
        ele.contentWindow.focus();
      }
    }, 100);
  },

  addTab: function (title, url) {
    var $this = utils.getRootVue();
    var index = $this.tabs.findIndex(function (tab) {
      return tab.url == url;
    });

    var tab = null;
    if (index === -1) {
      tab = {
        title: title,
        name: utils.uuid(),
        url: url,
      };
      $this.tabs.push(tab);
      utils.focusTab(tab.name);
    } else {
      tab = $this.tabs[index];
      var iframe = top.document.getElementById('frm-' + tab.name).contentWindow;
      iframe.location.href = url;
    }
    $this.tabName = tab.name;
  },

  removeTab: function (name) {
    var $this = utils.getRootVue();
    if (!name) {
      name = $this.tabName;
    }

    if ($this.tabName === name) {
      $this.activeChildMenu = null;
      $this.tabs.forEach(function (tab, index) {
        if (tab.name === name) {
          var nextTab = $this.tabs[index + 1] || $this.tabs[index - 1];
          if (nextTab) {
            $this.tabName = nextTab.name;
          }
        }
      });
    }

    $this.tabs = $this.tabs.filter(function (tab) {
      return tab.name !== name;
    });
  },

  addQuery: function (url, query) {
    if (!url) return '';
    url += (url.indexOf('?') === -1 ? '?' : '&');
    _.forOwn(query, function (value, key) {
      url += key + "=" + encodeURIComponent(value) + "&";
    });
    return url.substr(0, url.length - 1);
  },


  alertInfo: function (config) {
    if (!config) return false;

    alert({
      title: config.title,
      text: config.text,
      type: "info",
      confirmButtonText: config.button || "确 定",
      confirmButtonClass: "el-button el-button--primary",
      cancelButtonClass: "el-button el-button--default",
      showCancelButton: true,
      cancelButtonText: "取 消",
    }).then(function (result) {
      if (result.value && config.callback) {
        config.callback();
      }
    });

    return false;
  },
  alertDelete: function (config) {
    if (!config) return false;

    alert({
      title: config.title,
      text: config.text,
      type: "warning",
      confirmButtonText: config.button || "删 除",
      confirmButtonClass: "el-button el-button--danger",
      cancelButtonClass: "el-button el-button--default",
      showCancelButton: true,
      cancelButtonText: "取 消",
    }).then(function (result) {
      if (result.value && config.callback) {
        config.callback();
      }
    });

    return false;
  },

  alertSuccess: function (config) {
    if (!config) return false;

    alert({
      title: config.title,
      text: config.text,
      type: "success",
      confirmButtonText: config.button || "确 定",
      cancelButtonText: "取 消",
      confirmButtonClass: "el-button el-button--primary",
      showCancelButton: config.showCancelButton || false
    }).then(function (result) {
      if (result.value && config.callback) {
        config.callback();
      }
    });

    return false;
  },

  alertWarning: function (config) {
    if (!config) return false;

    alert({
      title: config.title,
      text: config.text,
      type: "warning",
      confirmButtonText: config.button || "确 定",
      cancelButtonText: "取 消",
      confirmButtonClass: "el-button el-button--warning",
      showCancelButton: config.showCancelButton || true
    }).then(function (result) {
      if (result.value && config.callback) {
        config.callback();
      }
    });

    return false;
  },
  alertExamWarning: function (config) {
    if (!config) return false;

    alert({
      title: config.title,
      text: config.text,
      type: "warning",
      confirmButtonText: config.button || "确 定",
      cancelButtonText: "取 消",
      confirmButtonClass: "el-button el-button--warning",
      showCancelButton: false
    }).then(function (result) {
      if (result.value && config.callback) {
        config.callback();
      }
    });

    return false;
  },

  getErrorMessage: function (error) {
    if (error.response && error.response.status === 500) {
      return JSON.stringify(error.response.data);
    }

    var message = error.message;
    if (error.response && error.response.data) {
      if (error.response.data.exceptionMessage) {
        message = error.response.data.exceptionMessage;
      } else if (error.response.data.message) {
        message = error.response.data.message;
      }
    }

    return message;
  },

  uuid: function () {
    return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(
      /[xy]/g,
      function (c) {
        var r = (Math.random() * 16) | 0,
          v = c == "x" ? r : (r & 0x3) | 0x8;
        return v.toString(16);
      }
    );
  },

  notifySuccess: function (message, position) {
    utils.getRootVue().$notify.success({
      title: '成功',
      message: message,
      position: position || 'top-right'
    });
  },

  notifyWarning: function (message, position) {
    utils.getRootVue().$notify.warning({
      title: '警告',
      message: message,
      position: position || 'top-right'
    });
  },

  notifyInfo: function (message, position) {
    utils.getRootVue().$notify.info({
      title: '提示',
      message: message,
      position: position || 'top-right'
    });
  },

  notifyError: function (error, position) {
    if (!error) return;

    var message = '';
    if (error.response) {
      message = utils.getErrorMessage(error);
    } else if (typeof error === 'string') {
      message = error;
    } else {
      message = error + '';
    }

    utils.getRootVue().$notify.error({
      title: '错误',
      message: message,
      position: position || 'top-right'
    });
  },

  success: function (message, options) {
    var vue = (options && options.layer) ? utils.getTabVue() : utils.getRootVue();

    vue.$message({
      type: "success",
      message: message,
      showIcon: true,
      showClose: true,
      customClass: 'el-message-extend'
    });
  },

  warning: function (message, options) {
    var vue = (options && options.layer) ? utils.getTabVue() : utils.getRootVue();

    vue.$message({
      type: "warning",
      message: message,
      showIcon: true,
      showClose: true,
      customClass: 'el-message-extend'
    });
  },

  error: function (error, options) {
    if (!error) return;
    var vue = (options && options.layer) ? utils.getTabVue() : utils.getRootVue();

    if (typeof error === 'string') {
      if (options && options.redirect) {
        var uuid = utils.uuid();
        sessionStorage.setItem(uuid, JSON.stringify({
          message: error
        }));
        if (options.current) {
          location.href = utils.getRootUrl("error", { uuid: uuid });
        } else {
          top.location.href = utils.getRootUrl("error", { uuid: uuid });
        }
      } else {
        vue.$message({
          type: "error",
          message: error,
          showIcon: true,
          showClose: true,
          customClass: 'el-message-extend'
        });
      }
    } else if (error.response) {
      var message = utils.getErrorMessage(error);

      var ignoreAuth = (options && options.ignoreAuth) ? true : false;

      if (!ignoreAuth && error.response && (error.response.status === 401 || error.response.status === 403)) {
        var location = _.trimEnd(window.location.href, '/');
        if (_.endsWith(location, '/xblms-admin') || _.endsWith(location, '/home') || _.endsWith(location, '/app')) {
          top.location.href = utils.getRootUrl('login', { status: 401 });

        } else {
          top.location.href = utils.getRootUrl('login', { status: 401 });

        }
      } else if (error.response && error.response.status === 500 || options && options.redirect) {
        var uuid = utils.uuid();

        sessionStorage.setItem(uuid, message);

        if (options && options.redirect) {
          top.location.href = utils.getRootUrl("error", { uuid: uuid })
          return;
        }

        top.utils.openLayer({
          title: false,
          closebtn: 0,
          width: "88%",
          height: "88%",
          url: utils.getRootUrl("error", { uuid: uuid }),
        });
        return;
      } else if (error.response && error.response.status === 400) {
        if (options && options.redirect) {
          var uuid = utils.uuid();
          sessionStorage.setItem(uuid, JSON.stringify({
            message: error
          }));

          top.location.href = utils.getRootUrl("error", { uuid: uuid });
        }
      }

      vue.$message({
        type: "error",
        message: message,
        showIcon: true,
        showClose: true,
        customClass: 'el-message-extend'
      });
    } else if (typeof error === 'object') {
      vue.$message({
        type: "error",
        message: error + '',
        showIcon: true,
        showClose: true,
        customClass: 'el-message-extend'
      });
    }
  },

  loading: function (app, isLoading, text) {
    if (isLoading) {
      if (app.pageLoad) {
        app.loading = app.$loading({ text: text || '页面加载中...' });
      }
    } else {
      app.loading ? app.loading.close() : (app.pageLoad = true);
    }
  },

  scrollTop: function () {
    document.documentElement.scrollTop = document.body.scrollTop = 0;
  },

  closeLayer: function (reload) {
    if (reload) {
      parent.location.reload();
    } else {
      parent.layer.closeAll();
    }
    return false;
  },
  closeLayerSelf: function () {
    var index = parent.layer.getFrameIndex(window.name);
    parent.layer.close(index);
  },
  openTopLeft: function (title, src, width) {
    top.$vue.topFrameTitle = title;
    top.$vue.topFrameSrc = src;
    top.$vue.topFrameWidth = width || 88;
    top.$vue.topFrameDrawer = true;
  },
  closeTopLeft: function () {
    top.$vue.topFrameDrawer = false;
  },
  openTopRight: function (title, src, width) {
    top.$vue.topRightFrameTitle = title;
    top.$vue.topRightFrameSrc = src;
    top.$vue.topRightFrameWidth = width || 88;
    top.$vue.topRightFrameDrawer = true;
  },
  closeTopRight: function () {
    top.$vue.topRightFrameDrawer = false;
  },
  openTopTop: function (title, src, width) {
    top.$vue.topTopFrameTitle = title;
    top.$vue.topTopFrameSrc = src;
    top.$vue.topTopFrameWidth = width || 50;
    top.$vue.topTopFrameDrawer = true;
  },
  closeTopTop: function () {
    top.$vue.topTopFrameDrawer = false;
  },
  openLayerPhoto: function (config) {
    layer.photos({
      photos: {
        "title": config.title || "", //相册标题
        "id": config.id || 0, //相册id
        "start": 0, //初始显示的图片序号，默认0
        "data": [   //相册包含的图片，数组格式
          {
            "alt": config.title || "",
            "pid": config.id || 0, //图片id
            "src": config.src || "", //原图地址
            "thumb": "" //缩略图地址
          }
        ]
      }
      , anim: 5 //0-6的选择，指定弹出图片动画类型，默认随机（请注意，3.0之前的版本用shift参数）
    });
  },
  openLayer: function (config) {
    if (!config || !config.url) return false;

    if (!config.width) {
      config.width = ($(window).width() - 50) + 'px';
    } else {
      var width = config.width + '';
      if (width.indexOf('%') == -1 && width.indexOf('px') == -1) {
        config.width = width + 'px';
      }
    }
    if (!config.height) {
      config.height = ($(window).height() - 50) + 'px';
    } else {
      var height = config.height + '';
      if (height.indexOf('%') == -1 && height.indexOf('px') == -1) {
        config.height = height + 'px';
      }
    }

    var index = layer.open({
      id: config.id || null,
      type: 2,
      btn: null,
      title: config.title,
      area: [config.width, config.height],
      maxmin: false,
      resize: false,
      shadeClose: true,
      closeBtn: config.closebtn,
      content: config.url,
      success: config.success,
      end: config.end,
      offset: config.offset || 'auto'
    });

    setTimeout(function () {
      document.getElementById('layui-layer-iframe' + index).focus();
    }, 100);

    if (config.max) {
      layer.full(index);
    }

    return false;
  },

  pointNotice: function (notice) {
    if (notice.success) {
      layer.msg('<div class="p-3 text-center"><div class="pb-3"><i class="bi bi-database-check fs-1 text-warning fw-bolder"></i></div>' + notice.msg + '<div class="pt-3 fs-3"><i class="bi bi-plus me-2"></i>' + notice.value + '</div></div>', { shade: 0.5,time:1500 });
    }
  },

  contains: function (str, val) {
    return str && val && str.indexOf(val) !== -1;
  },

  keyPress: function (submitFn, cancelFn) {
    $(document).keydown(function (e) {
      if ((e.ctrlKey && e.key !== "Control" && (e.keyCode || e.which) == 83) ||
        (e.ctrlKey && e.which == 13 || e.which == 10) ||
        (e.shiftKey && e.which == 13 || e.which == 10)) {
        e.preventDefault();
        e.stopPropagation();
        submitFn && submitFn();
      } else if (e.key === 'Escape') {
        e.preventDefault();
        e.stopPropagation();
        cancelFn && cancelFn();
      } else if (e.key === 'F1') {
        e.preventDefault();
        e.stopPropagation();
        var url = location.href;
        if (url.indexOf('/xblms-admin/') !== -1) {
          url = url.substring(url.indexOf('/xblms-admin/'));
        }
        utils.openDocs(url);
      }
    });
  },

  focus: function (vue, ref) {
    setTimeout(function () {
      vue.$refs[ref] && vue.$refs[ref].focus();
    }, 100);
  },

  ctrlSave: function (submitFn) {
    $(document).keydown(function (e) {
      var c = e.which || e.keyCode;
      if (e.ctrlKey && c == 83) {
        e.preventDefault();
        submitFn && submitFn();
      }
    });
  },

  validateMobile: function (rule, value, callback) {
    if (!value) {
      callback();
    } else if (!/^1[3-9]\d{9}$/.test(value)) {
      callback(new Error(rule.message || '字段必须是有效的手机号码'));
    } else {
      callback()
    }
  },

  validateDecimal: function (rule, value, callback) {
    if (!value) {
      callback();
    } else if (!/^-?\d+(\.\d{1,2})?$/.test(value)) {
      callback(new Error(rule.message || '字段必须是数字'));
    } else {
      callback()
    }
  },

  validateDigits: function (rule, value, callback) {
    if (!value) {
      callback();
    } else if (!/^-?\d+$/.test(value)) {
      callback(new Error(rule.message || '字段必须是整数'));
    } else {
      callback()
    }
  },

  validateMax: function (rule, value, callback) {
    if (value && value.length > parseInt(rule.value)) {
      callback(new Error(rule.message || '字段不能超过指定的长度'));
    } else {
      callback()
    }
  },

  validateMaxValue: function (rule, value, callback) {
    if (!value) {
      callback();
    } else if (!/^-?\d+(\.\d{1,2})?$/.test(value)) {
      callback(new Error(rule.message || '字段必须是数值，并且不能大于指定的值'));
    } else if (value && parseInt(value) > parseInt(rule.value)) {
      callback(new Error(rule.message || '字段必须是数值，并且不能大于指定的值'));
    } else {
      callback()
    }
  },

  validateMin: function (rule, value, callback) {
    if (value && value.length < parseInt(rule.value)) {
      callback(new Error(rule.message || '字段不能低于指定的长度'));
    } else {
      callback()
    }
  },

  validateMinValue: function (rule, value, callback) {
    if (!value) {
      callback();
    } else if (!/^-?\d+(\.\d{1,2})?$/.test(value)) {
      callback(new Error(rule.message || '字段必须是数值，并且不能小于指定的值'));
    } else if (value && parseInt(value) < parseInt(rule.value)) {
      callback(new Error(rule.message || '字段必须是数值，并且不能小于指定的值'));
    } else {
      callback()
    }
  },

  validateIdCard: function (rule, value, callback) {
    var reg = /(^\d{15}$)|(^\d{17}(\d|X|x)$)/;
    if (!value) {
      callback();
    } else if (!reg.test(value)) {
      callback(new Error(rule.message || '字段必须是身份证号码'));
    } else {
      callback()
    }
  },

  validateChinese: function (rule, value, callback) {
    if (!value) {
      callback();
    } else {
      var isAll = true;
      for (var i = 0; i < value.length; i++) {
        if (escape(value[i]).indexOf("%u") === -1) {
          isAll = false;
          continue;
        }
      }
      if (isAll) {
        callback()
      } else {
        callback(new Error(rule.message || '字段必须是中文'));
      }
    }
  },

  validateInt: function (rule, value, callback) {
    if (!value) {
      callback();
    } else if (!/^[-]?\d+$/.test(value)) {
      callback(new Error(rule.message || '字段必须是有效的数字值'));
    } else {
      callback()
    }
  },

  getRules: function (rules) {
    var options = [
      { required: "字段为必填项" },
      { email: "字段必须是有效的电子邮件" },
      { mobile: "字段必须是有效的手机号码" },
      { url: "字段必须是有效的url" },
      { alpha: "字段只能包含英文字母" },
      { alphaDash: "字段只能包含英文字母、数字、破折号或下划线" },
      { alphaNum: "字段只能包含英文字母或数字" },
      { alphaSpaces: "字段只能包含英文字母或空格" },
      { decimal: "字段必须是数字" },
      { digits: "字段必须是整数" },
      { max: "字段不能超过指定的长度" },
      { maxValue: "字段必须是数值，并且不能大于指定的值" },
      { min: "字段不能低于指定的长度" },
      { minValue: "字段必须是数值，并且不能小于指定的值" },
      { regex: "字段必须匹配指定的正则表达式" },
      { chinese: "字段必须是中文" },
      { zip: "字段必须是邮政编码" },
      { idCard: "字段必须是身份证号码" },
    ];

    if (rules) {
      var array = [];
      for (var i = 0; i < rules.length; i++) {
        var rule = rules[i];
        var ruleType = utils.toCamelCase(rule.type);

        if (ruleType === "required") {
          array.push({
            required: true,
            message: rule.message || options.required,
          });
        } else if (ruleType === "email") {
          array.push({
            type: "email",
            message: rule.message || options.email
          });
        } else if (ruleType === "mobile") {
          array.push({
            validator: utils.validateMobile,
            message: rule.message || options.mobile
          });
        } else if (ruleType === "url") {
          array.push({
            type: "url",
            message: rule.message || options.url
          });
        } else if (ruleType === "alpha") {
          array.push({
            type: "string",
            pattern: /^[a-zA-Z]+$/,
            message: rule.message || options.alpha
          });
        } else if (ruleType === "alphaDash") {
          array.push({
            type: "string",
            pattern: /^[a-zA-Z0-9_-]+$/,
            message: rule.message || options.alphaDash,
          });
        } else if (ruleType === "alphaNum") {
          array.push({
            type: "string",
            pattern: /^[a-zA-Z0-9]+$/,
            message: rule.message || options.alphaNum,
          });
        } else if (ruleType === "alphaSpaces") {
          array.push({
            type: "string",
            pattern: /^[a-zA-Z\s]+$/,
            message: rule.message || options.alphaSpaces,
          });
        } else if (ruleType === "decimal") {
          array.push({
            validator: utils.validateDecimal,
            message: rule.message || options.decimal
          });
        } else if (ruleType === "digits") {
          array.push({
            validator: utils.validateDigits,
            message: rule.message || options.digits
          });
        } else if (ruleType === "max") {
          array.push({
            validator: utils.validateMax,
            message: rule.message || options.max,
            value: rule.value
          });
        } else if (ruleType === "maxValue") {
          array.push({
            validator: utils.validateMaxValue,
            message: rule.message || options.maxValue,
            value: rule.value
          });
        } else if (ruleType === "min") {
          array.push({
            validator: utils.validateMin,
            message: rule.message || options.min,
            value: rule.value
          });
        } else if (ruleType === "minValue") {
          array.push({
            validator: utils.validateMinValue,
            message: rule.message || options.minValue,
            value: rule.value
          });
        } else if (ruleType === "regex" && rule.value) {
          var re = new RegExp(rule.value, "ig");
          array.push({
            type: "string",
            pattern: re,
            message: rule.message || options.regex,
          });
        } else if (ruleType === "chinese") {
          array.push({
            validator: utils.validateChinese,
            message: rule.message || options.chinese,
          });
        } else if (ruleType === "zip") {
          array.push({
            type: "string",
            pattern: /^[0-9]{6,6}$/,
            message: rule.message || options.zip,
          });
        } else if (ruleType === "idCard") {
          array.push({
            validator: utils.validateIdCard,
            message: rule.message || options.idCard,
          });
        }
      }

      return array;
    }
    return null;
  },

  scrollToError: function () {
    setTimeout(function () {
      var element = $('.el-form-item__error')[0];
      if (element) {
        var input = $(element).parent().parent()[0];
        if (input) {
          input.scrollIntoView();
        }
      }
    }, 100);
  },
  AESGetKey: function (salt) {
    return salt.substring(8, 40);
  },
  AESGetIV: function (salt) {
    return salt.substring(46, 62);
  },
  AESEncrypt: function (plainText, salt) {
    let key = utils.AESGetKey(salt);
    let iv = utils.AESGetIV(salt);
    const dataStr = typeof plainText === 'object' ? JSON.stringify(plainText) : String(plainText)
    const encrypted = CryptoJS.AES.encrypt(dataStr, CryptoJS.enc.Utf8.parse(key), {
      iv: CryptoJS.enc.Utf8.parse(iv),
      mode: CryptoJS.mode.CBC,
      padding: CryptoJS.pad.Pkcs7
    })
    return encrypted.toString()
  },
  AESDecrypt: function (plainText, salt) {
    let key = utils.AESGetKey(salt);
    let iv = utils.AESGetIV(salt);
    const decrypt = CryptoJS.AES.decrypt(plainText, CryptoJS.enc.Utf8.parse(key), {
      iv: CryptoJS.enc.Utf8.parse(iv),
      mode: CryptoJS.mode.CBC,
      padding: CryptoJS.pad.Pkcs7
    })
    return decrypt.toString(CryptoJS.enc.Utf8)
  },
};

Object.defineProperty(Object.prototype, "getEntityValue", {
  value: function (t) {
    var e;
    for (e in this) if (e.toLowerCase() == t.toLowerCase()) return this[e];
  },
});

if (window.swal && swal.mixin) {
  var alert = swal.mixin({
    confirmButtonClass: "el-button el-button--primary",
    cancelButtonClass: "el-button el-button--default",
    buttonsStyling: false,
  });
}

var PER_PAGE = 15;
var DEFAULT_AVATAR_URL = '/sitefiles/assets/images/default_avatar.png';
var DEFAULT_NOBODY_AVATAR_URL = '/sitefiles/assets/images/nobody_avatar.svg';
var DEFAULT_AVATAR_BG_URL = '/sitefiles/assets/images/default_avatar_bg.jpg';
var DEFAULT_LOGO_URL = "/sitefiles/assets/images/logo.png";
var DEFAULT_LOGINBG_URL = '/sitefiles/assets/images/cover/bg.png';

var DOCUMENTTITLE = 'XBLMS.EXAM';
var DOCUMENTTITLECN = '星期八在线考试系统';

var sessionId = utils.getQueryString('sessionId');
var accessToken = utils.getQueryString('accessToken');
if (sessionId && accessToken) {
  localStorage.setItem(SESSION_ID_NAME, sessionId);
  localStorage.removeItem(ACCESS_TOKEN_NAME);
  sessionStorage.removeItem(ACCESS_TOKEN_NAME);
  localStorage.setItem(ACCESS_TOKEN_NAME, accessToken);
}

var $token = sessionStorage.getItem(ACCESS_TOKEN_NAME) || localStorage.getItem(ACCESS_TOKEN_NAME) || utils.getQueryString('accessToken');
var $api = axios.create({
  baseURL: $apiUrl,
  headers: {
    Permission: utils.getQueryString("menuId"),
    Authorization: "Bearer " + $token
  },
});

$api.csrfPost = function (csrfToken, url, data) {
  return $api.post(url, data, {
    headers: {
      "X-CSRF-TOKEN": csrfToken
    }
  });
}
