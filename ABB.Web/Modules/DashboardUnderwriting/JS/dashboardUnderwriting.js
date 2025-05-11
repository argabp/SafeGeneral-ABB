// $(document).ready(function () {
//     var dashboardData = JSON.parse($("#dashboardData").val());
//     var produksi = [];
//     var target = [];
//     var nm_cab = [];
//    
//     dashboardData.forEach(data => {
//         produksi.push((data.produksi_sdthnini / 1000000).toFixed(2));
//         target.push((data.target_rkap / 1000000).toFixed(2));
//         nm_cab.push(data.nm_cab);
//     });
//
//     var areaChartData = {
//         labels  : nm_cab,
//         datasets: [
//             {
//                 label               : 'Produksi',
//                 backgroundColor     : 'rgba(210, 214, 222, 1)',
//                 borderColor         : 'rgba(210, 214, 222, 1)',
//                 pointRadius         : false,
//                 pointColor          : 'rgba(210, 214, 222, 1)',
//                 pointStrokeColor    : '#c1c7d1',
//                 pointHighlightFill  : '#fff',
//                 pointHighlightStroke: 'rgba(220,220,220,1)',
//                 data                : produksi
//             },
//             {
//                 label               : 'Target',
//                 backgroundColor     : 'rgba(60,141,188,0.9)',
//                 borderColor         : 'rgba(60,141,188,0.8)',
//                 pointRadius          : false,
//                 pointColor          : '#3b8bba',
//                 pointStrokeColor    : 'rgba(60,141,188,1)',
//                 pointHighlightFill  : '#fff',
//                 pointHighlightStroke: 'rgba(60,141,188,1)',
//                 data                : target
//             }
//         ]
//     }
//    
//     var barChartCanvas = $('#barChart').get(0).getContext('2d')
//     var barChartData = $.extend(true, {}, areaChartData)
//     var temp0 = areaChartData.datasets[0]
//     var temp1 = areaChartData.datasets[1]
//     barChartData.datasets[0] = temp1
//     barChartData.datasets[1] = temp0
//
//     var barChartOptions = {
//         responsive              : true,
//         maintainAspectRatio     : false,
//         datasetFill             : false,
//         tooltips: {
//             callbacks: {
//                 label (t, d) {
//                     const xLabel = d.datasets[t.datasetIndex].label;
//                     const yLabel = t.yLabel >= 1000 ? t.yLabel.toString().replace(/\B(?=(\d{3})+(?!\d))/g, '.') : t.yLabel;
//                     return xLabel + ': ' + yLabel;
//                 }
//             }
//         },
//         scales: {
//             yAxes: [
//                 {
//                     ticks: {
//                         beginAtZero: true,
//                         callback: (label, index, labels) => {
//                             return label.toFixed(0).toString().replace(/\B(?=(\d{3})+(?!\d))/g, '.');
//                         }
//                     },
//                     scaleLabel: {
//                         display: true,
//                         labelString: 'Dalam Jutaan Rupiah'
//                     }
//                 }
//             ]
//         },
//         // plugins: [ChartDataLabels]
//         plugins: {
//             title: {
//                 display: true,
//                 text: 'Release Year of Web Frameworks',
//                 color: 'blue',
//                 font: {
//                     weight: 'bold',
//                     size: 20
//                 }
//             },
//             datalabels: {
//                 // Position of the labels 
//                 // (start, end, center, etc.)
//                 anchor: 'end',
//                 // Alignment of the labels 
//                 // (start, end, center, etc.)
//                 align: 'end',
//                 // Color of the labels
//                 color: 'blue',
//                 font: {
//                     weight: 'bold',
//                 },
//                 formatter: function (value, context) {
//                     // Display the actual data value
//                     return value;
//                 }
//             }
//         }
//     }
//
//     new Chart(barChartCanvas, {
//         type: 'bar',
//         data: barChartData,
//         options: barChartOptions
//     })
// });


