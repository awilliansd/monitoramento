var app = app || {};
app.painel = app.painel || {};

app.painel.viewModel = (function ($, ko) {
    var self = this;

    self.categories = ko.observableArray();
    self.charts = ko.observableArray();
    self.usuarios = ko.observable();
    self.requisicoes = ko.observable();
    self.chartEmpty = ko.observable(true);
    self.latency = ko.observable("0");

    var collapseAll = function () {
        self.categories().forEach(function (ea) {
            ea.expand(false);
        });
    };

    self.sortCategories = function (left, right) {
        return left.categoryId() > right.categoryId() ? 1 : -1;
    };

    self.createChart = function (arrayData) {
        grafico = new app.painel.model.Grafico(arrayData[arrayData.length - 1], arrayData);
        self.charts.push(grafico);

        var element = 'idChart' + grafico.type();
        if (grafico.type() == 1)
            app.painel.apiChart.viewModel.startLogAPI(element, arrayData);
        else
            app.painel.apiChart.viewModel.startUsuario(element, arrayData);
    };

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/serverStatusHub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connect(connection);

    async function connect(conn) {
        conn.start()
            .then(function () {
                conn.invoke('getConnectionId')
                    .then(function (connectionId) {
                        console.log('Connectado, ID=' + connectionId);
                        conn.invoke("channelList", connectionId).catch(function (err) {
                            return console.error(err.toString());
                        });

                        conn.invoke("chartList", connectionId).catch(function (err) {
                            return console.error(err.toString());
                        });
                    });
            })
            .catch(function (err) {
                console.log(err);
                sleep(5000).then(function () {
                    connect(conn);
                });
            });
    }

    connection.onclose(function (e) {
        console.log('connection.onclose = ' + e);
        connect(connection);
    });

    async function sleep(msec) {
        return new Promise(resolve => setTimeout(resolve, msec));
    }

    connection.on("sendChannels", function (data) {
        console.log("Channels: ", data);
        data.forEach(function (ea) {
            var category = self.categories().find(f => f.categoryId() == ea.category);

            if (category == null) {
                category = new app.painel.model.Category(ea.category);
                self.categories.push(category);
            }

            var watcher = category.watchers().find(f => f.channelId() == ea.hubChannel);

            if (watcher == null) {
                watcher = new app.painel.model.Servico(ea);
                category.addWatcher(watcher);
            } else
                watcher.set(ea);
        });
    });

    connection.on("sendCharts", function (data) {
        console.log("Init chart: ", data);
        if (data == null || data == undefined || data.length == 0)
            return;

        self.chartEmpty(false);
        self.charts([]);
    });

    connection.on("notify", function (data) {
        console.log("Notify: ", data);
        $(".last-update").empty().append("<span>Ultima Atualização:</span> " + data.updateTime);
        var category = self.categories().find(f => f.categoryId() == data.category);

        if (category == null) {
            category = new app.painel.model.Category(data.category);
            self.categories.push(category);
        }
        ;

        var watcher = category.watchers().find(f => f.channelId() == data.hubChannel);

        if (watcher == null) {
            watcher = new app.painel.model.Servico(data);
            category.addWatcher(watcher);
        } else
            watcher.set(data);
    });

    connection.on("notifyUsuario", function (data) {
        console.log("Usuario: ", data);
        $(".last-update").empty().append("<span>Ultima Atualização:</span> " + data.updateTime);
        self.chartEmpty(false);
        var isStarted = false;

        self.charts().forEach(function (ea) {
            if (ea.type() == data.chartType) {
                ea.title(data.titulo);
                app.painel.apiChart.viewModel.startUsuario('idChart' + ea.type(), [data]);
                isStarted = true;
            }
        });

        if (isStarted)
            return;

        self.createChart([data]);
    });

    connection.on("notifyLogAPI", function (data) {
        console.log("LogAPI: ", data);
        $(".last-update").empty().append("<span>Ultima Atualização:</span> " + data.updateTime);
        self.chartEmpty(false);
        var isStarted = false;

        self.charts().forEach(function (ea) {
            if (ea.type() == data.chartType) {
                ea.title(data.titulo);
                app.painel.apiChart.viewModel.startLogAPI('idChart' + ea.type(), [data]);
                isStarted = true;
            }
        });

        if (isStarted)
            return;

        self.createChart([data]);
    });

    ko.applyBindings(self, $("#painel-container")[0]);

    return {
        collapseAll: collapseAll
    };
})($, ko);