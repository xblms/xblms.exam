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
  form: {
    groupType:'All'
  }
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

      if (res.group.id > 0) {
        //$this.form = res.group;
        $this.form = _.assign({}, res.group);
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
    $api.post($urlPost, { selectOrgans: $this.selectOrgans,group: $this.form }).then(function (response) {
      utils.success('操作成功！');
      utils.closeLayer(false);
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },

  btnSubmitClick: function () {
    var $this = this;

    if ($this.form.groupType === 'Range') {
      var selectNodes = this.$refs.organsTree.getCheckedNodes();
      if (selectNodes && selectNodes.length > 0) {
        selectNodes.forEach((node) => {
          $this.selectOrgans.push({ id: node.id, type: node.organType });
        });
      }
    }

    this.$refs.form.validate(function (valid) {
      if (valid) {
        $this.apiSubmit();
      }
    });
  },

  btnCancelClick: function () {
    utils.closeLayer();
  },
  filterNode(value, data) {
    if (!value) return true;
    return data.name.indexOf(value) !== -1;
  }
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
