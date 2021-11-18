var app = app || {};
app.painel = app.painel || {};
app.painel.model = app.painel.model || {};

app.painel.model.Servico = function (data) {
    if (data == null || data.hubChannel == null) {
        console.error("Serviço sem identificação");
        return;
    }

    var channelId = ko.observable(data.hubChannel);
    var category = ko.observable(data.category);
    var title = ko.observable(data.channelName);
    var status = ko.observable(data.code);
    var message = ko.observable(data.message);
    var date = ko.observable(data.date);
    var updateTime = ko.observable(data.updateTime);
    var startTime = ko.observable(data.startTime);
    var endTime = ko.observable(data.endTime);

    var set = function (data) {
        status(data.code);
        message(data.message);
        date(data.date);
        updateTime(data.updateTime);
        startTime(data.startTime);
        endTime(data.endTime);
    };

    return {
        channelId: channelId,
        category: category,
        title: title,
        status: status,
        message: message,
        date: date,
        set: set,
        startTime: startTime,
        endTime: endTime,
        updateTime: updateTime
    };
}