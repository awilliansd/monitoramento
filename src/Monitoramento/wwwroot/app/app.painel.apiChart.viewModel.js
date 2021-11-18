var app = app || {};
app.painel = app.painel || {};
app.painel.apiChart = app.painel.apiChart || {};

app.painel.apiChart.color = [
    {
        backgroundColor: 'rgba(6, 62, 85, 0.5)',
        borderColor: 'rgba(6, 62, 85, 1)'
    },
    {
        backgroundColor: 'rgba(40, 167, 69, 0.5)',
        borderColor: 'rgba(40, 167, 69, 1)'
    }
];

app.painel.apiChart.viewModel = (function ($, ko) {

    var startUsuario = function (el, data) {
        console.log("StartUsuario: " + data);
        var series = [];
        data = data[0];
        if (data && data.usuarios)
            data.usuarios.forEach((ea) => {
                series.push({
                    name: ea.perfil,
                    data: ea.execucoes
                });
            });

        Highcharts.chart(el, {
            chart: {
                type: 'areaspline'
            },
            title: {
                text: data.titulo
            },
            subtitle: {
                text: data.subTitulo
            },
            xAxis: {
                categories: data.xLegenda,
                allowDecimals: false,
                tickmarkPlacement: 'on',
                title: {
                    enabled: false
                }
            },
            yAxis: {
                title: {
                    text: 'Quantidade de Acessos'
                },
                labels: {
                    formatter: function () {
                        return this.value;
                    }
                }
            },
            plotOptions: {
                area: {
                    pointStart: 0,
                    marker: {
                        enabled: false,
                        symbol: 'circle',
                        radius: 2,
                        states: {
                            hover: {
                                enabled: true
                            }
                        }
                    }
                }
            },
            tooltip: {
                shared: true,
                valueSuffix: ' Acessos'
            },
            credits: {
                enabled: false
            },
            plotOptions: {
                areaspline: {
                    fillOpacity: 0.5
                }
            },
            series: series
        });


    }

    var startLogAPI = function (el, data) {
        console.log("StartLogAPI: " + data);
        var series = [];
        data = data[0];
        if (data && data.requisicoes)
            data.requisicoes.forEach((ea) => {
                series.push({
                    name: ea.legenda,
                    data: ea.execucoes
                });
            });

        Highcharts.chart(el, {
            chart: {
                type: 'areaspline'
            },
            title: {
                text: data.titulo
            },
            subtitle: {
                text: data.subTitulo
            },
            xAxis: {
                categories: data.xLegenda,
                allowDecimals: false,
                labels: {
                    formatter: function () {
                        return this.value; // clean, unformatted number for year
                    }
                }
            },
            yAxis: {
                title: {
                    text: 'Quantidade de Requisições'
                }
            },
            plotOptions: {
                area: {
                    pointStart: 0,
                    marker: {
                        enabled: false,
                        symbol: 'circle',
                        radius: 2,
                        states: {
                            hover: {
                                enabled: true
                            }
                        }
                    }
                }
            },
            tooltip: {
                shared: true,
                valueSuffix: ' Requisições'
            },
            credits: {
                enabled: false
            },
            plotOptions: {
                areaspline: {
                    fillOpacity: 0.5
                }
            },
            series: series
        });
    };

    return {
        startUsuario: startUsuario,
        startLogAPI: startLogAPI
    };
})($, ko);