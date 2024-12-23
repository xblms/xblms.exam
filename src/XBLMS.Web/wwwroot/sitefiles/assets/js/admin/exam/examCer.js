var $url = 'exam/examCer';

var data = utils.init({
  list: [],
  form: { title: '' },

});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url, { params: this.form }).then(function (response) {
      var res = response.data;
      $this.list = res.items;

    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  apiDelete: function (id) {
    var $this = this;

    utils.loading(this, true);
    $api.post($url + '/del', { id: id }).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success("操作成功");
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.apiGet();
    });
  },
  btnSearchClick: function () {
    this.apiGet();
  },
  btnEditClick: function (cer) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examCerEdit', { id: cer.id }),
      width: "98%",
      height: "98%",
      end: function () {
        $this.apiGet();
      }
    });
  },
  btnViewCer: function (cer) {
    top.utils.openLayerPhoto({
      title: cer.name,
      id: cer.id,
      src: cer.backgroundImg.replace('/cer/', '/cer/preview_cer_') + '?r=' + Math.random()
    })
  },
  btnAddClick: function () {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examCerEdit'),
      width: "98%",
      height: "98%",
      end: function () {
        $this.apiGet();
      }
    });
  },
  getAddUrl: function () {
    return utils.getExamUrl('examCertificateEdit', {
      id: -1,
      classifyId: this.classifyId,
      tabName: utils.getTabName()
    });
  },

  getEditUrl: function (id) {
    return utils.getExamUrl('examCertificateEdit', {
      id: id,
      classifyId: this.classifyId,
      tabName: utils.getTabName()
    });
  },
  btnDeleteClick: function (cer) {
    var $this = this;
    if (cer.paperCount > 0 || cer.userCount > 0) {
      utils.error("不能删除被使用的证书");
    }
    else {
      top.utils.alertDelete({
        title: '删除证书模板',
        text: '此操作将删除证书模板 ' + cer.name + '，确定删除吗？',
        callback: function () {
          $this.apiDelete(cer.id);
        }
      });
    }
  },

  btnSubmitClick: function () {
    var $this = this;
    this.$refs.form.validate(function (valid) {
      if (valid) {
        $this.apiSubmit();
      }
    });
  },

  btnCancelClick: function () {
    this.panel = false;
  },
  formatter: function (row) {
    for (var i = 0; i < this.typeList.length; i++) {
      if (this.typeList[i].value === row.txType) {
        return this.typeList[i].label;
      }
    }
  },
  showInput: function () {
    this.inputVisible = true;
    this.$nextTick(_ => {
      this.$refs.saveTagInput.$refs.input.focus();
    });
  },
  btnCountClick: function (row) {
    utils.openTopLeft(row.name + '-获证人员列表', utils.getExamUrl("examCerUsers", { id: row.id }));
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
