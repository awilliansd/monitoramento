var app = app || {};
app.painel = app.painel || {};
app.painel.model = app.painel.model || {};

app.painel.model.Grafico = function (data, dados) {
    if (data == null) {
        console.error("Grafico sem dados");
        return;
    }

    var _chart = null;
    var _type = ko.observable(data.chartType);
    var _title = ko.observable(data.titulo);
    var _legend = ko.observable(data.legenda);
    var _dados = ko.observableArray(dados);

    var setChart = function (chart) {
        _chart = chart;
    };

    return {
        type: _type,
        chart: _chart,
        title: _title,
        legend: _legend,
        dados: _dados,
        setChart: setChart
    };
};