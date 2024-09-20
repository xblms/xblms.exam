var $url = '/settings/block/add';

var data = utils.init({
  ruleId: utils.getQueryInt('id'),
  rule: null,
  areaTypes: null,
  blockAreas: null,
  form: null
});

var methods = {
  apiGet: function () {
    var $this = this;

    $api.get($url, {
      params: {
        ruleId: this.ruleId
      }
    }).then(function (response) {
      var res = response.data;

      $this.rule = res.rule;
      $this.areaTypes = res.areaTypes;
      $this.blockAreas = res.blockAreas;
      $this.form = _.assign({}, $this.rule);
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
      rule: this.form,
      blockAreas: this.blockAreas,
    }).then(function (response) {
      var res = response.data;

      utils.success('操作成功');
      utils.closeLayer();

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  addArea: function (id, name) {
    this.blockAreas.push({
      id: id,
      name: name
    });
  },

  addRange: function (isAllowList, range) {
    if (isAllowList) {
      this.form.allowList.push(range);
    } else {
      this.form.blockList.push(range);
    }
  },

  addChannel: function (id, name) {
    this.blockChannels.push({
      id: id,
      name: name
    });
  },

  handleAreaClose: function (id) {
    this.blockAreas = _.remove(this.blockAreas, function (n) {
      return id !== n.id;
    });
  },

  handleRangeClose: function (isAllowList, range) {
    if (isAllowList) {
      this.form.allowList = _.remove(this.form.allowList, function (n) {
        return range !== n;
      });
    } else {
      this.form.blockList = _.remove(this.form.blockList, function (n) {
        return range !== n;
      });
    }
  },

  handleChannelClose: function (id) {
    this.blockChannels = _.remove(this.blockChannels, function (n) {
      return id !== n.id;
    });
  },

  btnAreaAddClick: function () {
    utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getSettingsUrl('blockAddLayerAreaAdd'),
      width: '80%',
      height: '80%'
    });
  },

  btnRangeAddClick: function (isAllowList) {
    utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getSettingsUrl('blockAddLayerRangeAdd', { isAllowList: isAllowList }),
      width: '60%',
      height: '60%'
    });
  },

  btnSubmitClick: function () {
    var $this = this;
    this.$refs.form.validate(function (valid) {
      if (valid) {
        $this.apiSubmit();
      }
    });
  }
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  }
});