$(document).ready(function () {
    var dashboardData = JSON.parse($("#dashboardData").val());
    var produksi = [];
    var target = [];
    var nm_cab = [];
    
    dashboardData.forEach(data => {
        produksi.push((data.produksi_sdthnini / 1000000));
        target.push((data.target_rkap / 1000000));
        nm_cab.push(data.nm_cab.replace(" ", "<br>"));
    });
    
    // ZC.LICENSE = ["569d52cefae586f634c54f86dc99e6a9", "b55b025e438fa8a98e32482b5f768ff5"];

    let chartConfig = {
        type: 'bar3d',
        '3dAspect': {
            depth: 30,
            true3d: 0,
            yAngle: 10,
        },
        backgroundColor: '#fff',
        // title: {
        //     text: 'TARGET DAN REALISASI PRODUKSI UMUM CABANG DAN PEMASARAN S/D TAHUN INI',
        //     fontWeight: 'bold',
        //     height: '40px',
        // },
        legend: {
            backgroundColor: 'none',
            borderColor: 'none',
            item: {
                fontColor: '#333',
            },
            layout: 'float',
            shadow: false,
            width: '90%',
            x: '50%',
            y: '10%',
        },
        plotarea: {
            alpha: 0.3,
            backgroundColor: '#fff',
            margin: '95px 35px 50px 100px',
        },
        plot: {
            valueBox: {
                    thousandsSeparator: ".",
                    decimalsSeparator: ",",
                    offsetY : '-10px'
            },
        },
        scaleX: {
            values: nm_cab,
            alpha: 0.5,
            backgroundColor: '#fff',
            borderColor: '#333',
            borderWidth: '1px',
            guide: {
                visible: false,
            },
            itemOverlap: true,
            item: {
                fontColor: '#333',
                offsetX : '3px',
                fontSize: '11px'
            },
            tick: {
                alpha: 0.2,
                lineColor: '#333',
            },
            shortUnit: "",
            thousandsSeparator: ".",
            decimalsSeparator: ","
        },
        scaleY: {
            label: {
                text: 'Dalam Jutaan Rupiah'
            },
            alpha: 0.5,
            backgroundColor: '#fff',
            borderColor: '#333',
            borderWidth: '1px',
            format: '%v',
            guide: {
                alpha: 0.2,
                lineColor: '#333',
                lineStyle: 'solid',
            },
            item: {
                fontColor: '#333',
                paddingRight: '6px',
            },
            tick: {
                alpha: 0.2,
                lineColor: '#333',
            },
            thousandsSeparator: ".",
            decimalsSeparator: ","
        },
        series: [{
            text: 'Target',
            values: target,
            decimals: 2,
            tooltip: {
                text: 'Target %v',
                backgroundColor: '#03A9F4',
                borderColor: 'none',
                borderRadius: '5px',
                fontSize: '12px',
                padding: '6px 12px',
                shadow: false,
                thousandsSeparator: ".",
                decimalsSeparator: ","
            },
            backgroundColor: '#03A9F4 #4FC3F7',
            borderColor: '#03A9F4',
            legendMarker: {
                borderColor: '#03A9F4',
            },
        },
            {
                text: 'Produksi',
                values: produksi,
                decimals: 2,
                tooltip: {
                    text: 'Produksi %v',
                    backgroundColor: '#673AB7',
                    borderColor: 'none',
                    borderRadius: '5px',
                    fontSize: '12px',
                    padding: '6px 12px',
                    shadow: false,
                    thousandsSeparator: ".",
                    decimalsSeparator: ","
                },
                backgroundColor: '#673AB7 #9575CD',
                borderColor: '#673AB7',
                legendMarker: {
                    borderColor: '#673AB7',
                },
            },
        ],
    };

    zingchart.render({
        id: 'barChart',
        data: chartConfig,
        height: '90%',
        width: '100%',
        defaults: {
            fontFamily: 'sans-serif',
        },
    });
    
    setTimeout(() => {
        $("#barChart-license-text").hide();
    }, 5000);
});
