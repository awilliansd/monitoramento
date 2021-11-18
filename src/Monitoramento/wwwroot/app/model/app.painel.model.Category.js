var app = app || {};
app.painel = app.painel || {};
app.painel.model = app.painel.model || {};

app.painel.model.Category = function (data) {
    if (data == null)
        return;

    var categoryId = ko.observable(data);
    var watchers = ko.observableArray();
    var titulo = ko.observable();
    var expand = ko.observable(false);

    var status = ko.computed(function () {
        if (watchers().some(s => s.status() == app.painel.enum.code.Fail))
            return app.painel.enum.code.Fail;
        else if (watchers().some(s => s.status() == app.painel.enum.code.Warning))
            return app.painel.enum.code.Warning;
        else if (watchers().some(s => s.status() == app.painel.enum.code.Static))
            return app.painel.enum.code.Static;
        else
            return app.painel.enum.code.OK;
    });

    status.subscribe(function (val) {
        if (val == app.painel.enum.code.Fail || val == app.painel.enum.code.Warning) {
            app.painel.viewModel.collapseAll();
            expand(true);
        } else if (val == app.painel.enum.code.OK) {
            expand(false);
        }
    });

    var watchersLength = ko.computed(function () {
        return watchers().length;
    });

    var cssAttr = ko.computed(function () {
        return {
            'bg-success': status() == app.painel.enum.code.OK,
            'bg-danger': status() == app.painel.enum.code.Fail,
            'bg-warning': status() == app.painel.enum.code.Warning,
            'text-white': status() != app.painel.enum.code.Static,
            'clickable': status() != app.painel.enum.code.Static,
            'card-custom-active': expand()
        };
    });

    var footerCssAttr = ko.computed(function () {
        return {
            'card-icon-clound-up': data == app.painel.enum.category.Internal,
            'card-icon-clound-down': data == app.painel.enum.category.External,
            'card-icon-clound-pga': data == app.painel.enum.category.ExternalPGA,
            'card-icon-gear': data == app.painel.enum.category.Service,
            'card-icon-database': data == app.painel.enum.category.Jobs
        };
    });

    if (data == app.painel.enum.category.Internal)
        titulo("Web Services Internos");
    else if (data == app.painel.enum.category.External)
        titulo("Web Services Externos");
    else if (data == app.painel.enum.category.ExternalPGA)
        titulo("PGA - Web Services Externos");
    else if (data == app.painel.enum.category.Users)
        titulo("Usuarios Logados");
    else if (data == app.painel.enum.category.Service)
        titulo("Serviços");
    else if (data == app.painel.enum.category.Jobs)
        titulo("Jobs");

    var addWatcher = function (data) {
        watchers.push(data);
    }

    var clickToExpand = function () {
        if (status() != app.painel.enum.code.Static) {
            var isExpand = expand();
            app.painel.viewModel.collapseAll();
            expand(!isExpand);
        }
    }

    return {
        categoryId: categoryId,
        titulo: titulo,
        watchers: watchers,
        expand: expand,
        status: status,
        watchersLength: watchersLength,
        addWatcher: addWatcher,
        clickToExpand: clickToExpand,
        cssAttr: cssAttr,
        footerCssAttr: footerCssAttr
    }
}