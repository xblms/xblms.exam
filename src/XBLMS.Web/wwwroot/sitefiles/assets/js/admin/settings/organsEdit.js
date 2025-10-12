var $url = '/settings/organs';
var $urlInfo = $url + '/info';

var data = utils.init({
  id: utils.getQueryInt("id"),
  parentId: utils.getQueryInt("parentId"),
  type: utils.getQueryString("type"),
  companyId: utils.getQueryInt("companyId"),
  departmentId: utils.getQueryInt("departmentId"),
  parentType:utils.getQueryString("parentType"),
  form: { name:'',fullName:'' }
});

var methods = {
  apiGet: function () {
    var $this = this;
    if (this.id > 0) {
      $api.get($urlInfo, {
        params: {
          id: this.id,
          type: this.type
        }
      }).then(function (response) {
        var res = response.data;

        $this.userId = res.userId;
        $this.form.name = res.name;

      }).catch(function (error) {
        utils.error(error, { layer: true });
      }).then(function () {
        utils.loading($this, false);
      });
    }
    else {
      utils.loading($this, false);
    }

  },

  apiSubmit: function () {
    var $this = this;

    utils.loading(this, true);
    $api.post($url, {
      id: this.id,
      name: this.form.name,
      type: this.type,
      parentId: this.parentId,
      companyId: this.companyId,
      departmentId: this.departmentId,
      parentType:this.parentType
    }).then(function (response) {
      utils.success("操作成功");
      utils.closeLayer(false);
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },

  btnSubmitClick: function () {
    var $this = this;
    this.$refs.form.validate(function(valid) {
      if (valid) {
        $this.apiSubmit();
      }
    });
  },
  btnInsertDemoClick: function () {
    if (this.type === 'company') {
      this.form.name = "一级单位\n-二级单位\n--三级单位\n---四级单位\n----五级单位\n";
    }
    else {
      this.form.name = "一级部门\n-二级部门\n--三级部门\n---四级部门\n----五级部门\n";
    }
  },
  btnCancelClick: function () {
    utils.closeLayer();
  }
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  }
});
