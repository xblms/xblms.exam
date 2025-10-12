var $url = '/settings/usersGroup';
var $urlGet = $url + '/actions/editGet';
var $urlPost = $url + '/actions/editPost';

var data = utils.init({
  id: utils.getQueryInt("id"),
  groupTypeSelects: null,
  organs: null,
  checkdKeys: null,
  expandedKeys: null,
  checkStrictly: false,
  filterText: '',
  selectOrgans: [],
  form: null
});

var methods = {
  apiGet: function () {
    var $this = this;
    $api.get($urlGet, {
      params: {
        id: this.id,
      }
    }).then(function (response) {
      var res = response.data;

      $this.organs = res.organs;
      $this.groupTypeSelects = res.groupTypeSelects;
      $this.form = _.assign({}, res.group);

      if (res.group.id > 0) {
        $this.checkdKeys = $this.expandedKeys = res.selectOrganIds;
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
    $api.post($urlPost, { selectOrgans: $this.organs, group: $this.form }).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success("操作成功");
        utils.closeLayerSelf();
      }
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },

  btnSubmitClick: function () {
    var $this = this;

    if (this.form.groupType === 'Range' && (this.organs === null || this.organs.length === 0)) {
      utils.error('请选择组织范围', { layer: true });
      return;
    }

    this.$refs.form.validate(function (valid) {
      if (valid) {
        $this.apiSubmit();
      }
    });
  },

  handleOrganTagClose(tag) {
    this.organs.splice(this.organs.indexOf(tag), 1);
  },
  btnSelectOrganClick: function () {
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getCommonUrl('selectOrgan', { windowName: window.name }),
      width: "60%",
      height: "88%"
    });
  },
  selectOrganCallback: function (selectCallbackList) {
    selectCallbackList.forEach(selectOrgan => {
      let existIndex = this.organs.findIndex(item => {
        return item.id === selectOrgan.id && item.type === selectOrgan.type;
      });

      if (existIndex < 0) {
        this.organs.push(selectOrgan)
      }
    });
  },

};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  watch: {
    filterText(val) {
      this.$refs.organsTree.filter(val);
    }
  },
  created: function () {
    this.apiGet();
  }
});
