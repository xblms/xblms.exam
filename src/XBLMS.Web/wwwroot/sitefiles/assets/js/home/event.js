var $url = '/event';

var data = utils.init({
  eventData: [],
  loadingEvent: true,
});

var methods = {
  apiGet: function () {
    var $this = this;
    $api.get($url, { params: { isApp: false } }).then(function (response) {
      var res = response.data;
      $this.eventData = res.list;
      $this.createEvent()
    }).catch(function (error) {
      $this.loadingEvent = false;
    }).then(function () {
      $this.loadingEvent = false;
    });
  },
  createEvent: function () {
    var $this = this;
    var calendarEl = document.getElementById('calendar');
    var calendar = new FullCalendar.Calendar(calendarEl, {
      height: '100%',
      expandRows: true,
      slotMinTime: '08:00',
      slotMaxTime: '20:00',
      headerToolbar: {
        left: 'prev,next today',
        center: 'title',
        right: 'multiMonthYear,dayGridMonth,timeGridWeek,timeGridDay,listWeek'
      },
      initialView: 'dayGridMonth',
      navLinks: true,
      editable: true,
      selectable: true,
      locale: "zh-cn",
      nowIndicator: true,
      dayMaxEvents: true,
      events: this.eventData,
      eventClick: function (arg) {
        $this.btnViewPaperClick(arg.event.id);
      },
    });

    calendar.render();
  },
  btnViewPaperClick: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examPaperInfo', { id: id }),
      width: "78%",
      height: "98%"
    });
  },
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    utils.loading(this, false);
    this.apiGet();
  },
});
